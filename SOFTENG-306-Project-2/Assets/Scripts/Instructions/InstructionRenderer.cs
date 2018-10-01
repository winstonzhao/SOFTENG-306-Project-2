using UnityEngine;

namespace Instructions
{
    public abstract class InstructionRenderer : MonoBehaviour
    {
        public abstract bool IsEnabled { get; set; }
        protected Color textColor = new Color(79f / 255, 20f / 255, 20f / 255);
        public Color TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;
                Render();
            }
        }

        protected Color backgroundColor = new Color(1, 1, 1);
        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                Render();
            }
        }

        public abstract void Render();

    }
}