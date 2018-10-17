using System.Collections.Generic;
using GameDialog;

public class ElectricalNPC : InstructorNPC
{
    protected override string Name
    {
        get { return "Catherine"; }
    }

    protected override string QuestId
    {
        get { return "electrical-workshop"; }
    }

    protected override DialogFrame GetInstructorDialog(string me)
    {
        return new DialogFrame(me, "Hello, I'm " + me)
        {
            Next = new DialogFrame(Name, "Hi, I'm " + Name + " the electrical instructor!")
            {
                Options = new Dictionary<string, DialogFrame>()
                {
                    {
                        "What is Electrical?", new DialogFrame(Name,
                            "Electrical engineering is a professional engineering discipline that generally deals " +
                            "with the study and application of electricity, electronics, and electromagnetism.")
                        {
                            Next = new DialogFrame(Name,
                                "Electrical engineers work in a very wide range of industries and the skills " +
                                "required are likewise variable.")
                            {
                                Next = new DialogFrame(Name,
                                    "These range from basic circuit theory to the management skills required " +
                                    "of a project manager.")
                                {
                                    Next = new DialogFrame(Name,
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
                        "Workshop", new DialogFrame(Name, "Alright let's do the workshop!")
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
}
