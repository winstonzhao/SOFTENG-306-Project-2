using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Instructions
{
    // [RequireComponent(typeof(DraggableList))]
    public class InstructionExecutor : MonoBehaviour
    {
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

        private void Start()
        {
            draggableList = GetComponent<GenericDraggableList>();
            // draggableList.AllowedItems = new List<System.Type>
            // {
            //     typeof(Instruction)
            // };
            // draggableList.Rearrangeable = true;
            // draggableList.CopyOnDrag = false;

            playButton.EventHandler += Play;
            stopButton.EventHandler += Stop;
        }

        public void JumpToInstruction(Instruction instruction)
        {
            instructionIndex = instructions.FindIndex(i => i.Instruction == instruction);
        }

        public void Play()
        {
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
            executeNext = false;
            if (currentInstruction != null) currentInstruction.Renderer.BackgroundColor = prevBackground;

            currentInstruction = null;
            foreach (var instruction in instructions) instruction.Instruction.Editable = true;
        }

        public void FailExecution(string message)
        {
            Stop();
            Debug.Log("Instruction Exception: " + message);
            stopButton.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
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
