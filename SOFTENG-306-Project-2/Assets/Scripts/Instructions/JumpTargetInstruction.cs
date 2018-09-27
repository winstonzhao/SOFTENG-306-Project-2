using UnityEngine;

public class JumpTargetInstruciton : IInstruction
{
    private InstructionExecutor instructionExecutor;
    private Instructable target;

    public void Execute(Instructable target, InstructionExecutor executor)
    {
        instructionExecutor = executor;
        this.target = target;
    }

    public void Update()
    {
        instructionExecutor.ExecuteNextInstruction();
    }
}