using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionComponent
{
    public delegate void ComponnentClicked();
    public ComponnentClicked OnComponentClicked { get; set; }
    public string Text { get; set; }

    public InstructionComponent(string text)
    {
        Text = text;
    }
}

public abstract class Instruction : MonoBehaviour
{

    // Called once each time the instruction starts to be executed
    public abstract void Execute(Instructable target, InstructionExecutor executor);

    // Called every update while the instruction is the current executing instruction
    // (Move to the next instruction with executor.ExecuteNextInstruction() when done)
    public abstract void UpdateInstruction();

    public abstract List<InstructionComponent> InstructionComponents { get; }

}
