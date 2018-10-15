using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Instructions
{
    // [RequireComponent(typeof(DraggableList))]
    public class InstructionExecutor : MonoBehaviour
    {
        private static string PLAY_REGULAR = "green_sliderRight.png";
        private static string PLAY_RESET = "green_sliderLeft.png";
        private static Sprite[] SPRITE_SHEET;

        private InstructionObj currentInstruction;

        private GenericDraggableList draggableList;

        private bool executeNext;

        private float executionStart = float.MinValue;

        private int instructionIndex;

        private List<InstructionObj> instructions = new List<InstructionObj>();

        public ClickEventEmitter playButton;

        public ClickEventEmitter stopButton;

        public RobotController target;

        public Text Text;

        public SoftwareLevelGenerator LevelGenerator;

        public Text errorText;

        private bool playing = false;
        private bool reset = true;

        private void Start()
        {
            if (target == null)
            {
                target = FindObjectOfType<RobotController>();
            }

            SPRITE_SHEET = Resources.LoadAll<Sprite>("software_minigame/Sprites/greenSheet");
            draggableList = GetComponent<GenericDraggableList>();
            // draggableList.AllowedItems = new List<System.Type>
            // {
            //     typeof(Instruction)
            // };
            // draggableList.Rearrangeable = true;
            // draggableList.CopyOnDrag = false;

            playButton.EventHandler += Play;
            stopButton.EventHandler += Stop;

            if (LevelGenerator == null)
            {
                LevelGenerator = FindObjectOfType<SoftwareLevelGenerator>();
            }
        }

        public Sprite GetSpriteByName(string name)
        {
            for (int i = 0; i < SPRITE_SHEET.Length; i++)
            {
                if (SPRITE_SHEET[i].name == name)
                    return SPRITE_SHEET[i];
            }

            return null;
        }

        public void JumpToInstruction(Instruction instruction)
        {
            instructionIndex = instructions.FindIndex(i => i.Instruction == instruction);
        }

        public void Play()
        {
            if (playing) return;

            if (!reset)
            {
                // Reset the level
                target.ResetPos();
                LevelGenerator.GeneratedLevel(LevelGenerator.currentLevel);
                reset = true;
                playButton.GetComponent<Image>().sprite = GetSpriteByName(PLAY_REGULAR);
                foreach (var instruction in instructions) instruction.Renderer.ResetStyle();
                return;
            }

            reset = false;
            playing = true;
            Text.text = "Playing";
            foreach (var instruction in instructions) instruction.Instruction.Editable = false;

            instructionIndex = 0;
            instructions = draggableList.ListItems.Reverse().Select(l => new InstructionObj
            {
                Instruction = l.GetComponent<Instruction>(),
                Renderer = l.GetComponent<InstructionRenderer>(),
                Draggable =  l.GetComponent<Draggable>()
            }).ToList();

            ExecuteNextInstruction();
        }

        public void ExecuteNextInstruction()
        {
            executeNext = true;
        }

        private void DoExecute()
        {
            if (currentInstruction != null)
            {
                currentInstruction.Renderer.BackgroundColor = currentInstruction.Renderer.DefaultBackgroundColor;
            }

            if (instructionIndex > instructions.Count - 1)
            {
                if (currentInstruction != null)
                {
                    currentInstruction.Renderer.BackgroundColor = currentInstruction.Renderer.DefaultBackgroundColor;
                    currentInstruction = null;
                }
                Stop();
                return;
            }

            currentInstruction = instructions[instructionIndex];

            currentInstruction.Renderer.BackgroundColor = InstructionRenderer.ExecutingBackgroundColor;
            draggableList.Highlight(currentInstruction.Draggable);

            try
            {
                currentInstruction.Instruction.Execute(target, this);
            }
            catch (InstructionException e)
            {
                FailExecution(e.Message);
            }

            instructionIndex++;
            executeNext = false;
        }

        public void Stop()
        {
            playButton.GetComponent<Image>().sprite = GetSpriteByName(PLAY_RESET);
            playing = false;

            Text.text = "Stop";
            executeNext = false;
            if (currentInstruction != null)
            {
                currentInstruction.Renderer.BackgroundColor = InstructionRenderer.ExecutingBackgroundColor;
            }

            currentInstruction = null;
            foreach (var instruction in instructions) instruction.Instruction.Editable = true;
        }

        public void FailExecution(string message)
        {
            if (currentInstruction != null)
            {
                currentInstruction.Renderer.BackgroundColor = InstructionRenderer.FailBackgroundColor;
                currentInstruction.Renderer.Render();
                currentInstruction = null;
            }

            errorText.text = "<b>" + message + "</b>";
            StartCoroutine(AnimateErrorText());

            Stop();
            Debug.Log("Instruction Exception: " + message);
            Text.text = "Failed";
        }

        private IEnumerator AnimateErrorText()
        {
            while (errorText.rectTransform.anchoredPosition.y < errorText.rectTransform.sizeDelta.y/2 + 10)
            {
                errorText.rectTransform.anchoredPosition +=
                    new Vector2(0, errorText.rectTransform.sizeDelta.y * 4) * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(1.0f);

            while (errorText.rectTransform.anchoredPosition.y > - errorText.rectTransform.sizeDelta.y/2)
            {
                errorText.rectTransform.anchoredPosition +=
                    new Vector2(0, -errorText.rectTransform.sizeDelta.y * 4) * Time.deltaTime;
                yield return null;
            }
        }

        private void Update()
        {
            if (currentInstruction != null && !executeNext) currentInstruction.Instruction.UpdateInstruction();

            if (currentInstruction != null && Time.time - executionStart < currentInstruction.Instruction.MinTiming)
            {
                return;
            }

            if (executeNext)
            {
                executionStart = Time.time;
                DoExecute();
            }
        }

        private class InstructionObj
        {
            public Instruction Instruction { get; set; }
            public InstructionRenderer Renderer { get; set; }
            public Draggable Draggable { get; set; }
        }
    }
}
