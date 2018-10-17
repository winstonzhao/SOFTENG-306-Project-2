using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.physics;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Instructions
{
    public class RobotMoveToInstruction : Instruction, IPointerExitHandler, IPointerEnterHandler
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

        private GameObject tile;

        public RobotMoveToInstruction()
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

        private void OnDestroy()
        {
            if (tile != null) Destroy(tile);
        }

        private void onClicked(object obj)
        {
            if (!Editable) return;
            trackMouse = !trackMouse;
            mouseDebounce = true;
            if (trackMouse)
            {
                instructionRenderer.BackgroundColor = InstructionRenderer.SelectedBackgroundColor;
            }
            else
            {
                instructionRenderer.BackgroundColor = instructionRenderer.DefaultBackgroundColor;
            }
        }

        public void Update()
        {

            if (Input.GetMouseButtonDown(0) && !mouseDebounce && trackMouse)
            {
                trackMouse = false;

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
                        targetPos = new Vector3(selectedObj.Position.x, 1, selectedObj.Position.z);
                        instructionRenderer.Render();
                    }
                }

                if (instructionRenderer != null)
                {
                    instructionRenderer.BackgroundColor = instructionRenderer.DefaultBackgroundColor;
                }
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

            if (selectedObj == null) throw new InstructionException("No Move target selected");

            var didMove = robot.MoveTo(targetPos, null);
            if (!didMove) throw new InstructionException("Could not move to selected target");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (tile == null) return;

            Destroy(tile);
            tile = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (selectedObj == null) return;

            var prefab = Resources.Load<GameObject>("Prefabs/Instructions/RobotGhost");

            tile = Instantiate(prefab);
            tile.GetComponent<IsoTransform>().Position = new Vector3(selectedObj.Position.x, 1, selectedObj.Position.z);
            tile.transform.localScale = new Vector3(.8f, .8f, 1);

        }
    }
}