using System.Collections.Generic;
using GameDialog;

public class CivilNPC : NPC
{
    // Use this for initialization
    public override Dialog GetDialog()
    {
        var me = Toolbox.Instance.GameManager.Player.Username;

        const string npc = "Civil Instructor";

        DialogFrame frame;

        if (Toolbox.Instance.QuestManager.HasFinishedOrIsCurrent("civil-workshop"))
        {
            frame = new DialogFrame(me, "Hello, I'm " + me)
            {
                Next = new DialogFrame(npc, "Hi, I'm the " + npc + "!")
                {
                    Options = new Dictionary<string, DialogFrame>
                    {
                        {
                            "What is Civil?", new DialogFrame(npc,
                                "Civil engineering is a professional engineering discipline that deals with the design, " +
                                "construction, and maintenance of the physical and naturally built environment, " +
                                "including works such as roads, bridges, canals, dams, airports, sewerage systems, " +
                                "pipelines, and railways.")
                            {
                                Next = new DialogFrame(npc,
                                    "Civil engineering is traditionally broken into a number of sub-disciplines. It " +
                                    "is considered the second-oldest engineering discipline after military " +
                                    "engineering, and it is defined to distinguish non-military engineering from " +
                                    "military engineering.")
                                {
                                    Next = new DialogFrame(npc,
                                        "Civil engineering takes place in the public sector from municipal through " +
                                        "to national governments, and in the private sector from individual " +
                                        "homeowners through to international companies.")
                                    {
                                        Next = new DialogFrame(me, "Wow, Civil sounds really great!")
                                    }
                                }
                            }
                        },
                        {
                            "Play Civil Game", new DialogFrame(me, "Alright let's play!")
                            {
                                TransitionToScene = "Civil Level 1"
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
