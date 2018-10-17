using System.Collections.Generic;
using GameDialog;

public class SoftwareNPC : InstructorNPC
{
    protected override string Name
    {
        get { return "Rashina"; }
    }

    protected override string QuestId
    {
        get { return "software-workshop"; }
    }

    protected override DialogFrame GetInstructorDialog(string me)
    {
        return new DialogFrame(me, "Hello, I'm " + me)
        {
            Next = new DialogFrame(Name, "Hi, I'm " + Name + " the software instructor!")
            {
                Options = new Dictionary<string, DialogFrame>
                {
                    {
                        "What is Software?", new DialogFrame(Name,
                            "A Bachelor of Engineering (Honours) in Software Engineering focuses on giving you " +
                            "the skills to engineer large, complex and fault-tolerant systems that function " +
                            "reliably, are effectively developed, and can be maintained efficiently.")
                        {
                            Next = new DialogFrame(Name,
                                "Beyond imparting fundamental knowledge, the specialisation also prepares you for " +
                                "the technological environment ahead…")
                            {
                                Next = new DialogFrame(Name,
                                    "…so you can pursue courses and applications in areas such as human-computer " +
                                    "interaction, serious games, smart energy consumption...")
                                {
                                    Next = new DialogFrame(Name,
                                        "…learning aids, autonomous robots, and intelligent software agents.")
                                    {
                                        Next = new DialogFrame(me, "Wow that's really interesting! Thanks!")
                                    }
                                }
                            }
                        }
                    },
                    {
                        "Workshop", new DialogFrame(Name, "Alright let's do the workshop!")
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
}
