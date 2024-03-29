using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Instructions
{
    public class JumpTargetInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;
        private string label = "END JUMP";

        public override string InstructionName
        {
            get { return "JumpTarget"; }
        }

        public override bool Editable { get; set; }

        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        public override float MinTiming
        {
            get { return 0.0f; }
        }

        public override ReadOnlyCollection<InstructionComponent> InstructionComponents
        {
            get
            {
                return new List<InstructionComponent>
                {
                    new InstructionComponent(Label)
                }.AsReadOnly();
            }
        }


        public override void Execute(RobotController target, InstructionExecutor executor)
        {
            instructionExecutor = executor;
        }

        public override void UpdateInstruction()
        {
            // Don't do anything, only a placeholder to jump to
            instructionExecutor.ExecuteNextInstruction();
        }
    }
}