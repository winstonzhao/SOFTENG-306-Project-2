using System.Collections.Generic;
using GameDialog;

public class UndergradNPC : NPC
{
    // Use this for initialization
    public override Dialog GetDialog()
    {
        var me = Toolbox.Instance.GameManager.Player.Username;

        const string npc = "Tina";

        var frame = new DialogFrame(me, "Hi, I'm " + me)
        {
            Next = new DialogFrame(npc, "Nice to meet you!")
            {
                Next = new DialogFrame(npc, "My name's " + npc + ", and I study Software Engineering!")
                {
                    Next = new DialogFrame(me, "Hey, if you don't mind me asking?")
                    {
                        Options = new Dictionary<string, DialogFrame>
                        {
                            {
                                "Why Engineering?", new DialogFrame(npc,
                                    "I chose engineering as I enjoyed design and science at school.")
                                {
                                    Next = new DialogFrame(npc,
                                        "I had researched engineering and found that it offered a good combination " +
                                        "of both theoretical and practical skills that would be useful for many " +
                                        "career paths. ")
                                    {
                                        Next = new DialogFrame(npc,
                                            "Oooo, and by the way, you might want to head to the leech and to talk " +
                                            "to Katherine, the software advisor!")
                                        {
                                            Next = new DialogFrame(me,
                                                "Thanks a lot! Your insight will definitely be super helpful in " +
                                                "helping me decide whether engineering is for me!")
                                        }
                                    }
                                }
                            },
                            {
                                "Your experience?", new DialogFrame(npc,
                                    "I was nervous about the programming paper that is taken by all engineering " +
                                    "students in their first year.")
                                {
                                    Next = new DialogFrame(npc,
                                        "I had never done any coding before and was worried that I may fall " +
                                        "behind the other students.")
                                    {
                                        Next = new DialogFrame(npc,
                                            "However, the class ended up being a very good introduction to the " +
                                            "basics of programming, and I enjoyed it a lot and chose to continue " +
                                            "in software as my specialisation.")
                                        {
                                            Next = new DialogFrame(npc,
                                                "Oooo, and by the way, you might want to head to the leech and " +
                                                "to talk to Katherine, the software advisor!")
                                            {
                                                Next = new DialogFrame(me,
                                                    "Thanks alot! Your insight will definitely be super helpful " +
                                                    "in helping me decide whether engineering is for me!")
                                            }
                                        }
                                    }
                                }
                            },
                            {
                                "Any advice?", new DialogFrame(npc,
                                    "I would encourage everyone to stay open-minded and give everything a try. " +
                                    "There are many clubs and events that university offers, both within and " +
                                    "outside of engineering.")
                                {
                                    Next = new DialogFrame(npc,
                                        "They are a great place to meet new people and make friends.")
                                    {
                                        Next = new DialogFrame(npc,
                                            "Engineering especially is a degree that focuses a lot on teamwork and " +
                                            "collaboration, so having a good group of friends is really important.")
                                        {
                                            Next = new DialogFrame(npc,
                                                "Oooo, and by the way, you might want to head to the leech and to " +
                                                "talk to Katherine, the software advisor!")
                                            {
                                                Next = new DialogFrame(me,
                                                    "Thanks alot! Your insight will definitely be super helpful in " +
                                                    "helping me decide whether engineering is for me!")
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
