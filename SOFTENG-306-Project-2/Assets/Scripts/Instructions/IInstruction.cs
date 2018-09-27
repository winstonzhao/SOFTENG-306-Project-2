using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInstruction
{

    // Called once each time the instruction starts to be executed
    void Execute(Instructable target, InstructionExecutor executor);

    // Called every update while the instruction is the current executing instruction
    // (Move to the next instruction with executor.ExecuteNextInstruction() when done)
    void Update();

}
