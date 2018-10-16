using System.Collections.Generic;
using Utils;

namespace GameDialog
{
    public enum DialogPosition
    {
        Left,
        Right
    }

    public class Dialog
    {
        public readonly DialogFrame StartFrame;

        public readonly Dictionary<string, DialogPosition> Directions;

        public readonly Runnable OnComplete;

        public Dialog(DialogFrame startFrame, Dictionary<string, DialogPosition> directions, Runnable onComplete = null)
        {
            StartFrame = startFrame;
            Directions = directions;
            OnComplete = onComplete;
        }
    }
}
