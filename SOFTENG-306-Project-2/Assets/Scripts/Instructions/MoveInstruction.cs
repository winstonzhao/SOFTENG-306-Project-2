using System.Collections.Generic;
using UnityEngine;

namespace Instructions
{
    public enum MoveType
    {
        Absolute, Relative
    }

    public class MoveInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;
        private Instructable target;

        float t;

        private Vector3 start;
        private Vector3 end;

        public Vector3 moveTarget;

        public float seconds;

        private bool moving;

        public MoveType moveType;

        private bool trackMouse = false;

        private bool mouseDebounce = false;

        private InstructionRenderer instructionRenderer;

        public void Start()
        {
            Editable = true;
            instructionRenderer = GetComponent<InstructionRenderer>();
        }

        private void onClicked(object obj)
        {
            if (!Editable || trackMouse) return;
            trackMouse = true;
            mouseDebounce = true;
        }

        public override List<InstructionComponent> InstructionComponents
        {
            get
            {
                return new List<InstructionComponent>
            {
                new InstructionComponent("Move"),
                new InstructionComponent(Mathf.Round(moveTarget.x).ToString())
                {
                    OnComponentClicked = onClicked
                },
                new InstructionComponent(Mathf.Round(moveTarget.y).ToString())
                {
                    OnComponentClicked = onClicked
                }
            };
            }
        }

        public override bool Editable { get; set; }

        public void Update()
        {
            if (trackMouse)
            {
                moveTarget.x += Input.GetAxis("Mouse X");
                moveTarget.y += Input.GetAxis("Mouse Y");
                if (instructionRenderer != null)
                {
                    instructionRenderer.BackgroundColor = new Color(0, 0, 1);
                    instructionRenderer.Render();
                }
            }

            if (Input.GetMouseButtonDown(0) && !mouseDebounce && trackMouse)
            {
                trackMouse = false;
                moveTarget.x = Mathf.Round(moveTarget.x);
                moveTarget.y = Mathf.Round(moveTarget.y);

                if (instructionRenderer != null)
                {
                    instructionRenderer.BackgroundColor = new Color(1, 1, 1);
                }
            }
            mouseDebounce = false;
        }


        public override void UpdateInstruction()
        {
            if (moving)
            {
                t += Time.deltaTime / seconds;

                target.transform.position = Vector3.Lerp(start, end, t);

                if (t >= 1)
                {
                    moving = false;
                    instructionExecutor.ExecuteNextInstruction();
                }
            }
        }

        public override void Execute(Instructable target, InstructionExecutor executor)
        {
            instructionExecutor = executor;
            this.target = target;

            t = 0;
            start = target.transform.position;
            end = moveTarget;

            if (moveType == MoveType.Relative)
            {
                end += start;
            }

            moving = true;
        }

    }
}
