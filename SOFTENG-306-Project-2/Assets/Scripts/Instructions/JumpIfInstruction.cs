using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Instructions
{
    public class JumpIfInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;
        private JumpTargetInstruction trueTarget;
        private InstructionManager instructionManager;

        private RobotController robot;

        private Directions compareDirection;
        private RobotController.Compare comparison;

        public override string InstructionName
        {
            get { return "JumpIf"; }
        }

        public override bool Editable { get; set; }

        public override ReadOnlyCollection<InstructionComponent> InstructionComponents
        {
            get
            {
                return new List<InstructionComponent>
                {
                    new InstructionComponent("JumpIf"),
                    new DropdownInstructionComponent("up")
                    {
                        OnComponentClicked = OnDirClicked
                    },
                    new DropdownInstructionComponent("up")
                    {
                        OnComponentClicked = OnCompareClicked,
                        values = new List<string>() {"Less", "Equal", "Greater"}
                    }
                }.AsReadOnly();
            }
        }

        private void OnDirClicked(object obj)
        {
            compareDirection = (Directions) obj;
        }

        private void OnCompareClicked(object obj)
        {
            comparison = (RobotController.Compare) obj;
        }

        private void OnDestroy()
        {
            if (instructionManager != null)
            {
                instructionManager.RemoveColor(GetInstanceID() + "JumpIf");
            }
        }

        private JumpTargetInstruction CreateTarget(string name)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Instructions/JumpTargetUI");
            var gameObj = Instantiate(prefab);

            gameObj.GetComponent<RectTransform>().SetParent(GetComponentInParent<Canvas>().transform, false);

            instructionManager = FindObjectOfType<InstructionManager>();
            if (instructionManager == null)
            {
                var go = new GameObject();
                instructionManager = go.AddComponent<InstructionManager>();
            }


            var target = gameObj.GetComponent<JumpTargetInstruction>();
            target.transform.position = transform.position;

            var targetDraggable = gameObj.GetComponent<DraggableUIItem>();

            var instructionRenderer = GetComponent<InstructionRenderer>();
            var targetRenderer = targetDraggable.GetComponent<InstructionRenderer>();
            targetRenderer.IsEnabled = false;

            // Set colors to match
            var color = instructionManager.GenerateColor(GetInstanceID() + "JumpIf");
            targetRenderer.DefaultBackgroundColor = color;
            instructionRenderer.DefaultBackgroundColor = color;

            var draggableItem = GetComponent<DraggableUIItem>();
            draggableItem.AddConnectedItem(targetDraggable);
            draggableItem.OnDropZoneChanged += d => targetRenderer.IsEnabled = d != null;

            target.Label = name;

            return target;
        }

        private void Start()
        {
            // Load the End Jump component
            trueTarget = CreateTarget("JumpIf END");
        }

        public override void Execute(RobotController target, InstructionExecutor executor)
        {
            instructionExecutor = executor;
            robot = target;
        }

        public override void UpdateInstruction()
        {
            if (robot.CompareItem(compareDirection, comparison, true, -1, -1))
            {
                instructionExecutor.JumpToInstruction(trueTarget);
            }

            instructionExecutor.ExecuteNextInstruction();
        }
    }
}