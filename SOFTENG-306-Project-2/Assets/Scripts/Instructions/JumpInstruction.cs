using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JumpInstruction : Instruction
{
    private InstructionExecutor instructionExecutor;
    private JumpTargetInstruction jumpTarget;

    public override bool Editable { get; set; }

    public override List<InstructionComponent> InstructionComponents
    {
        get
        {
            return new List<InstructionComponent> 
            {
                new InstructionComponent("Jump")
            };
        }
    }

    void Start()
    {
        
        var prefab = Resources.Load<GameObject>("Prefabs/JumpTargetUI");
        var gameObj = Instantiate<GameObject>(prefab);

        gameObj.GetComponent<RectTransform>().SetParent(FindObjectOfType<Canvas>().transform, false);

        jumpTarget = gameObj.GetComponent<JumpTargetInstruction>();
        jumpTarget.transform.position = transform.position;

        var targetDraggable = gameObj.GetComponent<DraggableItem>();

        var targetRenderer = targetDraggable.GetComponent<InstructionRenderer>();
        targetRenderer.IsEnabled = false;

        var draggableItem = GetComponent<DraggableItem>();
        draggableItem.AddConnectedItem(targetDraggable);
        draggableItem.OnDropZoneChanged += d => targetRenderer.IsEnabled = d != null;
    }

    public override void Execute(Instructable target, InstructionExecutor executor)
    {
        instructionExecutor = executor;
    }

    public override void UpdateInstruction()
    {
        instructionExecutor.JumpToInstruction(jumpTarget);
        instructionExecutor.ExecuteNextInstruction();
    }
}