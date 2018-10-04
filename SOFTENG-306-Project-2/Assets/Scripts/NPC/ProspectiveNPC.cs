using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProspectiveNPC : NPC {

    // Use this for initialization
    public override Dialog GetDialog()
    {
        DialogFrame frame1 = new DialogFrame("Hey, I'm Cho Chang, today is so exiting!", 1);
        DialogFrame frame2 = new DialogFrame("I've learnt so many interesting things!",
            1);
        DialogFrame frame3 = new DialogFrame("This day really exposed a variety of things you can do with an engineering degree and the places it can take you.",
            1);
        DialogFrame frame4 = new DialogFrame("Talking to many people who have been through the same process...", 1);
        DialogFrame frame5 = new DialogFrame(" ...you’re currently wading through was incredibly helpful as they hold the wisdom of hindsight, thus knowing how the degree works in its entirety.", 1);
        DialogFrame frame6 = new DialogFrame("Meeting students from all over the country was also a wonderful experience, a room full of intelligent young women brimming with potential...", 1);
        DialogFrame frame7 = new DialogFrame("...and all glad to be given the chance to explore the option of engineering in this busy day.", 1);

        Dictionary<int, string> nameMap = new Dictionary<int, string>();

        frame1.Next = frame2;
        frame2.Next = frame3;
        frame3.Next = frame4;
        frame4.Next = frame5;
        frame5.Next = frame6;
        frame6.Next = frame7;

        Dialog dialog = new Dialog(frame1);
        dialog.NameMap[1] = "Cho Chang";

        return dialog;
    }
}
