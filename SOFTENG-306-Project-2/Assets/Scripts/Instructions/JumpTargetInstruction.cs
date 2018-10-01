using System.Collections.Generic;
using UnityEngine;

namespace Instructions
{
    public class JumpTargetInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;

        public override bool Editable { get; set; }

        public override List<InstructionComponent> InstructionComponents
        {
            get
            {
                return new List<InstructionComponent>
            {
                new InstructionComponent("END JUMP")
            };
            }
        }


        public override void Execute(Instructable target, InstructionExecutor executor)
        {
            instructionExecutor = executor;
        }

        public override void UpdateInstruction()
        {
            instructionExecutor.ExecuteNextInstruction();
        }
    }
}