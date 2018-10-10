using System.Collections.Generic;
using GameDialog;

public class DoorNPC : NPC
{
    public string Level;

    public override Dialog GetDialog()
    {
        var me = Toolbox.Instance.GameManager.Player.Username;

        var frame = new DialogFrame(me, "This is going to take me to Engineering Leech.")
        {
            Options = new Dictionary<string, DialogFrame>
            {
                {
                    "Let's go!", new DialogFrame(me, "Let's go!")
                    {
                        TransitionToScene = Level
                    }
                },
                {
                    "I want to stay!", new DialogFrame(me, "On second thought…")
                }
            }
        };

        var directions = new Dictionary<string, DialogPosition>
        {
            { me, DialogPosition.Left }
        };

        return new Dialog(frame, directions);
    }
}
