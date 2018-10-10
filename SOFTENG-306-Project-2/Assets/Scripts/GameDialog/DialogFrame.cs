using System.Collections.Generic;

namespace GameDialog
{
    public class DialogFrame
    {
        public string Name { get; private set; }

        public string Text { get; private set; }

        public DialogFrame Next;

        public bool ButtonFrame { get; private set; }

        private Dictionary<string, DialogFrame> options;
        public Dictionary<string, DialogFrame> Options
        {
            get { return options; }
            set
            {
                ButtonFrame = true;
                options = value;
            }
        }

        public bool TransitionFrame { get; private set; }

        private string transitionToScene;
        public string TransitionToScene
        {
            get { return transitionToScene; }
            set
            {
                TransitionFrame = true;
                transitionToScene = value;
            }
        }

        public DialogFrame(string name, string text)
        {
            TransitionFrame = false;
            ButtonFrame = false;
            Text = text;
            Name = name;
        }
    }
}
