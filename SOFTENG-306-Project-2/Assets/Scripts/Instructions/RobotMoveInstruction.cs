using System.Collections.Generic;
using UnityEngine;

namespace Instructions
{
    public class RobotMoveInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;

        public RobotController robot;

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
            var didMove = false;

            switch (moveDirection)
            {
                case Directions.Up:
                    didMove = robot.MoveTL();
                    break;
                case Directions.Down:
                    didMove = robot.MoveBR();
                    break;
                case Directions.Left:
                    didMove = robot.MoveBL();
                    break;
                case Directions.Right:
                    didMove = robot.MoveTR();
                    break;
            }

            if (!didMove)
            {
                executor.Stop();
            }
        }

    }
}