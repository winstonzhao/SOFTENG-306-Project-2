using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionExecutor : MonoBehaviour
{

    public Instructable target;

    private List<IInstruction> instructions = new List<IInstruction>();

    private int instructionIndex;
    private IInstruction currentInstruction;

    void Start()
    {
        var moveInstruction = new MoveInstruction(new Vector3(1, 1, 0), 2, MoveType.Relative);
        var jumpTarget = new JumpTargetInstruciton();
        var jump = new JumpInstruction(jumpTarget);

        instructions.Add(jumpTarget);
        instructions.Add(moveInstruction);
        instructions.Add(jump);

        Play();
    }

    public void JumpToInstruction(IInstruction instruction)
    {
        instructionIndex = instructions.IndexOf(instruction);
    }

    public void Play()
    {
        instructionIndex = 0;
        ExecuteNextInstruction();
    }

    public void ExecuteNextInstruction()
    {
        if (instructionIndex > instructions.Count) return;
        
        currentInstruction = instructions[instructionIndex];
        currentInstruction.Execute(target, this);
        instructionIndex++;
    }

    public void FailExecution(string message)
    {
        Debug.Log("Instruction Exception: " + message);
    }

    void Update()
    {
        if (currentInstruction != null)
        {
            currentInstruction.Update();
        }
    }
}
