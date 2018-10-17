using System.Collections.Generic;
using GameDialog;

public class UndergradNPC5 : NPC
{
    // Use this for initialization
    public override Dialog GetDialog()
    {
        var me = Toolbox.Instance.GameManager.Player.Username;

        const string npc = "Hannah";

        var frame = new DialogFrame(me, "Hello, I'm " + me)
        {
            Next = new DialogFrame(npc, "Nice to meet you!")
            {
                Next = new DialogFrame(npc, "My name's " + npc + ", and I study engineering science!")
                {
                    Next = new DialogFrame(me, "Hey, is it ok if I ask you a couple of questions?")
                    {
                        Options = new Dictionary<string, DialogFrame>
                        {
                            {
                                "Why Engineering?", new DialogFrame(npc,
                                    "I chose engineering as I have always loved using maths and" +
                                    " science to solve problems.")
                                {
                                    Next = new DialogFrame(npc,
                                        "Being curious about why things work the way they do also made" +
                                        " me interested in engineering as a career choice, as through engineering" +
                                        " I could get a better understanding of the world around me.")
                                    {
                                        Next = new DialogFrame(me, "Wow that's so cool!")
                                        {
                                            Next = new DialogFrame(npc,
                                                "It really is. Come back and talk to me anytime.")
                                        }
                                    }
                                }
                            },
                            {
                                "Your experience?", new DialogFrame(npc,
                                    "My biggest concern coming in was not meeting new people. " +
                                    "I thought with engineering being an academic heavy subject, " +
                                    "and the fact that people joke about engineers being anti-social, " +
                                    "I wouldn’t be able to find many new people to make friends with.")
                                {
                                    Next = new DialogFrame(npc,
                                        "Everyone is really lovely though, and there is a lot of group work " +
                                        "where you get to meet new people.")
                                    {
                                        Next = new DialogFrame(me,
                                            "Oh wow, that's really interesting!")
                                        {
                                            Next = new DialogFrame(npc,
                                                "Thanks, no problem, come to talk me anytime!")
                                        }                                
                                    }
                                }
                            },
                            {
                                "Any advice?", new DialogFrame(npc,
                                    "I would advise that you go into university with an idea of meeting new people. " +
                                    "If you go in with a lot of friends you are already close to, that’s great! ")
                                {
                                    Next = new DialogFrame(npc,
                                        "But having new friends who study the same thing as you can really help " +
                                        "encourage you to get through hard assignments and tests.")
                                    {
                                        Next = new DialogFrame(npc,
                                            "Plus creating a good network of contacts early on is only going to help you further on in life and in your career!")
                                        {
                                            Next = new DialogFrame(me, "Thank you, that's really helpful!")
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
