using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Instructions
{
    public class JumpInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;
        private JumpTargetInstruction jumpTarget;
        private InstructionManager instructionManager;

        public override string InstructionName
        {
            get { return "Jump"; }
        }

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

        private void OnDestroy()
        {
            if (instructionManager != null)
            {
                instructionManager.RemoveColor(GetInstanceID() + "Jump");
            }
        }

        private void Start()
        {
            // Load the End Jump component
            var prefab = Resources.Load<GameObject>("Prefabs/Instructions/JumpTargetUI");
            var gameObj = Instantiate(prefab);

            gameObj.GetComponent<RectTransform>().SetParent(GetComponentInParent<Canvas>().transform, false);

            jumpTarget = gameObj.GetComponent<JumpTargetInstruction>();
            jumpTarget.transform.position = transform.position;


            instructionManager = FindObjectOfType<InstructionManager>();
            if (instructionManager == null)
            {
                var go = new GameObject();
                instructionManager = go.AddComponent<InstructionManager>();
            }

            var targetDraggable = gameObj.GetComponent<DraggableUIItem>();

            var instructionRenderer = GetComponent<InstructionRenderer>();
            var targetRenderer = targetDraggable.GetComponent<InstructionRenderer>();
            targetRenderer.IsEnabled = false;

            // Set colors to match
            var color = instructionManager.GenerateColor(GetInstanceID() + "Jump");
            targetRenderer.DefaultBackgroundColor = color;
            instructionRenderer.DefaultBackgroundColor = color;

            var draggableItem = GetComponent<DraggableUIItem>();
            draggableItem.AddConnectedItem(targetDraggable);
            targetDraggable.AddConnectedItem(draggableItem);

            draggableItem.OnDropZoneChanged += d => targetRenderer.IsEnabled = d != null;

        }

        public override void Execute(RobotController target, InstructionExecutor executor)
        {
            instructionExecutor = executor;
        }

        public override void UpdateInstruction()
        {
            // Jump to new position
            instructionExecutor.JumpToInstruction(jumpTarget);
            instructionExecutor.ExecuteNextInstruction();
        }
    }
}