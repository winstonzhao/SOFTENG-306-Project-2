using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Instructions
{
    public class RobotIncrementInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;

        private Directions moveDirection = Directions.Up;

        private RobotController robot;

        public SoftwareLevelGenerator SoftwareLevelGenerator;

        public override ReadOnlyCollection<InstructionComponent> InstructionComponents
        {
            get
            {
                return new List<InstructionComponent>
                {
                    new InstructionComponent("Increment"),
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
            if (SoftwareLevelGenerator == null)
            {
                SoftwareLevelGenerator = FindObjectOfType<SoftwareLevelGenerator>();
            }
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

            var currentX = robot.X;
            var currentZ = robot.Z;

            switch (moveDirection)
            {
                case Directions.Up:
                    currentX++;
                    break;
                case Directions.Down:
                    currentX--;
                    break;
                case Directions.Left:
                    currentZ++;
                    break;
                case Directions.Right:
                    currentZ--;
                    break;
            }

            var obj = SoftwareLevelGenerator.GetObject(currentX, currentZ);
            if (obj == null)
            {
                throw new InstructionException("Could not find object " + moveDirection.ToString());
                return;
            }

            var arrayElement = obj.GetComponent<ArrayElement>();
            if (arrayElement == null)
            {
                throw new InstructionException("Could not find object " + moveDirection.ToString());
                return;
            }

            arrayElement.Value++;

        }
    }
}