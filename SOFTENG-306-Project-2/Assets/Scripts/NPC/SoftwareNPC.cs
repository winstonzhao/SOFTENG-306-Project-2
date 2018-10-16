using System.Collections.Generic;
using GameDialog;

public class SoftwareNPC : NPC
{
    // Use this for initialization
    public override Dialog GetDialog()
    {
        var me = Toolbox.Instance.GameManager.Player.Username;

        const string npc = "Software Instructor";

        DialogFrame frame;

        if (Toolbox.Instance.QuestManager.HasFinishedOrIsCurrent("software-workshop"))
        {
            frame = new DialogFrame(me, "Hello, I'm " + me)
            {
                Next = new DialogFrame(npc, "Hi, I'm the " + npc + "!")
                {
                    Options = new Dictionary<string, DialogFrame>
                    {
                        {
                            "What is SOFTENG?", new DialogFrame(npc,
                                "A Bachelor of Engineering (Honours) in Software Engineering focuses on giving you " +
                                "the skills to engineer large, complex and fault-tolerant systems that function " +
                                "reliably, are effectively developed, and can be maintained efficiently.")
                            {
                                Next = new DialogFrame(npc,
                                    "Beyond imparting fundamental knowledge, the specialisation also prepares you for " +
                                    "the technological environment ahead…")
                                {
                                    Next = new DialogFrame(npc,
                                        "…so you can pursue courses and applications in areas such as human-computer " +
                                        "interaction, serious games, smart energy consumption...")
                                    {
                                        Next = new DialogFrame(npc,
                                            "…learning aids, autonomous robots, and intelligent software agents.")
                                        {
                                            Next = new DialogFrame(me, "Wow that's really interesting! Thanks!")
                                        }
                                    }
                                }
                            }
                        },
                        {
                            "Play Software Game", new DialogFrame(npc, "Alright let's play!")
                            {
                                TransitionToScene = "SoftwareMinigameWithInstructions"
                            }
                        },
                        {
                            "Bye", new DialogFrame(me, "Bye!")
                        }
                    }
                }
            };
        }
        else
        {
            frame = new DialogFrame(npc,
                "Sorry it looks like you aren't enrolled for this workshop right now, check your timetable to see " +
                "where you should be going.");
        }

        var directions = new Dictionary<string, DialogPosition>
        {
            { me, DialogPosition.Left }, { npc, DialogPosition.Right }
        };

        return new Dialog(frame, directions);
    }
}
