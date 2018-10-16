using System.Collections.Generic;
using GameDialog;

public class ElectricalNPC : NPC
{
    // Use this for initialization
    public override Dialog GetDialog()
    {
        var me = Toolbox.Instance.GameManager.Player.Username;

        const string npc = "Electrical Instructor";

        DialogFrame frame;

        if (Toolbox.Instance.QuestManager.HasFinishedOrIsCurrent("electrical-workshop"))
        {
            frame = new DialogFrame(me, "Hello, I'm " + me)
            {
                Next = new DialogFrame(npc, "Hi, I'm the " + npc + "!")
                {
                    Options = new Dictionary<string, DialogFrame>()
                    {
                        {
                            "What is ELECTENG?", new DialogFrame(npc,
                                "Electrical engineering is a professional engineering discipline that generally deals " +
                                "with the study and application of electricity, electronics, and electromagnetism.")
                            {
                                Next = new DialogFrame(npc,
                                    "Electrical engineers work in a very wide range of industries and the skills " +
                                    "required are likewise variable.")
                                {
                                    Next = new DialogFrame(npc,
                                        "These range from basic circuit theory to the management skills required " +
                                        "of a project manager.")
                                    {
                                        Next = new DialogFrame(npc,
                                            "The tools and equipment that an individual engineer may need are " +
                                            "similarly variable, ranging from a simple voltmeter to a top end " +
                                            "analyzer to sophisticated design and manufacturing software")
                                        {
                                            Next = new DialogFrame(me, "Wow! That's really cool!")
                                        }
                                    }
                                }
                            }
                        },
                        {
                            "Play Electrical Game", new DialogFrame(npc, "Alright let's play!")
                            {
                                TransitionToScene = "Welcome Screen"
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
