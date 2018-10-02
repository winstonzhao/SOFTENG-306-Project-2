using System.Collections.Generic;
using UnityEngine;

namespace Instructions
{
    public class RobotPickupInstruction : Instruction
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
                new InstructionComponent("Pickup"),
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

            didMove = robot.PickUpItem((RobotController.Direction)moveDirection);

            if (!didMove)
            {
                executor.Stop();
            }
        }

    }
}