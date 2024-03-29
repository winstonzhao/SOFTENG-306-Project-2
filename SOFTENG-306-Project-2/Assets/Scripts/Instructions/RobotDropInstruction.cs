using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Instructions
{
    public class RobotDropInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;

        private Directions moveDirection = Directions.Up;
        private RobotController robot;

        public override string InstructionName
        {
            get { return "Drop"; }
        }

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

        public override void Execute(RobotController target, InstructionExecutor executor)
        {
            instructionExecutor = executor;
            robot = target;
            var didMove = false;

            // Try to drop
            didMove = robot.DropItem(moveDirection);

            if (!didMove) throw new InstructionException("Could not drop in given direction");
        }
    }
}