using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIInstructionRenderer : InstructionRenderer 
{

    class ChildComp
    {
        public Vector2 Size { get; set; }
        public RectTransform RectTransform { get; set; }
        public Behaviour Behaviour {get; set; }
    }

    public float spacing = 2;

    private Image image;
    private List<Text> texts = new List<Text>();
    private List<ChildComp> children = new List<ChildComp>();
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
            children.ForEach(c => c.RectTransform.gameObject.SetActive(value));
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
        var canvas = FindObjectOfType<Canvas>();
        GetComponent<RectTransform>().SetParent(canvas.transform, false);
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
        eventHandler.EventHandler += () => component.OnComponentClicked(null);

        return text;
    }

    private GameObject CreateDropdownObject(DropdownInstructionComponent component)
    {
        var prefab = Resources.Load<GameObject>("Prefabs/InstructionDropdown");
        var go = Instantiate<GameObject>(prefab);
        go.name = name;
        go.GetComponent<RectTransform>().SetParent(transform, false);
        
        var dropdown = go.GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener((value) => component.OnComponentClicked(value));
        dropdown.options = component.values.Select(v => new Dropdown.OptionData(v)).ToList();

        return go;
    }

    private void UpdateSize()
    {
        float height = 0;            
        float width = children.Sum(c => c.Size.x);
        width += spacing * instruction.InstructionComponents.Count - spacing;

        float start = 0;
        foreach (var child in children)
        {
            child.RectTransform.anchoredPosition = new Vector2(start + child.Size.x / 2, 0);
            child.RectTransform.localPosition += new Vector3(0, 0, -0.1f);
            start += spacing + child.Size.x;
            height = Mathf.Max(child.Size.y, height);
        }

        boxCollider.size = new Vector2(width, height);
        rectTransform.sizeDelta = new Vector2(width, height);
        GetComponent<Draggable>().Size = new Vector2(width, height) * FindObjectOfType<Canvas>().GetComponent<RectTransform>().localScale.x;

    }
    
    public override void Render()
    {
        var components = instruction.InstructionComponents;
        if (children.Count != components.Count)
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
            children.Clear();

            for (int i = 0; i < components.Count; i++)
            {
                ChildComp child = new ChildComp();
                if (components[i].GetType() == typeof(DropdownInstructionComponent))
                {
                    var go = CreateDropdownObject(components[i] as DropdownInstructionComponent);
                    child.RectTransform = go.GetComponent<RectTransform>();
                    child.Size = child.RectTransform.sizeDelta;
                }
                else
                {
                    var text = CreateTextObject(components[i], "text " + i);
                    child.Size = new Vector2(text.preferredWidth, text.preferredHeight);
                    child.RectTransform = text.GetComponent<RectTransform>();
                    texts.Add(text);
                }
                children.Add(child);
                UpdateSize();
            }
        }

        // bool dirtyText = false;

        // for (int i = 0; i < texts.Count; i++)
        // {
        //     if (texts[i].text != components[i].Text) dirtyText = true;
        //     texts[i].text = components[i].Text;
        // }

        // if (dirtyText)
        // {
        //     UpdateSize();
        // }

        image.color = BackgroundColor;

    }

}
