using System.Collections.Generic;
using GameDialog;

public class ProspectiveNPC2 : NPC
{
    // Use this for initialization
    public override Dialog GetDialog()
    {
        var me = Toolbox.Instance.GameManager.Player.Username;

        const string npc = "Bellatrix Lestrange";

        var frame = new DialogFrame(npc, "I'm Lestrange, " + npc + "!")
        {
            Next = new DialogFrame(npc, "Nice to make your acquaintance…")
            {
                Next = new DialogFrame(me, "Are you enjoying the exhibition day?")
                {
                    Next = new DialogFrame(npc, "Well…")
                    {
                        Next = new DialogFrame(npc,
                            "We each have a unique toolbox comprised of life experience, daily experience, school " +
                            "experience, sport experience, and anything else that we know without knowing we know…")
                        {
                            Next = new DialogFrame(npc,
                                "…which can empower us to improve the conditions of life we see around us, if we " +
                                "know how to be creative in how we approach this toolbox.")
                        }
                    }
                }
            }
        };

        var directions = new Dictionary<string, DialogPosition>
        {
            { me, DialogPosition.Left }, { npc, DialogPosition.Right }
        };

        var achievementsManager = Toolbox.Instance.AchievementsManager;
        return new Dialog(frame, directions, () => { achievementsManager.MarkCompleted("talk-to-student"); });
    }
}
