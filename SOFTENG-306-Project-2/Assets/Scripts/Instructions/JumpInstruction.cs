using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Instructions
{
    public class JumpInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;
        private JumpTargetInstruction jumpTarget;

        public override bool Editable { get; set; }

        public override ReadOnlyCollection<InstructionComponent> InstructionComponents
        {
            get
            {
                return new List<InstructionComponent>
                {
                    new InstructionComponent("Jump")
                }.AsReadOnly();
            }
        }

        private void Start()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Instructions/JumpTargetUI");
            var gameObj = Instantiate(prefab);

            gameObj.GetComponent<RectTransform>().SetParent(FindObjectOfType<Canvas>().transform, false);

            jumpTarget = gameObj.GetComponent<JumpTargetInstruction>();
            jumpTarget.transform.position = transform.position;

            var targetDraggable = gameObj.GetComponent<DraggableItem>();

            var targetRenderer = targetDraggable.GetComponent<InstructionRenderer>();
            targetRenderer.IsEnabled = false;

            var draggableItem = GetComponent<DraggableItem>();
            draggableItem.AddConnectedItem(targetDraggable);
            draggableItem.OnDropZoneChanged += d => targetRenderer.IsEnabled = d != null;
        }

        public override void Execute(Instructable target, InstructionExecutor executor)
        {
            instructionExecutor = executor;
        }

        public override void UpdateInstruction()
        {
            instructionExecutor.JumpToInstruction(jumpTarget);
            instructionExecutor.ExecuteNextInstruction();
        }
    }
}