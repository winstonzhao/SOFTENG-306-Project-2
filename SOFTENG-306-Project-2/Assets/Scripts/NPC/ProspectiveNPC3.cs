using System.Collections.Generic;
using GameDialog;

public class ProspectiveNPC3 : NPC
{
    // Use this for initialization
    public override Dialog GetDialog()
    {
        const string npc = "Minerva McGonagall";

        var frame = new DialogFrame(npc, "Helloooooooo!")
        {
            Next = new DialogFrame(npc, "I'm Minerva!")
            {
                Next = new DialogFrame(npc, "I'm really enjoying today!")
                {
                    Next = new DialogFrame(npc,
                        "One of the more striking comments from the student panel was, 'don’t hesitate in " +
                        "taking opportunities.'")
                    {
                        Next = new DialogFrame(npc,
                            "So whether you have any doubts, or if you are dead set on engineering, you may find " +
                            "yourself pleasantly surprised")
                        {
                            Next = new DialogFrame(npc,
                                "as I have. It was because of this event that I decided to shed my perpetual state " +
                                "of ambiguity with my career choice, and set my heart on becoming an engineer.")
                        }
                    }
                }
            }
        };

        var directions = new Dictionary<string, DialogPosition>
        {
            { npc, DialogPosition.Right }
        };

        var achievementsManager = Toolbox.Instance.AchievementsManager;
        return new Dialog(frame, directions, () => { achievementsManager.MarkCompleted("talk-to-student"); });
    }
}
