using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DraggableList))]
public class InstructionExecutor : MonoBehaviour
{
    class InstructionObj
    {
        public Instruction Instruction { get; set; }
        public InstructionRenderer Renderer { get; set; }
    }

    public ClickEventEmitter playButton;
    public ClickEventEmitter stopButton;

    public Instructable target;

    private List<InstructionObj> instructions = new List<InstructionObj>();

    private int instructionIndex;
    private InstructionObj currentInstruction;

    private DraggableList draggableList;

    private Color prevBackground;

    public float MinExecutionSeconds = 0.4f;

    private float executionStart = float.MinValue;

    private bool executeNext = false;

    void Start()
    {
        draggableList = GetComponent<DraggableList>();
        draggableList.AllowedItems = new List<System.Type>
        {
            typeof(Instruction)
        };
        draggableList.Rearrangeable = true;
        draggableList.CopyOnDrag = false;

        playButton.EventHandler += Play;
        stopButton.EventHandler += Stop;
    }

    public void JumpToInstruction(Instruction instruction)
    {
        instructionIndex = instructions.FindIndex(i => i.Instruction == instruction);
    }

    public void Play()
    {
        foreach (var instruction in instructions)
        {
            instruction.Instruction.Editable = false;
        }

        instructionIndex = 0;
        instructions = draggableList.ListItems.Reverse().Select(l => new InstructionObj() 
        {
            Instruction = l.GetComponent<Instruction>(),
            Renderer = l.GetComponent<InstructionRenderer>()
        }).ToList();
        
        ExecuteNextInstruction();
    }

    public void ExecuteNextInstruction()
    {
        executeNext = true;
    }

    private void DoExecute()
    {
        if (currentInstruction != null) 
        {
            currentInstruction.Renderer.BackgroundColor = prevBackground;
        }

        if (instructionIndex > instructions.Count - 1)
        {
            Stop();
            return;
        }
        
        currentInstruction = instructions[instructionIndex];
        
        prevBackground = currentInstruction.Renderer.BackgroundColor;
        currentInstruction.Renderer.BackgroundColor = new Color(0, 1, 0.0f);

        currentInstruction.Instruction.Execute(target, this);
        instructionIndex++;
        executeNext = false;
    }

    public void Stop()
    {
        if (currentInstruction != null) 
        {
            currentInstruction.Renderer.BackgroundColor = prevBackground;
        }
        
        currentInstruction = null;
        foreach (var instruction in instructions)
        {
            instruction.Instruction.Editable = true;
        }
    }

    public void FailExecution(string message)
    {
        Debug.Log("Instruction Exception: " + message);
    }

    void Update()
    {
        if (currentInstruction != null && !executeNext)
        {
            currentInstruction.Instruction.UpdateInstruction();
        }

        if (executeNext && Time.time - executionStart > MinExecutionSeconds)
        {
            executionStart = Time.time;
            DoExecute();
        }
    }
}
