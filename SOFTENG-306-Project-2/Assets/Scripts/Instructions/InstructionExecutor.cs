using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityScript.Macros;

namespace Instructions
{
    public class InstructionExecutor : MonoBehaviour
    {
        private static string PLAY_REGULAR = "green_sliderRight.png";
        private static string PLAY_RESET = "green_sliderLeft.png";
        private static Sprite[] SPRITE_SHEET;

        private GenericDraggableList draggableList;

        private bool executeNext;

        private float executionStart = float.MinValue;

        private int instructionIndex;
        private List<InstructionObj> instructions = new List<InstructionObj>();
        private InstructionObj currentInstruction;

        public ClickEventEmitter playButton;
        public ClickEventEmitter stopButton;

        public RobotController target;

        public Text StatusText;
        public Text ErrorText;

        public SoftwareLevelGenerator LevelGenerator;

        private bool playing = false;
        private bool reset = true;


        // Sets up some text to be used in case of an error
        private void SetupErrorText()
        {
            var go = new GameObject("Error text");

            var rectTransform = go.AddComponent<RectTransform>();
            rectTransform.SetParent(GetComponentInParent<Canvas>().transform, false);

            rectTransform.anchorMin = new Vector2(0.5f, 0);
            rectTransform.anchorMax = new Vector2(0.5f, 0);

            // Resize component to size of text automatically
            var sizeFitter = go.AddComponent<ContentSizeFitter>();
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            ErrorText = go.AddComponent<Text>();
            ErrorText.fontSize = 40;
            ErrorText.color = new Color(1, 1, 1);
            ErrorText.font = Font.CreateDynamicFontFromOSFont("Arial", 40);

            rectTransform.anchoredPosition = new Vector2(0, -rectTransform.sizeDelta.y / 2);
        }

        private void Start()
        {
            if (target == null)
            {
                target = FindObjectOfType<RobotController>();
            }

            if (ErrorText == null)
            {
                SetupErrorText();
            }

            SPRITE_SHEET = Resources.LoadAll<Sprite>("software_minigame/Sprites/greenSheet");
            draggableList = GetComponent<GenericDraggableList>();

            playButton.EventHandler += Play;
            stopButton.EventHandler += Stop;

            stopButton.GetComponent<Image>().color = new Color(.6f, .6f,.6f);

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

        /// <summary>
        /// Goes to the given instruction and will continue execution from there
        /// </summary>
        public void JumpToInstruction(Instruction instruction)
        {
            instructionIndex = instructions.FindIndex(i => i.Instruction == instruction);
        }

        /// <summary>
        /// Starts execution from the beginning
        /// If it has not been reset, will reset first
        /// </summary>
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

                // Change all instructions back to default
                foreach (var instruction in instructions) instruction.Renderer.ResetStyle();
                return;
            }

            // Grey out stop button
            stopButton.GetComponent<Image>().color = new Color(1, 1, 1);

            reset = false;
            playing = true;
            StatusText.text = "Playing";
            foreach (var instruction in instructions) instruction.Instruction.Editable = false;

            // Load all instructions from draggable list
            instructionIndex = 0;
            instructions = draggableList.ListItems.Reverse().Select(l => new InstructionObj
            {
                Instruction = l.GetComponent<Instruction>(),
                Renderer = l.GetComponent<InstructionRenderer>(),
                Draggable =  l.GetComponent<Draggable>()
            }).ToList();

            ExecuteNextInstruction();
        }

        /// <summary>
        /// Will queue the next instruction in the list to be executed
        /// </summary>
        public void ExecuteNextInstruction()
        {
            executeNext = true;
        }

        private void DoExecute()
        {
            if (currentInstruction != null)
            {
                // Reset current instruction
                currentInstruction.Renderer.BackgroundColor = currentInstruction.Renderer.DefaultBackgroundColor;
            }

            // Reached end of instructions
            if (instructionIndex > instructions.Count - 1)
            {
                currentInstruction = null;
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

        /// <summary>
        /// Stops the execution at the end of the current instruction
        /// </summary>
        public void Stop()
        {
            if (!playing) return;

            playButton.GetComponent<Image>().sprite = GetSpriteByName(PLAY_RESET);
            stopButton.GetComponent<Image>().color = new Color(.6f, .6f,.6f);
            playing = false;

            StatusText.text = "Stop";
            executeNext = false;
            if (currentInstruction != null)
            {
                currentInstruction.Renderer.BackgroundColor = InstructionRenderer.ExecutingBackgroundColor;
            }

            currentInstruction = null;
            foreach (var instruction in instructions) instruction.Instruction.Editable = true;
        }

        /// <summary>
        /// To be called when an instruction has failed
        /// Will show some error text to the user with the given message
        /// </summary>
        public void FailExecution(string message)
        {
            if (currentInstruction != null)
            {
                currentInstruction.Renderer.BackgroundColor = InstructionRenderer.FailBackgroundColor;
                currentInstruction.Renderer.Render();
                currentInstruction = null;
            }

            ErrorText.text = "<b>" + message + "</b>";
            StartCoroutine(AnimateErrorText());

            Stop();
            Debug.Log("Instruction Exception: " + message);
            StatusText.text = "Failed";
        }

        private IEnumerator AnimateErrorText()
        {
            // Move up
            while (ErrorText.rectTransform.anchoredPosition.y < ErrorText.rectTransform.sizeDelta.y/2 + 10)
            {
                ErrorText.rectTransform.anchoredPosition +=
                    new Vector2(0, ErrorText.rectTransform.sizeDelta.y * 4) * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(1.0f);

            // Move down
            while (ErrorText.rectTransform.anchoredPosition.y > - ErrorText.rectTransform.sizeDelta.y/2)
            {
                ErrorText.rectTransform.anchoredPosition +=
                    new Vector2(0, -ErrorText.rectTransform.sizeDelta.y * 4) * Time.deltaTime;
                yield return null;
            }
        }

        private void Update()
        {
            // Update the current instruction
            if (currentInstruction != null && !executeNext) currentInstruction.Instruction.UpdateInstruction();

            // Keep updating until it has finished or the min time has finished
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
