using System.Collections.Generic;
using GameDialog;

public class CivilNPC : InstructorNPC
{
    protected override string Name
    {
        get { return "Kelsey"; }
    }

    protected override string QuestId
    {
        get { return "civil-workshop"; }
    }

    protected override DialogFrame GetInstructorDialog(string me)
    {
        return new DialogFrame(me, "Hello, I'm " + me)
        {
            Next = new DialogFrame(Name, "Hi, I'm " + Name + ", the civil instructor!")
            {
                Options = new Dictionary<string, DialogFrame>
                {
                    {
                        "What is Civil?", new DialogFrame(Name,
                            "Civil engineering is a professional engineering discipline that deals with the design, " +
                            "construction, and maintenance of the physical and naturally built environment, " +
                            "including works such as roads, bridges, canals, dams, airports, sewerage systems, " +
                            "pipelines, and railways.")
                        {
                            Next = new DialogFrame(Name,
                                "Civil engineering is traditionally broken into a number of sub-disciplines. It " +
                                "is considered the second-oldest engineering discipline after military " +
                                "engineering, and it is defined to distinguish non-military engineering from " +
                                "military engineering.")
                            {
                                Next = new DialogFrame(Name,
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
                        "Workshop", new DialogFrame(me, "Alright let's do the workshop!")
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
}
