using System.Collections.Generic;
using GameDialog;

public class UndergradNPC3 : NPC
{
    // Use this for initialization
    public override Dialog GetDialog()
    {
        var me = Toolbox.Instance.GameManager.Player.Username;

        const string npc = "Saachi";

        var frame = new DialogFrame(me, "Hello, I'm " + me)
        {
            Next = new DialogFrame(npc, "Nice to meet you!")
            {
                Next = new DialogFrame(npc, "My name's " + npc + ", and I study mechantronics!")
                {
                    Next = new DialogFrame(me, "Hey, if you don't mind me asking?")
                    {
                        Options = new Dictionary<string, DialogFrame>
                        {
                            {
                                "Why Engineering?", new DialogFrame(npc,
                                    "I enjoyed both maths and physics at school, and also really enjoyed the design " +
                                    "subjects that I had creative freedom with.")
                                {
                                    Next = new DialogFrame(npc,
                                        "Engineering, in particular mechanical/mechatronics combines these 2 " +
                                        "aspects well.")
                                    {
                                        Next = new DialogFrame(me, "Oh wow, that's really interesting!")
                                        {
                                            Next = new DialogFrame(npc,
                                                "Thanks, no problem, come to talk me anytime!")
                                        }
                                    }
                                }
                            },
                            {
                                "Your experience?", new DialogFrame(npc,
                                    "The biggest concern I had about engineering wasn’t about the actual coursework, " +
                                    "but about how I would be able to make friends if none of my school friends took " +
                                    "my specialisation.")
                                {
                                    Next = new DialogFrame(npc,
                                        "Things like not having a lab partner, project partner, groups for teams etc.")
                                    {
                                        Next = new DialogFrame(npc,
                                            "Turns out that once you get into your specialisation, you spend a lot " +
                                            "more time with them in labs, classes, tutorials etc and you naturally " +
                                            "get to know them better.")
                                        {
                                            Next = new DialogFrame(npc,
                                                "Everyone is also really friendly and willing to help out, so " +
                                                "there’s no need to worry about that after all!")
                                            {
                                                Next = new DialogFrame(me,
                                                    "Oh wow, that's really interesting!")
                                                {
                                                    Next = new DialogFrame(npc,
                                                        "Thanks, no problem, come to talk me anytime!")
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            {
                                "Any advice?", new DialogFrame(npc,
                                    "My advice is to make the most of the facilities and engineering opportunities " +
                                    "offered by the faculty.")
                                {
                                    Next = new DialogFrame(npc,
                                        "There’s heaps of events, workshops, meetups, information evenings and " +
                                        "people to answer any questions you have.")
                                    {
                                        Next = new DialogFrame(npc,
                                            "WEN is also another great community which offers exclusive events for " +
                                            "girls that build on your skills and also allows a forum to make great " +
                                            "new friends")
                                        {
                                            Next = new DialogFrame(me, "Oh wow, that's really interesting!")
                                            {
                                                Next = new DialogFrame(npc,
                                                    "Thanks, no problem, come to talk me anytime!")
                                            }
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
