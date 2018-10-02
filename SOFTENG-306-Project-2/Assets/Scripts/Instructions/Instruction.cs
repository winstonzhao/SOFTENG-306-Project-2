using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Instructions
{
    public class InstructionComponent
    {
        public delegate void ComponnentClicked(object args);

        public InstructionComponent(string text)
        {
            Text = text;
        }

        public ComponnentClicked OnComponentClicked { get; set; }
        public string Text { get; set; }
    }

    public class DropdownInstructionComponent : InstructionComponent
    {
        public List<string> values = new List<string> {"up", "down", "left", "right"};

        public DropdownInstructionComponent(string text) : base(text)
        {
        }
    }

    public abstract class Instruction : MonoBehaviour
    {
        public abstract ReadOnlyCollection<InstructionComponent> InstructionComponents { get; }

        public abstract bool Editable { get; set; }

        // Called once each time the instruction starts to be executed
        public abstract void Execute(Instructable target, InstructionExecutor executor);

        // Called every update while the instruction is the current executing instruction
        // (Move to the next instruction with executor.ExecuteNextInstruction() when done)
        public abstract void UpdateInstruction();
    }
}