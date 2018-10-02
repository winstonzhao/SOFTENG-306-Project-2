using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Instructions
{
    [ExecuteInEditMode]
    public class UIInstructionRenderer : InstructionRenderer
    {
        private BoxCollider2D boxCollider;
        private readonly List<ChildComp> children = new List<ChildComp>();

        private Image image;

        private List<Image> images = new List<Image>();

        public Instruction instruction;

        public Vector2 padding = new Vector2(20, 10);
        private RectTransform rectTransform;

        public float spacing = 2;

        public override bool IsEnabled
        {
            get { return image.enabled; }

            set
            {
                image.enabled = value;
                boxCollider.enabled = value;
                children.ForEach(c => c.RectTransform.gameObject.SetActive(value));
            }
        }

        private void OnEnable()
        {
            if (instruction == null) instruction = GetComponent<Instruction>();

            boxCollider = GetComponent<BoxCollider2D>();
            image = GetComponent<Image>();
            rectTransform = GetComponentInChildren<RectTransform>();

            Render();
        }

        private void Start()
        {
            var canvas = FindObjectOfType<Canvas>();
            GetComponent<RectTransform>().SetParent(canvas.transform, false);
            Render();
        }

        private Text CreateTextObject(InstructionComponent component, string name = "text")
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Instructions/InstructionTextUI");
            var go = Instantiate(prefab);
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
            var prefab = Resources.Load<GameObject>("Prefabs/Instructions/InstructionDropdown");
            var go = Instantiate(prefab);
            go.name = name;
            go.GetComponent<RectTransform>().SetParent(transform, false);

            var dropdown = go.GetComponent<Dropdown>();
            dropdown.onValueChanged.AddListener(value => component.OnComponentClicked(value));
            dropdown.options = component.values.Select(v => new Dropdown.OptionData(v)).ToList();

            return go;
        }

        private void UpdateSize()
        {
            var components = instruction.InstructionComponents;

            for (var i = 0; i < components.Count; i++)
                if (children[i].InstructionComponent.GetType() != typeof(DropdownInstructionComponent))
                {
                    var text = children[i].RectTransform.GetComponent<Text>();
                    text.text = components[i].Text;
                    children[i].Size = new Vector2(text.preferredWidth, text.preferredHeight);
                }

            float height = 0;
            var width = children.Sum(c => c.Size.x);
            width += spacing * components.Count - spacing;

            float start = 0;
            foreach (var child in children)
            {
                child.RectTransform.anchoredPosition = new Vector2(start + child.Size.x / 2 + padding.x, 0);
                child.RectTransform.localPosition += new Vector3(0, 0, -0.1f);

                start += spacing + child.Size.x;
                height = Mathf.Max(child.Size.y, height);
            }

            width = width + padding.x * 2;
            height = height + padding.y * 2;
            boxCollider.size = new Vector2(width, height);
            rectTransform.sizeDelta = new Vector2(width, height);
            GetComponent<Draggable>().Size = new Vector2(width, height) *
                                             FindObjectOfType<Canvas>().GetComponent<RectTransform>().localScale.x;
        }

        private void DestroyChildren()
        {
            while (transform.childCount != 0)
                foreach (Transform child in transform)
                {
#if UNITY_EDITOR
                    DestroyImmediate(child.gameObject);
#else
                    Destroy(child.gameObject);
                #endif
                }

            children.Clear();
        }

        public override void Render()
        {
            var components = instruction.InstructionComponents;
            if (children.Count != components.Count)
            {
                DestroyChildren();
                for (var i = 0; i < components.Count; i++)
                {
                    var child = new ChildComp();
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
                    }

                    child.InstructionComponent = components[i];
                    children.Add(child);
                }

                UpdateSize();
                images = GetComponentsInChildren<Image>().ToList();
            }

            var dirtyText = false;

            for (var i = 0; i < children.Count; i++)
            {
                if (children[i].InstructionComponent.Text != components[i].Text) dirtyText = true;
                children[i].InstructionComponent.Text = components[i].Text;
            }

            if (dirtyText) UpdateSize();

            image.color = BackgroundColor;
            images.ForEach(i => i.color = BackgroundColor);
        }

        private class ChildComp
        {
            public Vector2 Size { get; set; }
            public RectTransform RectTransform { get; set; }
            public Behaviour Behaviour { get; set; }
            public InstructionComponent InstructionComponent { get; set; }
        }
    }
}