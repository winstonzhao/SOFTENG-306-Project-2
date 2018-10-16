using System.Collections;
using System.Collections.Generic;
using GameDialog;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;

public class GreeterNPC : NPC
{
    private IsoTransform NpcTransform;

    private Animator MovementAnimator;

    private IsoTransform PlayerTransform;

    private ProximityToggle ProximityToggle;

    private Vector3 Origin;

    private bool IntroduceStudentServices;

    private bool IsGreeting;

    // Use this for initialization
    public override Dialog GetDialog()
    {
        var me = Toolbox.Instance.GameManager.Player.Username;

        const string npc = "Naomi";

        DialogFrame frame;

        if (IsGreeting)
        {
            frame = IntroductionDialogFrame(me, npc);
        }
        else if (IntroduceStudentServices)
        {
            frame = IntroduceStudentServicesDialogFrame(me, npc);
        }
        else if (Toolbox.Instance.QuestManager.Current.Id == "visit-leech")
        {
            frame = new DialogFrame(npc, "You should go downstairs via the elevator to visit the workshops");
        }
        else if (Toolbox.Instance.QuestManager.Current.Id.EndsWith("-workshop"))
        {
            var workshop = Toolbox.Instance.QuestManager.Current.Title.ToLower();
            frame = new DialogFrame(npc, "You should go downstairs via the elevator to do your " + workshop);
        }
        else
        {
            switch (Toolbox.Instance.QuestManager.Current.Id)
            {
                case "networking":
                    frame = NetworkingDialogFrame(me, npc);
                    break;
                case "collect-prize":
                    frame = CollectPrizeDialogFrame(me, npc);
                    break;
                default:
                    frame = new DialogFrame(npc, "I hope you had a great time at enginuity day!");
                    break;
            }
        }

        var directions = new Dictionary<string, DialogPosition>
        {
            { me, DialogPosition.Left }, { npc, DialogPosition.Right }
        };

        return new Dialog(frame, directions);
    }

    public override void Start()
    {
        base.Start();

        NpcTransform = gameObject.GetComponent<IsoTransform>();
        MovementAnimator = gameObject.GetComponent<Animator>();
        ProximityToggle = GetComponent<ProximityToggle>();
        PlayerTransform = GameObject.Find("Player").GetComponent<IsoTransform>();

        StartCoroutine(SetDirection());

        if (!Toolbox.Instance.GameManager.Settings.HasBeenGreeted)
        {
            IsGreeting = true;
            IntroduceStudentServices = true;
            StartCoroutine(Greet());
        }
    }

    private IEnumerator SetDirection()
    {
        // Trigger transition to south east for one frame
        MovementAnimator.SetFloat("hSpeed", -1.0f);
        yield return null;
        MovementAnimator.SetFloat("hSpeed", 0f);
    }

    private IEnumerator Greet()
    {
        var toolbox = Toolbox.Instance;
        toolbox.FocusManager.Other = this;

        ProximityToggle.enabled = false;

        var speech = transform.Find("Speech Bubble").GetComponent<SpeechBubble>();
        var alert = transform.Find("Alert Bubble").GetComponent<SpeechBubble>();

        speech.gameObject.SetActive(false);
        alert.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        alert.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        alert.gameObject.SetActive(false);

        var from = NpcTransform.Position;
        var to = PlayerTransform.Position;

        Origin = from;

        yield return StartCoroutine(MoveNextTo(to, 1f));

        ShowDialog();
    }

    private IEnumerator WalkBack()
    {
        var toolbox = Toolbox.Instance;

        // Move back to the original position
        yield return StartCoroutine(MoveTo(Origin, 1f));

        // Face the correct way
        yield return StartCoroutine(SetDirection());

        toolbox.FocusManager.Other = null;
        ProximityToggle.enabled = false;
        IsGreeting = false;
    }

    private IEnumerator MoveNextTo(Vector3 dest, float duration)
    {
        var src = NpcTransform.Position;

        dest = new Vector3(dest.x, dest.y, dest.z);

        // Move directly next to player rather than on top
        dest[0] += src.x > dest.x ? 1f : src.x < dest.x ? -1f : 0f;
        dest[2] += src.z > dest.z ? 1f : src.z < dest.z ? -1f : 0f;

        yield return StartCoroutine(MoveTo(dest, duration));
    }

