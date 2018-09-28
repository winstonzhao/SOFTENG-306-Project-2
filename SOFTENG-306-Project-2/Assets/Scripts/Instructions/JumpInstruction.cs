using UnityEngine;

public class JumpInstruction : Instruction
{
    private InstructionExecutor instructionExecutor;
    private Instructable target;

    public JumpTargetInstruction jumpTarget;

    public override string Name
    {
        get
        {
            return "Jump";
        }
    }

    public override void Execute(Instructable target, InstructionExecutor executor)
    {
        instructionExecutor = executor;
        this.target = target;
    }

    public override void UpdateInstruction()
    {
        instructionExecutor.JumpToInstruction(jumpTarget);
        instructionExecutor.ExecuteNextInstruction();
    }
}