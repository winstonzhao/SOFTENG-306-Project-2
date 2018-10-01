using System.Collections.Generic;
using UnityEngine;

namespace Instructions
{
    public class MoveRobotInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;
        private Instructable target;

        public RobotController robot;

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
            instructionExecutor.ExecuteNextInstruction();
        }

        public override void Execute(Instructable target, InstructionExecutor executor)
        {
            instructionExecutor = executor;
            this.target = target;

            switch (moveDirection)
            {
                case Directions.Up:
                    robot.MoveTL();
                    break;
                case Directions.Down:
                    robot.MoveBR();
                    break;
                case Directions.Left:
                    robot.MoveBL();
                    break;
                case Directions.Right:
                    robot.MoveBR();
                    break;
            }
        }

    }
}