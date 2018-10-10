using System.Collections.Generic;

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

        public Dialog(DialogFrame startFrame, Dictionary<string, DialogPosition> directions)
        {
            StartFrame = startFrame;
            Directions = directions;
        }
    }
}
