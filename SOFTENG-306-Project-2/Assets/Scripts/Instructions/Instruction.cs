using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Instructions
{
    public class InstructionComponent
    {
        public delegate void ComponnentClicked(object args);

        public InstructionComponent(string text)
        {
            Text = text;
        }

        public ComponnentClicked OnComponentClicked { get; set; }
        public string Text { get; set; }
    }

    public class DropdownInstructionComponent : InstructionComponent
    {
        public List<string> values = new List<string> {"up", "down", "left", "right"};

        public DropdownInstructionComponent(string text) : base(text)
        {
        }
    }

    public abstract class Instruction : MonoBehaviour, IPointerClickHandler
    {
        private static string HELP_PREFIX = "software_minigame/Prefabs/help_";

        public abstract string InstructionName { get; }

        // List of components to be rendered as part of this instruction
        public abstract ReadOnlyCollection<InstructionComponent> InstructionComponents { get; }

        // Toggle if this instruction should allow changes to it
        public abstract bool Editable { get; set; }

        // Called once each time the instruction starts to be executed
        public abstract void Execute(RobotController target, InstructionExecutor executor);

        // Called every update while the instruction is the current executing instruction
        // (Move to the next instruction with executor.ExecuteNextInstruction() when done)
        public abstract void UpdateInstruction();

        public virtual float MinTiming
        {
            get { return 0.4f; }
        }

        /// <summary>
        /// Handle right click to open help
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;

            // Don't load if it already exists
            var helpObj = GameObject.Find("help_" + InstructionName);

            if (helpObj == null)
            {
                // Load the correct help prefab for the instruction
                GameObject prefab = Resources.Load<GameObject>(HELP_PREFIX + InstructionName);
                if (prefab == null) return;

                helpObj = Instantiate<GameObject>(prefab);
                helpObj.name = "help_" + InstructionName;
                helpObj.transform.SetParent(GameObject.Find("InstructionCanvas").transform, false);
            }

            helpObj.SetActive(true);

        }
    }
}