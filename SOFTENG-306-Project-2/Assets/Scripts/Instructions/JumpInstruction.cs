using UnityEditor;
using UnityEngine;

public class JumpInstruction : Instruction
{
    private InstructionExecutor instructionExecutor;
    private Instructable target;

    private JumpTargetInstruction jumpTarget;

    public override string Name
    {
        get
        {
            return "Jump";
        }
    }

    void Start()
    {
        var prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/JumpTarget.prefab", typeof(GameObject));
        var gameObj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        jumpTarget = gameObj.GetComponent<JumpTargetInstruction>();
        GetComponent<DraggableItem>().AddConnectedItem(gameObj.GetComponent<DraggableItem>());
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