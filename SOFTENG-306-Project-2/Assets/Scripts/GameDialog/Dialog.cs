using System.Collections.Generic;
using Utils;

namespace GameDialog
{
    public enum DialogPosition
    {
        Left,
        Right
    }

    /// <summary>
    /// Representation of a dialog - a dialog is composed of frames
    /// </summary>
    public class Dialog
    {
        /// <summary>
        /// The initial piece of dialog to visit
        /// </summary>
        public readonly DialogFrame StartFrame;

        /// <summary>
        /// The position of the dialog sprites - whether the NPC should be on the left or right, and whether
        /// the user should be on the left or right etc
        /// </summary>
        public readonly Dictionary<string, DialogPosition> Directions;

        /// <summary>
        /// Callback for when the dialog is closed
        /// </summary>
        public readonly Runnable OnComplete;

        public Dialog(DialogFrame startFrame, Dictionary<string, DialogPosition> directions, Runnable onComplete = null)
        {
            StartFrame = startFrame;
            Directions = directions;
            OnComplete = onComplete;
        }
    }
}
