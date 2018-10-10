using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Instructions
{
    public class JumpInstruction : Instruction
    {
        private InstructionExecutor instructionExecutor;
        private JumpTargetInstruction jumpTarget;

        private BezierCurve curve;

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
            Destroy(curve.gameObject);
        }

        private void Update()
        {
            curve.UpdatePoints(new Vector3[]
            {
                this.transform.position,
                Vector3.Lerp(this.transform.position, jumpTarget.transform.position, 0.25f) + new Vector3(4, 0, 0),
                Vector3.Lerp(this.transform.position, jumpTarget.transform.position, 0.75f) + new Vector3(4, 0, 0),
                this.jumpTarget.transform.position
            });
        }

        private void Start()
        {
            // Load the End Jump component
            var prefab = Resources.Load<GameObject>("Prefabs/Instructions/JumpTargetUI");
            var gameObj = Instantiate(prefab);

            gameObj.GetComponent<RectTransform>().SetParent(GetComponentInParent<Canvas>().transform, false);

            jumpTarget = gameObj.GetComponent<JumpTargetInstruction>();
            jumpTarget.transform.position = transform.position;

            var targetDraggable = gameObj.GetComponent<DraggableUIItem>();

            var targetRenderer = targetDraggable.GetComponent<InstructionRenderer>();
            targetRenderer.IsEnabled = false;

            var draggableItem = GetComponent<DraggableUIItem>();
            draggableItem.AddConnectedItem(targetDraggable);
            draggableItem.OnDropZoneChanged += d => targetRenderer.IsEnabled = d != null;

            curve = BezierLines.Instance.AddCurve(
                new Vector3[]
                {
                    this.transform.position,
                    Vector3.Lerp(this.transform.position, jumpTarget.transform.position, 0.25f) + new Vector3(4, 0, 0),
                    Vector3.Lerp(this.transform.position, jumpTarget.transform.position, 0.75f) + new Vector3(4, 0, 0),
                    this.jumpTarget.transform.position
                });
        }

        public override void Execute(RobotController target, InstructionExecutor executor)
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