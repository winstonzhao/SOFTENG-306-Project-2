using UnityEngine;

public enum MoveType
{
    Absolute, Relative
}

public class MoveInstruction : IInstruction
{
    private InstructionExecutor instructionExecutor;
    private Instructable target;

    float t;

    private Vector3 start;
    private Vector3 end;

    private Vector3 moveTarget;

    private float time;

    private bool moving;

    private MoveType moveType;

    public MoveInstruction(Vector3 point, float seconds, MoveType moveType = MoveType.Absolute)
    {
        moveTarget = point;
        time = seconds;
        this.moveType = moveType;
    }

    public void Update()
    {
        if (moving) 
        {
            t += Time.deltaTime/time;

            target.transform.position = Vector3.Lerp(start, end, t);

            if (t >= 1)
            {
                moving = false;
                instructionExecutor.ExecuteNextInstruction();
            }
        }
    }

    public void Execute(Instructable target, InstructionExecutor executor)
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