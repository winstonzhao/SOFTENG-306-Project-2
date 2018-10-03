using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.physics;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;

namespace Instructions
{
    public class RobotMoveLocationInstruction : Instruction
    {
        private InstructionComponent component2;
        private InstructionExecutor instructionExecutor;
        private InstructionRenderer instructionRenderer;
        private bool mouseDebounce;

        private RobotController robot;

        private IsoTransform selectedObj;

        private bool trackMouse;

        private bool executeNext = false;
        private Vector3 targetPos;

        public RobotMoveLocationInstruction()
        {
            component2 = new InstructionComponent("POS")
            {
                OnComponentClicked = onClicked
            };
        }

        public override ReadOnlyCollection<InstructionComponent> InstructionComponents
        {
            get
            {
                return new List<InstructionComponent>
                {
                    new InstructionComponent("MoveTo"),
                    component2
                }.AsReadOnly();
            }
        }

        public override bool Editable { get; set; }

        public void Start()
        {
            Editable = true;
            instructionRenderer = GetComponent<InstructionRenderer>();
        }

        private void onClicked(object obj)
        {
            if (!Editable || trackMouse) return;
            trackMouse = true;
            mouseDebounce = true;
        }

        public void Update()
        {
            if (trackMouse)
                if (instructionRenderer != null)
                {
                    instructionRenderer.BackgroundColor = new Color(0, 0, 1);
                    instructionRenderer.Render();
                }

            if (Input.GetMouseButtonDown(0) && !mouseDebounce && trackMouse)
            {
                trackMouse = false;
                Debug.Log("Check raycast");

                var isoRay = Isometric.MouseToIsoRay();

                //do an isometric raycast on left mouse click
                if (Input.GetMouseButtonDown(0)) {
                    IsoRaycastHit isoRaycastHit;
                    if (IsoPhysics.Raycast(isoRay, out isoRaycastHit)) {
                        selectedObj = isoRaycastHit.IsoTransform;
                        component2 =
                            new InstructionComponent("X: " + selectedObj.Position.x + " Z: " + selectedObj.Position.z)
                            {
                                OnComponentClicked = onClicked
                            };
                        instructionRenderer.Render();
                    }
                }

//                var isoRay = Isometric.MouseToIsoRay();
//
//                var hit = false;
//                foreach (var raycastHit in IsoPhysics.RaycastAll(isoRay))
//                {
//                    Debug.Log("we clicked on " + raycastHit.Collider.name + " at " + raycastHit.Point);
//                    if (raycastHit.IsoTransform.name.ToLower().StartsWith("floor tile"))
//                    {
//                        hit = true;
//                        selectedObj = raycastHit.IsoTransform;
//                        component2 =
//                            new InstructionComponent("X: " + selectedObj.Position.x + " Z: " + selectedObj.Position.z)
//                            {
//                                OnComponentClicked = onClicked
//                            };
//                        instructionRenderer.Render();
//                        break;
//                    }
//                }

                // if (!hit) selectedObj = null;

                if (instructionRenderer != null) instructionRenderer.BackgroundColor = new Color(1, 1, 1);
            }

            mouseDebounce = false;
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

            targetPos = new Vector3(selectedObj.Position.x, 1, selectedObj.Position.z);
            var didMove = robot.MoveTo(targetPos);
            if (!didMove) throw new InstructionException();
        }
    }
}