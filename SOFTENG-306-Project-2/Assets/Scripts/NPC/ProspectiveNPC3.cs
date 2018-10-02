using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProspectiveNPC3 : NPC {

    // Use this for initialization
    public override Dialog GetDialog()
    {
        DialogFrame frame1 = new DialogFrame("Helloooooooo!", 1);
        DialogFrame frame2 = new DialogFrame("I'm Minerva!",
            1);
        DialogFrame frame3 = new DialogFrame("I'm really enjoying today!",
            1);
        DialogFrame frame4 = new DialogFrame("One of the more striking comments from the student panel was, 'don’t hesitate in taking opportunities.'", 1);
        DialogFrame frame5 = new DialogFrame("So whether you have any doubts, or if you are dead set on engineering, you may find yourself pleasantly surprised", 1);
        DialogFrame frame6 = new DialogFrame(" as I have. It was because of this event that I decided to shed my perpetual state of ambiguity with my career choice, and set my heart on becoming an engineer.", 1);

        Dictionary<int, string> nameMap = new Dictionary<int, string>();

        frame1.Next = frame2;
        frame2.Next = frame3;
        frame3.Next = frame4;
        frame4.Next = frame5;
        frame5.Next = frame6;

        Dialog dialog = new Dialog(frame1);
        dialog.NameMap[1] = "Minerva McGonagall";

        return dialog;
    }
}
