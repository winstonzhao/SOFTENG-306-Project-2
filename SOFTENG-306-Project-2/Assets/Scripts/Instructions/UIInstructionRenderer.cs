using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIInstructionRenderer : InstructionRenderer {

    public float spacing = 2;

    private Image image;
    private List<Text> texts = new List<Text>();
    private BoxCollider2D boxCollider;
    private RectTransform rectTransform;

    public Instruction instruction;

    public override bool IsEnabled
    {
        get
        {
            return image.enabled;
        }

        set
        {
            image.enabled = value;
            boxCollider.enabled = value;
            texts.ForEach(t => t.enabled = value);
        }
    }

    void OnEnable()
    {
        if (instruction == null)
        {
            instruction = GetComponent<Instruction>();
        }

        boxCollider = GetComponent<BoxCollider2D>();
        image = GetComponent<Image>();
        rectTransform = GetComponentInChildren<RectTransform>();

        Render();
    }

    void Start()
    {
        Render();
    }

    private Text CreateTextObject(InstructionComponent component, string name = "text")
    {
        var prefab = Resources.Load<GameObject>("Prefabs/InstructionTextUI");
        var go = Instantiate<GameObject>(prefab);
        go.name = name;
        go.GetComponent<RectTransform>().SetParent(transform, false);

        var text = go.GetComponent<Text>();
        text.text = component.Text;
        text.color = TextColor;

        var collider = go.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(text.preferredWidth, text.preferredHeight);
        collider.enabled = component.OnComponentClicked != null;
        var eventHandler = go.GetComponent<ClickEventEmitter>();
        eventHandler.EventHandler += () => component.OnComponentClicked();

        return text;
    }

    private void UpdateSize()
    {
        float height = 0;            
        float width = texts.Sum(t => t.preferredWidth);
        width += spacing * instruction.InstructionComponents.Count - spacing;

        float start = 0;
        foreach (var text in texts)
        {
            text.GetComponent<RectTransform>().anchoredPosition = new Vector2(start + text.preferredWidth / 2, 0);
            text.GetComponent<RectTransform>().localPosition += new Vector3(0, 0, -0.1f);
            start += spacing + text.preferredWidth;
            height = Mathf.Max(text.preferredHeight, height);
        }

        boxCollider.size = new Vector2(width, height);
        rectTransform.sizeDelta = new Vector2(width, height);
        GetComponent<Draggable>().Size = new Vector2(width, height);

    }
    
    public override void Render()
    {
        var components = instruction.InstructionComponents;
        if (texts.Count != components.Count)
        {
            while (transform.childCount != 0) {
                foreach (Transform child in transform)
                {
                    #if UNITY_EDITOR
                        DestroyImmediate(child.gameObject);
                    #else
                        Destroy(child.gameObject);
                    #endif
                }
            }
            texts.Clear();

            for (int i = 0; i < components.Count; i++)
            {
                var text = CreateTextObject(components[i], "text " + i);
                texts.Add(text);
                UpdateSize();
            }
        }

        bool dirtyText = false;

        for (int i = 0; i < texts.Count; i++)
        {
            if (texts[i].text != components[i].Text) dirtyText = true;
            texts[i].text = components[i].Text;
        }

        if (dirtyText)
        {
            UpdateSize();
        }

        image.color = BackgroundColor;

    }

}
