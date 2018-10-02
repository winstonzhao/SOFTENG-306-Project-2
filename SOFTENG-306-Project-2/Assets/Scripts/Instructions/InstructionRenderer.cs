using UnityEngine;

namespace Instructions
{
    public abstract class InstructionRenderer : MonoBehaviour
    {
        protected Color backgroundColor = new Color(1, 1, 1);
        protected Color textColor = new Color(79f / 255, 20f / 255, 20f / 255);
        public abstract bool IsEnabled { get; set; }

        public Color TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
                Render();
            }
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                Render();
            }
        }

        public abstract void Render();
    }
}