using UnityEngine;

public class JumpInstruction : IInstruction
{
    private InstructionExecutor instructionExecutor;
    private Instructable target;

    private JumpTargetInstruciton jumpTarget;

    public JumpInstruction(JumpTargetInstruciton jumpTarget)
    {
        this.jumpTarget = jumpTarget;
    }

    public void Execute(Instructable target, InstructionExecutor executor)
    {
        instructionExecutor = executor;
        this.target = target;
    }

    public void Update()
    {
        instructionExecutor.JumpToInstruction(jumpTarget);
        instructionExecutor.ExecuteNextInstruction();
    }
}