using System.Collections.Generic;
using GameDialog;

public class ProspectiveNPC : NPC
{
    // Use this for initialization
    public override Dialog GetDialog()
    {
        const string npc = "Cho Chang";

        var frame = new DialogFrame(npc, "Hey, I'm " + npc + ", today is so exiting!")
        {
            Next = new DialogFrame(npc, "I've learnt so many interesting things!")
            {
                Next = new DialogFrame(npc,
                    "This day really exposed a variety of things you can do with an engineering degree and the " +
                    "places it can take you.")
                {
                    Next = new DialogFrame(npc,
                        "Talking to many people who have been through the same process…")
                    {
                        Next = new DialogFrame(npc,
                            "…you’re currently wading through was incredibly helpful as they hold the wisdom " +
                            "of hindsight, thus knowing how the degree works in its entirety.")
                        {
                            Next = new DialogFrame(npc,
                                "Meeting students from all over the country was also a wonderful experience, a " +
                                "room full of intelligent young women brimming with potential…")
                            {
                                Next = new DialogFrame(npc,
                                    "…and all glad to be given the chance to explore the option of engineering " +
                                    "in this busy day.")
                            }
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
