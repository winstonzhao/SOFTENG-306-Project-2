using System.Collections.Generic;
using UnityEngine;

class TextObject
{
    public MeshRenderer Renderer { get; set; }
    public TextMesh TextMesh { get; set; }
    public GameObject GameObject { get; set; }


}

[ExecuteInEditMode]
public class InstructionRenderer : MonoBehaviour
{
    public Instruction instruction;

    public bool IsEnabled
    {
        get
        {
            return texts[0].Renderer.enabled;
        }
        set
        {
            foreach (var text in texts)
            {
                text.Renderer.enabled = value;
            }

            spriteRenderer.enabled = value;
            boxCollider.enabled = value;
        }
    }

    private List<TextObject> texts = new List<TextObject>();
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

    private void SetFields(int size)
    {
        if (boxCollider == null) 
        {
            boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
            var square = transform.GetChild(0);
            spriteRenderer = square.GetComponent<SpriteRenderer>();
        }

        if (texts.Count != size)
        {
            var textParent = transform.GetChild(1);
            texts.Clear();
            while (textParent.childCount != 0) {
                foreach (Transform child in textParent)
                {
                    #if UNITY_EDITOR
                    DestroyImmediate(child.gameObject);
                    #else
                    Destroy(child.gameObject);
                    #endif
                }
            }

            for (int i = 0; i < size; i++)
            {
                var go = new GameObject("text " + i);
                var meshRender = go.AddComponent<MeshRenderer>();
                var textMesh = go.AddComponent<TextMesh>();
                textMesh.characterSize = 0.1f;
                textMesh.fontSize = 96;
                textMesh.color = new Color(79f/255, 20f/255, 20f/255);
                textMesh.alignment = TextAlignment.Center;
                textMesh.anchor = TextAnchor.MiddleLeft;

                texts.Add(new TextObject
                {
                    GameObject = go,
                    Renderer = meshRender,
                    TextMesh = textMesh
                });
                go.transform.position = transform.position;
                // go.transform.position += new Vector3(meshRender.bounds.size.x, 0, 0);
                go.transform.parent = textParent.transform;
            }
        }

        if (draggable == null) draggable = GetComponent<Draggable>();
    }

    public void UpdateSize()
    {
        SetFields(instruction.InstructionComponents.Count);

        var square = transform.GetChild(0);
        var components = instruction.InstructionComponents;
        var fullWidth = 0f;

        for (int i = 0; i < components.Count; i++)
        {
            texts[i].TextMesh.text = components[i].Text;
            fullWidth += texts[i].Renderer.bounds.size.x;
        }

        draggable.Width = fullWidth;
        var start = -fullWidth / 4;

        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].GameObject.transform.position += new Vector3(start, 0, 0);
            start += texts[i].Renderer.bounds.size.x / 2;
        }

        square.transform.localScale = new Vector3(fullWidth, 1, 1);

        boxCollider.size = new Vector3(fullWidth, 1, 1);
    }

}