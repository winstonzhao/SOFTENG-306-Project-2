using System.Collections.Generic;
using GameDialog;

public class UndergradNPC2 : NPC
{
    // Use this for initialization
    public override Dialog GetDialog()
    {
        var me = Toolbox.Instance.GameManager.Player.Username;

        const string npc = "Megan";

        var frame = new DialogFrame(me, "Hello, I'm " + me)
        {
            Next = new DialogFrame(npc, "Nice to meet you!")
            {
                Next = new DialogFrame(npc, "My name's Megan, and I study Software Engineering!")
                {
                    Next = new DialogFrame(me, "Hey, if you don't mind me asking?")
                    {
                        Options = new Dictionary<string, DialogFrame>
                        {
                            {
                                "Why Engineering?", new DialogFrame(npc,
                                    "My choice was less traditional than most, I wasn't sure what I wanted to do, " +
                                    "and Engineering is such a vast industry, which has so much to offer.")
                                {
                                    Next = new DialogFrame(npc,
                                        "Another major consideration was that Engineering gave me structure, once I " +
                                        "chose Engineering I only had to choose one general…")
                                    {
                                        Next = new DialogFrame(npc,
                                            "… education paper and then my next decision was my specialisation at " +
                                            "the end of first year.")
                                        {
                                            Next = new DialogFrame(npc,
                                                "I chose Engineering because I loved problem solving and enjoyed " +
                                                "calculus, and engineering would allow open many different doors " +
                                                "for me.")
                                            {
                                                Next = new DialogFrame(npc,
                                                    "Engineering allows a lot of choice, without an overwhelming " +
                                                    "vastness of options.")
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
                                }
                            },
                            {
                                "Your experience?", new DialogFrame(npc,
                                    "Honestly, I was worried that I wouldn't be smart enough. I loved Calculus and " +
                                    "English, but I was not a fan of Physics, so I was worried that Engineering " +
                                    "wouldn't be the right choice for me.")
                                {
                                    Next = new DialogFrame(npc,
                                        "While it’s true some specialisations require you to know Physics, but many " +
                                        "like Software don’t require it after first year.")
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
                                "Any advice?", new DialogFrame(npc,
                                    "University is not the same as highschool, it demands more attention than you " +
                                    "gave high school.")
                                {
                                    Next = new DialogFrame(npc,
                                        "University gives you a lot of freedom, and it’s important that you focus " +
                                        "on what is most important to you.")
                                    {
                                        Next = new DialogFrame(npc,
                                            "In first year I tried to do uni, work and spend a lot of time with " +
                                            "my friends - you can’t have it all.")
                                        {
                                            Next = new DialogFrame(npc,
                                                "But you do have a very long summer to catch up with all your friends.")
                                            {
                                                Next = new DialogFrame(npc,
                                                    "Balance is important and if you are working part-time, as much " +
                                                    "as you can try and focus on your studies because at the end of " +
                                                    "the day it is more important. ")
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

        var questManager = Toolbox.Instance.QuestManager;
        return new Dialog(frame, directions, () => { questManager.MarkCurrentFinished("networking"); });
    }

    public override void Update()
    {
        var networking = Toolbox.Instance.QuestManager.HasFinishedOrIsCurrent("networking");
        gameObject.SetActive(networking);

        base.Update();
    }
}
