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

        public float MinExecutionSeconds = 0.4f;

        public ClickEventEmitter playButton;

        private Color prevBackground;
        public ClickEventEmitter stopButton;

        public RobotController target;

        public Text Text;

        public SoftwareLevelGenerator LevelGenerator;

        private bool playing = false;
        private bool reset = true;

        private void Start()
        {
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
                Renderer = l.GetComponent<InstructionRenderer>()
            }).ToList();

            ExecuteNextInstruction();
        }

        public void ExecuteNextInstruction()
        {
            executeNext = true;
        }

        private void DoExecute()
        {
            if (currentInstruction != null) currentInstruction.Renderer.BackgroundColor = prevBackground;

            if (instructionIndex > instructions.Count - 1)
            {
                Stop();
                return;
            }

            currentInstruction = instructions[instructionIndex];

            prevBackground = currentInstruction.Renderer.BackgroundColor;
            currentInstruction.Renderer.BackgroundColor = new Color(0, 1, 0.0f);

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
            if (currentInstruction != null) currentInstruction.Renderer.BackgroundColor = prevBackground;

            currentInstruction = null;
            foreach (var instruction in instructions) instruction.Instruction.Editable = true;
        }

        public void FailExecution(string message)
        {
            if (currentInstruction != null) currentInstruction.Renderer.TextColor = new Color(1, 0, 0);
            Stop();
            Debug.Log("Instruction Exception: " + message);
            Text.text = "Failed";
        }

        private void Update()
        {
            if (currentInstruction != null && !executeNext) currentInstruction.Instruction.UpdateInstruction();

            if (executeNext && Time.time - executionStart > MinExecutionSeconds)
            {
                executionStart = Time.time;
                DoExecute();
            }
        }

        private class InstructionObj
        {
            public Instruction Instruction { get; set; }
            public InstructionRenderer Renderer { get; set; }
        }
    }
}
