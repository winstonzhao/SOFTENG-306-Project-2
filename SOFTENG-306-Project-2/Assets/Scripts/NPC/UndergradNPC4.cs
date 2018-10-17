using System.Collections.Generic;
using GameDialog;

public class UndergradNPC4 : NPC
{
    // Use this for initialization
    public override Dialog GetDialog()
    {
        var me = Toolbox.Instance.GameManager.Player.Username;

        const string npc = "Susanne";

        var frame = new DialogFrame(me, "Hello, I'm " + me)
        {
            Next = new DialogFrame(npc, "Nice to meet you!")
            {
                Next = new DialogFrame(npc, "My name's " + npc + ", and I study CHEMMAT!")
                {
                    Next = new DialogFrame(me, "Hey, if you don't mind me asking?")
                    {
                        Options = new Dictionary<string, DialogFrame>
                        {
                            {
                                "Why Engineering?", new DialogFrame(npc,
                                    "I really liked the idea of helping other people and making a change in the " +
                                    "world. I think that engineering shows so much growth and potential and I really " +
                                    "wished to be a part of that.")
                                {
                                    Next = new DialogFrame(npc,
                                        "I had talked to a few female students beforehand and they also really " +
                                        "enjoyed the experience and that inspired me to take this course")
                                    {
                                        Next = new DialogFrame(me, "Oh wow, that's really interesting!")
                                        {
                                            Next = new DialogFrame(npc, "Thanks, no problem, come to talk me anytime!")
                                        }
                                    }
                                }
                            },
                            {
                                "Your experience?", new DialogFrame(npc,
                                    "I was really worried that perhaps my skills weren't satisfactory enough to be " +
                                    "able to show my full potential as an engineer.")
                                {
                                    Next = new DialogFrame(npc,
                                        "However, I ended up making good friends who I have established a study " +
                                        "group with in which us as a group can grow together and inspire one another.")
                                    {
                                        Next = new DialogFrame(me, "Oh wow, that's really interesting!")
                                        {
                                            Next = new DialogFrame(npc, "Thanks, no problem, come to talk me anytime!")
                                        }
                                    }
                                }
                            },
                            {
                                "Any advice?", new DialogFrame(npc,
                                    "I would advise younger students to go out and talk to those who are taking " +
                                    "the course now.")
                                {
                                    Next = new DialogFrame(npc,
                                        "I'm sure that it would be inspiring to them and would help them decide on " +
                                        "where their future paths might lead them.")
                                    {
                                        Next = new DialogFrame(npc,
                                            "I would also advise the younger female students to get involved with " +
                                            "WEN and see the future of engineering for females.")
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
