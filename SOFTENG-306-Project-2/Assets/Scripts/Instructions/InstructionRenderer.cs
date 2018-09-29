using UnityEngine;

[ExecuteInEditMode]
public class InstructionRenderer : MonoBehaviour
{
    public Instruction instruction;

    public bool IsEnabled
    {
        get
        {
            return meshRenderer.enabled;
        }
        set
        {
            meshRenderer.enabled = value;
            spriteRenderer.enabled = value;
            boxCollider.enabled = value;
        }
    }

    private TextMesh textMesh;
    private MeshRenderer meshRenderer;
    private BoxCollider2D boxCollider;
    private Draggable draggable;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        UpdateSize();
    }

    void OnEnable()
    {
        if (instruction == null)
        {
            instruction = GetComponent<Instruction>();
        }
        
        UpdateSize();
    }

    private void SetFields()
    {
        if (boxCollider == null) 
        {
            boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
        }

        if (textMesh == null)
        {
            var square = transform.GetChild(0);
            var text = transform.GetChild(1);
            textMesh = text.GetComponent<TextMesh>();
            meshRenderer = text.GetComponent<MeshRenderer>();
            spriteRenderer = square.GetComponent<SpriteRenderer>();
        }

        if (draggable == null) draggable = GetComponent<Draggable>();
    }

    public void UpdateSize()
    {
        SetFields();
        var square = transform.GetChild(0);

        textMesh.text = instruction.Name;

        var width = meshRenderer.bounds.size.x;
        draggable.Width = width;

        square.transform.localScale = new Vector3(width, 1, 1);

        boxCollider.size = new Vector3(width, 1, 1);
    }

}