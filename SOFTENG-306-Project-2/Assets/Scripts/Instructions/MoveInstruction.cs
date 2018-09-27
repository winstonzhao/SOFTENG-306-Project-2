using UnityEngine;

public class MoveInstruction : IInstruction
{
    private InstructionExecutor instructionExecutor;
    private Instructable target;

    float t;

    private Vector3 start;
    private Vector3 end;

    private float time;

    private bool moving;

    public MoveInstruction(Vector3 point, float seconds)
    {
        t = 0;
        end = point;
        time = seconds;
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

        start = target.transform.position;
        moving = true;
    }

}