using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Instruction : MonoBehaviour
{

    // Called once each time the instruction starts to be executed
    public abstract void Execute(Instructable target, InstructionExecutor executor);

    // Called every update while the instruction is the current executing instruction
    // (Move to the next instruction with executor.ExecuteNextInstruction() when done)
    public abstract void UpdateInstruction();

    public abstract string Name { get; }

}
