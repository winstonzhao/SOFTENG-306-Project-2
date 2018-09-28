using UnityEngine;

public class JumpTargetInstruction : Instruction
{
    private InstructionExecutor instructionExecutor;

    public override string Name
    {
        get
        {
            return "END JUMP";
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