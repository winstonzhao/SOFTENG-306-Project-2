using UnityEngine;

namespace Instructions
{
    public abstract class InstructionRenderer : MonoBehaviour
    {
        private Color defaultBackgroundColor = new Color(0.4f, 0.7f, 0.9f);
        public virtual Color DefaultBackgroundColor
        {
            get { return defaultBackgroundColor; }
            set
            {
                defaultBackgroundColor = value;
                backgroundColor = value;
                Render();
            }
        }

        public static Color FailBackgroundColor = new Color(1, .5f, .5f);
        public static Color ExecutingBackgroundColor = new Color(0, 1, 0);
        public static Color SelectedBackgroundColor = new Color(0, 0, 1);

        protected Color backgroundColor = new Color(0.4f, 0.7f, 0.9f);
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