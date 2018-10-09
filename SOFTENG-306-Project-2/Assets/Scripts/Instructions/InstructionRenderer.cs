using UnityEngine;

namespace Instructions
{
    public abstract class InstructionRenderer : MonoBehaviour
    {
        public static Color DefaultBackgoundColor = new Color(1, 1, 1);
        public static Color FailBackgroundColor = new Color(1, .5f, .5f);
        public static Color ExecutingBackgroundColor = new Color(0, 1, 0);

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

        /// <summary>
        /// Resets this instructions UI style to default
        /// </summary>
        public abstract void ResetStyle();
    }
}