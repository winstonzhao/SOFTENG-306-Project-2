using System.Collections.Generic;
using GameDialog;

public class UndergradNPC6 : NPC
{
    // Use this for initialization
    public override Dialog GetDialog()
    {
        var me = Toolbox.Instance.GameManager.Player.Username;

        const string npc = "Sophie";

        var frame = new DialogFrame(me, "Hello, I'm " + me)
        {
            Next = new DialogFrame(npc, "Nice to meet you!")
            {
                Next = new DialogFrame(npc, "My name's " + npc + ", and I study biomedical engineering!")
                {
                    Next = new DialogFrame(me, "Is it alright if I ask you some questions?")
                    {
                        Options = new Dictionary<string, DialogFrame>
                        {
                            {
                                "Why Engineering?", new DialogFrame(npc,
                                    "I loved graphics, physics and biology in high school " +
                                    "and engineering was the best mix. ")
                                {
                                    Next = new DialogFrame(npc,
                                        "Biomedical engineering allows me to combine this love of both whilst constantly" +
                                        "challenging me mentally.")
                                    {
                                        Next = new DialogFrame(me, "That's so awesome, I might like the visual side too!")
                                        {
                                            Next = new DialogFrame(npc,
                                                "It sure is. Come back and talk to me anytime.")
                                        }
                                    }
                                }
                            },
                            {
                                "Your experience?", new DialogFrame(npc,
                                    "I was scared because I didn't have " +
                                    "any coding, or really much " +
                                    "mechanical knowledge that I would struggle.")
                                {
                                    Next = new DialogFrame(npc,
                                        "But the first year courses were very approachable. " +
                                        "The lecturers really do their best to help you get the skills you need and are" +
                                        "always keen to give you their extra time.")
                                    {
                                        Next = new DialogFrame(me,
                                            "Oh wow, that's reassuring!")
                                        {
                                            Next = new DialogFrame(npc,
                                                "No problem, come to talk me anytime!")
                                        }                                
                                    }
                                }
                            },
                            {
                                "Any advice?", new DialogFrame(npc,
                                    "Talk to as many second years as you can!")
                                {
                                    Next = new DialogFrame(npc,
                                        " While the courses may not offer the best representation" +
                                        " of the specialisation, those that have done it will tell you how it is.")
                                    {
                                        Next = new DialogFrame(me, "Thank you, that's really helpful!")
                                        {
                                            Next = new DialogFrame(npc,
                                                "Thanks for talking to me, come back anytime!")
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

        return new Dialog(frame, directions, () =>
        {
            Toolbox.Instance.QuestManager.MarkCurrentFinished("networking");
            Toolbox.Instance.AchievementsManager.MarkCompleted("speak-undergrad");
        });
    }

    public override void Update()
    {
        var networking = Toolbox.Instance.QuestManager.HasFinishedOrIsCurrent("networking");
        gameObject.SetActive(networking);

        base.Update();
    }
}
