using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DraggableList))]
public class InstructionExecutor : MonoBehaviour
{

    public ClickEventEmitter playButton;

    public Instructable target;

    private List<Instruction> instructions = new List<Instruction>();

    private int instructionIndex;
    private Instruction currentInstruction;

    private DraggableList draggableList;

    void Start()
    {
        draggableList = GetComponent<DraggableList>();
        draggableList.AllowedItems = new List<System.Type>
        {
            typeof(Instruction)
        };
        draggableList.Rearrangeable = true;
        draggableList.CopyOnDrag = false;

        playButton.SetEventHandler(Play);

        // var moveInstruction = new MoveInstruction(new Vector3(1, 1, 0), 2, MoveType.Relative);
        // var jumpTarget = new JumpTargetInstruciton();
        // var jump = new JumpInstruction(jumpTarget);

        // instructions.Add(jumpTarget);
        // instructions.Add(moveInstruction);
        // instructions.Add(jump);

        // Play();
    }

    public void JumpToInstruction(Instruction instruction)
    {
        instructionIndex = instructions.IndexOf(instruction);
    }

    public void Play()
    {
        instructionIndex = 0;
        instructions = draggableList.ListItems.Reverse().Select(l => l.GetComponent<Instruction>()).ToList();
        ExecuteNextInstruction();
    }

    public void ExecuteNextInstruction()
    {
        if (instructionIndex > instructions.Count - 1)
        {
            currentInstruction = null;
            return;
        }
        
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
            currentInstruction.UpdateInstruction();
        }
    }
}
