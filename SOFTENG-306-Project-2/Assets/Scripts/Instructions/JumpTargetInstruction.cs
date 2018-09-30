using System.Collections.Generic;
using UnityEngine;

public class JumpTargetInstruction : Instruction
{
    private InstructionExecutor instructionExecutor;

    public override List<InstructionComponent> InstructionComponents
    {
        get
        {
            return new List<InstructionComponent> 
            {
                new InstructionComponent("END JUMP")
            };
        }
    }


    public override void Execute(Instructable target, InstructionExecutor executor)
    {
        instructionExecutor = executor;
    }

    public override void UpdateInstruction()
    {
        instructionExecutor.ExecuteNextInstruction();
    }
}