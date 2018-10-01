using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Instructions
{
    class InstructionText
    {
        public MeshRenderer Renderer { get; set; }
        public TextMesh TextMesh { get; set; }
        public GameObject GameObject { get; set; }

        public BoxCollider2D Collider { get; set; }

    }

    [Obsolete("Deprecated, a canvas system with UIInsructionRenderer")]
    [ExecuteInEditMode]
    public class ObjectInstructionRenderer : InstructionRenderer
    {
        public Instruction instruction;

        public override bool IsEnabled
        {
            get
            {
                return spriteRenderer.enabled;
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

        private List<InstructionText> texts = new List<InstructionText>();
        private BoxCollider2D boxCollider;
        private Draggable draggable;
        private SpriteRenderer spriteRenderer;

        public float Spacing = 0.4f;

        void Start()
        {
            SetFields();
        }

        void OnEnable()
        {
            if (instruction == null)
            {
                instruction = GetComponent<Instruction>();
            }

            SetFields();
        }

        private InstructionText CreateTextObject(InstructionComponent component, string name = "text")
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Instructions/InstructionText");
            var go = Instantiate<GameObject>(prefab);
            go.name = name;

            var meshRender = go.GetComponent<MeshRenderer>();
            var textMesh = go.GetComponent<TextMesh>();
            var collider = go.GetComponent<BoxCollider2D>();

            textColor = textMesh.color;
            textMesh.text = component.Text;
            collider.enabled = component.OnComponentClicked != null;

            collider.size = meshRender.bounds.size;
            collider.offset = new Vector2(meshRender.bounds.size.x / 2, 0);
            var eventHandler = go.GetComponent<ClickEventEmitter>();
            eventHandler.EventHandler += () => component.OnComponentClicked(null);

            return new InstructionText
            {
                GameObject = go,
                Renderer = meshRender,
                TextMesh = textMesh,
                Collider = collider
            };

        }

        private void SetFields()
        {
            var size = instruction.InstructionComponents.Count;

            if (boxCollider == null)
            {
                boxCollider = GetComponent<BoxCollider2D>();
                boxCollider.isTrigger = true;
                var square = transform.GetChild(0);
                spriteRenderer = square.GetComponent<SpriteRenderer>();
                backgroundColor = spriteRenderer.color;
            }

            if (draggable == null) draggable = GetComponent<Draggable>();

            if (texts.Count != size)
            {
                var textParent = transform.GetChild(1);
                texts.Clear();
                while (textParent.childCount != 0)
                {
                    foreach (Transform child in textParent)
                    {
#if UNITY_EDITOR
                        DestroyImmediate(child.gameObject);
#else
                        Destroy(child.gameObject);
#endif
                    }
                }

                var components = instruction.InstructionComponents;

                for (int i = 0; i < size; i++)
                {
                    var textObj = CreateTextObject(components[i], "text " + i);
                    texts.Add(textObj);

                    // Move forward so it will be clicked before its parent
                    textObj.GameObject.transform.position = transform.position + new Vector3(0, 0, -0.1f);
                    textObj.GameObject.transform.parent = textParent.transform;
                }

                UpdateSize();

            }

            Render();
        }

        private float UpdateSize()
        {
            var square = transform.GetChild(0);
            var fullWidth = texts.Sum(t => t.Renderer.bounds.size.x) + (Spacing * texts.Count) - Spacing;

            draggable.Size = new Vector2(fullWidth, texts[0].Renderer.bounds.size.y);
            var start = -fullWidth / 2;

            for (int i = 0; i < texts.Count; i++)
            {
                texts[i].GameObject.transform.position = transform.position + new Vector3(0, 0, -0.1f);
                texts[i].TextMesh.color = TextColor;
                texts[i].GameObject.transform.position += new Vector3(start, 0, 0);
                start += texts[i].Renderer.bounds.size.x + Spacing;
            }
            square.transform.localScale = new Vector3(fullWidth, 1, 1);
            boxCollider.size = new Vector3(fullWidth, 1, 1);

            return fullWidth;
        }

        public override void Render()
        {
            var square = transform.GetChild(0);

            var components = instruction.InstructionComponents;
            var dirtyText = false;

            for (int i = 0; i < components.Count; i++)
            {
                if (texts[i].TextMesh.text != components[i].Text) dirtyText = true;
                texts[i].TextMesh.text = components[i].Text;
            }

            if (dirtyText)
            {
                UpdateSize();
            }

            square.GetComponent<SpriteRenderer>().color = backgroundColor;

        }

    }
}