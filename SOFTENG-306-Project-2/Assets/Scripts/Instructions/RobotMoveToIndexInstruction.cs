using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.physics;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;

namespace Instructions
{
    public class RobotMoveToIndexInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;
        private InstructionRenderer instructionRenderer;

        public SoftwareLevelGenerator softwareLevelGenerator;

        private RobotController robot;

        private bool executeNext = false;
        private Vector3 targetPos;
        private Directions moveDirection;

        public override ReadOnlyCollection<InstructionComponent> InstructionComponents
        {
            get
            {
                return new List<InstructionComponent>
                {
                    new InstructionComponent("MoveToIndex"),
                    new DropdownInstructionComponent("Direction")
                    {
                        OnComponentClicked = onClicked
                    }
                }.AsReadOnly();
            }
        }

        public override bool Editable { get; set; }
        
        private void onClicked(object obj)
        {
            moveDirection = (Directions) obj;
        }

        public void Start()
        {
            Editable = true;
            instructionRenderer = GetComponent<InstructionRenderer>();
            if (softwareLevelGenerator == null)
            {
                softwareLevelGenerator = FindObjectOfType<SoftwareLevelGenerator>();
            }
        }

        public void Update()
        {
        }

        public override void UpdateInstruction()
        {
            if (executeNext)
            {
                instructionExecutor.ExecuteNextInstruction();
            }
            else
            {
                if (robot.GetComponent<IsoTransform>().Position == targetPos)
                {
                    executeNext = true;
                }
            }
        }

        public override void Execute(RobotController target, InstructionExecutor executor)
        {
            instructionExecutor = executor;
            robot = target;
            executeNext = false;

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

            var obj = softwareLevelGenerator.GetObject(currentX, currentZ);
            var arrayElement = obj.GetComponent<ArrayElement>();
            if (arrayElement == null)
            {
                return;
            }

            targetPos = softwareLevelGenerator.IndexLocation("a" + arrayElement.value);
            var didMove = robot.MoveTo(Vector3.zero, "a" + arrayElement.value);
            if (!didMove) throw new InstructionException();
        }
    }
}