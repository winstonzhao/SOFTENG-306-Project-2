using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    Absolute, Relative
}

public class MoveInstruction : Instruction
{
    private InstructionExecutor instructionExecutor;
    private Instructable target;

    float t;

    private Vector3 start;
    private Vector3 end;

    public Vector3 moveTarget;

    public float seconds;

    private bool moving;

    public MoveType moveType;

    public override List<InstructionComponent> InstructionComponents
    {
        get
        {
            return new List<InstructionComponent> 
            {
                new InstructionComponent("Move"),
                new InstructionComponent(moveTarget.x.ToString()),
                new InstructionComponent(moveTarget.y.ToString())
            };
        }
    }

    public override void UpdateInstruction()
    {
        if (moving) 
        {
            t += Time.deltaTime/seconds;

            target.transform.position = Vector3.Lerp(start, end, t);

            if (t >= 1)
            {
                moving = false;
                instructionExecutor.ExecuteNextInstruction();
            }
        }
    }

    public override void Execute(Instructable target, InstructionExecutor executor)
    {
        instructionExecutor = executor;
        this.target = target;

        t = 0;
        start = target.transform.position;
        end = moveTarget;

        if (moveType == MoveType.Relative)
        {
            end += start;
        }

        moving = true;
    }

}