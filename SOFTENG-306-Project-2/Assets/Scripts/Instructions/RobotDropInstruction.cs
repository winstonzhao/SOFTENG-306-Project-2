using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Instructions
{
    public class RobotDropInstruction : Instruction
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
                    new InstructionComponent("Drop"),
                    new DropdownInstructionComponent("direction")
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

            didMove = robot.DropItem((RobotController.Direction) moveDirection);

            if (!didMove) executor.Stop();
        }
    }
}