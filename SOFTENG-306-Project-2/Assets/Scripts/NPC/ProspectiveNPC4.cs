using System.Collections.Generic;
using GameDialog;

public class ProspectiveNPC4 : NPC
{
    // Use this for initialization
    public override Dialog GetDialog()
    {
        var me = Toolbox.Instance.GameManager.Player.Username;

        const string npc = "Harry Potter";

        var frame = new DialogFrame(npc, "Why hello there!")
        {
            Next = new DialogFrame(npc, "I'm Harry Potter (the chosen one)!")
            {
                Next = new DialogFrame(me, "The chosen what?")
                {
                    Next = new DialogFrame(npc, "…nevermind, how was your day?")
                    {
                        Next = new DialogFrame(me,
                            "It was good, I really enjoyed talking to all the other prospective students… like " +
                            "yourself! How about yourself?")
                        {
                            Next = new DialogFrame(npc,
                                "Well, there are more opportunities in engineering than I previously realized – it’s " +
                                "more than Calculus and Physics, it’s practical.")
                            {
                                Next = new DialogFrame(npc,
                                    "It can be a glove controlled shooter game or a smartphone gaming app, for " +
                                    "example – engineering can be for pleasure.")
                                {
                                    Next = new DialogFrame(npc,
                                        "This enginuity day showcased all the ‘hidden’ aspects of engineering, and " +
                                        "promoted understanding in a helpful way that I would recommend to any " +
                                        "students considering working in the field.")
                                    {
                                        Next = new DialogFrame(npc,
                                            "I learnt that the future is limitless; it is not bound by gender or " +
                                            "stereotypes.")
                                        {
                                            Next = new DialogFrame(me, "Oh wow! Really interesting!")
                                        }
                                    }
                                }
                            }
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
