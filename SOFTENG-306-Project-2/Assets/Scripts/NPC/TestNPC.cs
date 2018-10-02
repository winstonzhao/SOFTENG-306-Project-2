using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNPC : NPC {



    // Use this for initialization
    public override Dialog GetDialog() { 

    DialogFrame frame1 = new DialogFrame("Hello, my name is Jeff.", 0);
        DialogFrame frame2 = new DialogFrame("Well, that's great isn't it?", 1);
        DialogFrame frame3 = new DialogFrame("Why is it always raining in Auckland?", 0);
        DialogFrame frame4 = new DialogFrame("That's due to the city having a large amount of dust particles in the air, " +
                        "those particles act as seeds for the clouds that create precipitation!",
            1);
        DialogFrame frame5 = new DialogFrame("In fact, that's why rain is the city is often more sporadic than in the suburbs!",
            1);
        DialogFrame frame6 = new DialogFrame("Ooooooo, I see, that's super interesting!", 0);

        DialogFrame buttonFrame = new DialogFrame("How are you a good Student?", 1);
        Dictionary<string, DialogFrame> frameMap = new Dictionary<string, DialogFrame>();
        frameMap.Add("Option One", frame4);
        frameMap.Add("Option Two", frame5);
        frameMap.Add("Option Three", frame6);
        frame6.MakeTransitionFrame("Menu");

        buttonFrame.MakeOptionFrame(frameMap);


        frame1.Next = frame2;
        frame2.Next = frame3;
        frame3.Next = buttonFrame;

        return new Dialog(frame1);
    }
}
