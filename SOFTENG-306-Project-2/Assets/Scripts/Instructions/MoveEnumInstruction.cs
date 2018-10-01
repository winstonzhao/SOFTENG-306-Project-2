using System.Collections.Generic;
using UnityEngine;

namespace Instructions
{
    public enum Directions
    {
        Up, Down, Left, Right
    }


    public class MoveEnumInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;
        private Instructable target;

        float t;

        private Vector3 start;
        private Vector3 end;

        public float seconds = 1;

        private bool moving;

        private Directions moveDirection = Directions.Up;

        public void Start()
        {
            Editable = true;
        }

        private void onClicked(object obj)
        {
            moveDirection = (Directions)obj;
        }

        public override List<InstructionComponent> InstructionComponents
        {
            get
            {
                return new List<InstructionComponent>
            {
                new InstructionComponent("Move"),
                new DropdownInstructionComponent("up")
                {
                    OnComponentClicked = onClicked
                }
            };
            }
        }

        public override bool Editable { get; set; }

        public void Update()
        {

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
            end = start;
            switch (moveDirection)
            {
                case Directions.Up:
                    end += new Vector3(0, 1, 0);
                    break;
                case Directions.Down:
                    end += new Vector3(0, -1, 0);
                    break;
                case Directions.Left:
                    end += new Vector3(-1, 0, 0);
                    break;
                case Directions.Right:
                    end += new Vector3(1, 0, 0);
                    break;
            }

            moving = true;
        }

    }
}