    private IEnumerator MoveTo(Vector3 dest, float duration)
    {
        var time = 0f;
        var src = NpcTransform.Position;

        var hSpeed = dest.z - src.z;
        var vSpeed = dest.x - src.x;

        MovementAnimator.SetFloat("hSpeed", hSpeed);
        MovementAnimator.SetFloat("vSpeed", vSpeed);
        MovementAnimator.SetBool("vIdle", vSpeed == 0.0f);
        MovementAnimator.SetBool("hIdle", hSpeed == 0.0f);
        MovementAnimator.SetBool("walking", true);

        // Move the NPC next to our player
        while (time < duration)
        {
            NpcTransform.Position = Vector3.Lerp(src, dest, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        MovementAnimator.SetFloat("hSpeed", 0f);
        MovementAnimator.SetFloat("vSpeed", 0f);
        MovementAnimator.SetBool("vIdle", true);
        MovementAnimator.SetBool("hIdle", true);
        MovementAnimator.SetBool("walking", false);
    }

    private DialogFrame IntroduceStudentServicesDialogFrame(string me, string npc)
    {
        // This is what the dialog eventually jumps to, different options lead to this - which is why it's a variable
        var timetable = new DialogFrame(npc, "It’s your timetable for the day!")
        {
            Next = new DialogFrame(npc,
                "Here you will find the workshops you will be attending as well as the networking event in " +
                "the afternoon.")
            {
                Next = new DialogFrame(npc,
                    "I hope you find this useful. There is also a list of items you should complete before the " +
                    "day is over.")
                {
                    Next = new DialogFrame(npc,
                        "Each time you complete a task it will be ticked off your list. Complete all tasks and " +
                        "return back to this stall for a prize!")
                    {
                        Next = new DialogFrame(me, "Thanks!")
                        {
                            Next = new DialogFrame(npc,
                                "No problem! Use the elevator to visit the workshops in the engineering leech")
                            {
                                OnComplete = () =>
                                {
                                    Toolbox.Instance.QuestManager.MarkFinished("get-timetable");
                                    Toolbox.Instance.GameManager.Settings.HasBeenGreeted = true;
                                    IntroduceStudentServices = false;
                                }
                            }
                        }
                    }
                }
            }
        };

        return new DialogFrame(npc,
            "This is the Student Services stall, where students come to pick up their assignments and exam scripts.")
        {
            Next = new DialogFrame(npc, "I have an important file for you too!")
            {
                Next = new DialogFrame(npc, "I wonder what it is…")
                {
                    Options = new Dictionary<string, DialogFrame>
                    {
                        {
                            "Okay", new DialogFrame(me, "What is it?")
                            {
                                Next = timetable
                            }
                        },
                        {
                            "Oh no!", new DialogFrame(me, "I hope it's not an assignment")
                            {
                                Next = new DialogFrame(npc, "No…")
                                {
                                    Next = timetable
                                }
                            }
                        }
                    }
                }
            }
        };
        ;
    }

    private DialogFrame IntroductionDialogFrame(string me, string npc)
    {
        return new DialogFrame(npc, "Hey " + me + ", welcome to Enginuity Day!")
        {
            Next = new DialogFrame(npc,
                "It is wonderful that you could join us. My name is Naomi, let me show you around.")
            {
                Next = new DialogFrame(npc,
                    "This is the engineering lobby where you will find the Student Services stall, shall we take a look?")
                {
                    Next = new DialogFrame(npc, "Use your arrow keys to follow me.")
                    {
                        // NPC moves back to the origin after talking to the player
                        OnComplete = () => StartCoroutine(WalkBack())
                    }
                }
            }
        };
    }

    private DialogFrame NetworkingDialogFrame(string me, string npc)
    {
        return new DialogFrame(npc,
            "It's so nice to see other students and graduates back at university.")
        {
            Next = new DialogFrame(npc,
                "This is a great chance for you to find out more about engineering in general.")
            {
                Next = new DialogFrame(me,
                    "I should go speak to these people…")
            }
        };
    }

    private DialogFrame CollectPrizeDialogFrame(string me, string npc)
    {
        return new DialogFrame(npc,
            "Well done, I see you have come to collect your prize.")
        {
            Next = new DialogFrame(npc,
                "Just remember, this is just an indicator of one path you could take after you leave high school.")
            {
                Next = new DialogFrame(npc,
                    "The decision should entirely be your own, but I hope you have gained some valuable insight " +
                    "from our current engineering students.")
                {
                    Next = new DialogFrame(npc,
                        "All the specialisations are fantastic, and in first year engineering you will get the " +
                        "chance to experience them all before you make your decision to specialise.")
                    {
                        Next = new DialogFrame(npc,
                            "Once again, I hope you have enjoyed your Enginuity Day.")
                        {
                            Next = new DialogFrame(npc,
                                "You can find your prize in your backpack.")
                            {
                                OnComplete = () => { Toolbox.Instance.QuestManager.MarkFinished("post-workshops"); }
                            }
                        }
                    }
                }
            }
        };
    }
}
