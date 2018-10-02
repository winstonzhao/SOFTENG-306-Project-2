using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Instructions
{
    public class RobotPickupInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;

        private Directions moveDirection = Directions.Up;

        public RobotController robot;

        public override ReadOnlyCollection<InstructionComponent> InstructionComponents
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
                }.AsReadOnly();
            }
        }

        public override bool Editable { get; set; }

        public void Start()
        {
            Editable = true;
        }

        private void onClicked(object obj)
        {
            moveDirection = (Directions) obj;
        }

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

            didMove = robot.PickUpItem((RobotController.Direction) moveDirection);

            if (!didMove) executor.Stop();
        }
    }
}