using UnityEngine;

[ExecuteInEditMode]
public class InstructionRenderer : MonoBehaviour
{
    public Instruction instruction;

    void Start()
    {
        UpdateSize();
    }

    void OnEnable()
    {
        if (instruction == null)
            instruction = FindObjectOfType<Instruction>();
        UpdateSize();
    }


    public void UpdateSize()
    {
        var text = transform.GetChild(1);
        var square = transform.GetChild(0);

        text.GetComponent<TextMesh>().text = instruction.Name;

        var width = text.GetComponent<MeshRenderer>().bounds.size.x;
        GetComponent<Draggable>().Width = width;

        square.transform.localScale = new Vector3(width, 1, 1);

        GetComponent<BoxCollider2D>().size = new Vector3(width, 1, 1);
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

}