using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Instructions
{
    public class JumpIfInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;
        private JumpTargetInstruction trueTarget;

        private RobotController robot;

        private Directions compareDirection;
        private RobotController.Compare comparison;

        private BezierCurve curve;

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
//            Destroy(curve.gameObject);
        }

        private void Update()
        {
//            curve.UpdatePoints(new Vector3[]
//            {
//                this.transform.position,
//                Vector3.Lerp(this.transform.position, trueTarget.transform.position, 0.25f) + new Vector3(4, 0, 0),
//                Vector3.Lerp(this.transform.position, trueTarget.transform.position, 0.75f) + new Vector3(4, 0, 0),
//                this.trueTarget.transform.position
//            });
        }

        private JumpTargetInstruction CreateTarget(string name)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Instructions/JumpTargetUI");
            var gameObj = Instantiate(prefab);

            gameObj.GetComponent<RectTransform>().SetParent(GetComponentInParent<Canvas>().transform, false);

            var target = gameObj.GetComponent<JumpTargetInstruction>();
            target.transform.position = transform.position;

            var targetDraggable = gameObj.GetComponent<DraggableUIItem>();

            var targetRenderer = targetDraggable.GetComponent<InstructionRenderer>();
            targetRenderer.IsEnabled = false;

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

//            curve = FindObjectOfType<BezierLines>().AddCurve(
//                new Vector3[]
//                {
//                    this.transform.position,
//                    Vector3.Lerp(this.transform.position, trueTarget.transform.position, 0.25f) + new Vector3(4, 0, 0),
//                    Vector3.Lerp(this.transform.position, trueTarget.transform.position, 0.75f) + new Vector3(4, 0, 0),
//                    this.trueTarget.transform.position
//                });
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