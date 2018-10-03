using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Instructions
{
    public class JumpTargetInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;

        public override bool Editable { get; set; }

        public override ReadOnlyCollection<InstructionComponent> InstructionComponents
        {
            get
            {
                return new List<InstructionComponent>
                {
                    new InstructionComponent("END JUMP")
                }.AsReadOnly();
            }
        }


        public override void Execute(RobotController target, InstructionExecutor executor)
        {
            instructionExecutor = executor;
        }

        public override void UpdateInstruction()
        {
            instructionExecutor.ExecuteNextInstruction();
        }
    }
}