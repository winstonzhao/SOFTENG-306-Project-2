using System.Collections.Generic;
using Utils;

namespace GameDialog
{
    public class DialogFrame
    {
        /// <summary>
        /// The name of the speaker
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// What the speaker is saying in this dialog frame
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// The subsequent dialog frame - if null the dialog finishes here; UNLESS there are options
        /// </summary>
        public DialogFrame Next;

        /// <summary>
        /// Whether the dialog frame expects the user to select an option from <see cref="Options"/>
        /// </summary>
        public bool ButtonFrame { get; private set; }

        private Dictionary<string, DialogFrame> options;
        /// <summary>
        /// The options to display & their subsequent dialog frames
        /// </summary>
        public Dictionary<string, DialogFrame> Options
        {
            get { return options; }
            set
            {
                ButtonFrame = true;
                options = value;
            }
        }

        /// <summary>
        /// Whether at the end of this frame, the game should transition to the given <see cref="TransitionToScene"/>
        /// </summary>
        public bool TransitionFrame { get; private set; }

        private string transitionToScene;
        /// <summary>
        /// If set, this is the scene to transition to at the end of the dialog frame
        /// </summary>
        public string TransitionToScene
        {
            get { return transitionToScene; }
            set
            {
                TransitionFrame = true;
                transitionToScene = value;
            }
        }

        /// <summary>
        /// Callback for when the dialog frame is completed
        /// </summary>
        public Runnable OnComplete;

        public DialogFrame(string name, string text)
        {
            TransitionFrame = false;
            ButtonFrame = false;
            Text = text;
            Name = name;
        }
    }
}
