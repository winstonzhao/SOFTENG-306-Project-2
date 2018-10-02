using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProspectiveNPC2 : NPC {

    // Use this for initialization
    public override Dialog GetDialog()
    {
        DialogFrame frame1 = new DialogFrame("I'm Lestrange, Bellatrix Lestrange!", 1);
        DialogFrame frame2 = new DialogFrame("Nice to make your aquaintance...",
            1);
        DialogFrame frame3 = new DialogFrame("Are you enjoying the exhibition day?",
            0);
        DialogFrame frame4 = new DialogFrame("Well...", 1);
        DialogFrame frame5 = new DialogFrame(" We each have a unique toolbox comprised of life experience, daily experience, school experience, sport experience, and anything else that we know without knowing we know...", 1);
        DialogFrame frame6 = new DialogFrame("...which can empower us to improve the conditions of life we see around us, if we know how to be creative in how we approach this toolbox.", 1);

        Dictionary<int, string> nameMap = new Dictionary<int, string>();

        frame1.Next = frame2;
        frame2.Next = frame3;
        frame3.Next = frame4;
        frame4.Next = frame5;
        frame5.Next = frame6;

        Dialog dialog = new Dialog(frame1);
        dialog.NameMap[1] = "Bellatrix Lestrange";

        return dialog;
    }
}
