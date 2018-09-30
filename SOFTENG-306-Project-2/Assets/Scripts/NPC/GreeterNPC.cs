using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreeterNPC : NPC {

    // Use this for initialization
    public override Dialog GetDialog()
    {
        DialogFrame frame1 = new DialogFrame("Hi, welcome to One Day or Day One!", 1);
        DialogFrame frame2 = new DialogFrame("The point of this game is to help you gain some valuable knowledge about engineering concepts," +
            " women in engineering and other important ideas!",
            1);
        DialogFrame frame3 = new DialogFrame("Go around talk to the students around the area, they'll teach you a lot!",
            1);
        DialogFrame frame4 = new DialogFrame("Afterwards you can go downstairs into the leech, where you can play some minigames and talk to the lecturers!", 1);

        Dictionary<int, string> nameMap = new Dictionary<int, string>();

        frame1.Next = frame2;
        frame2.Next = frame3;
        frame3.Next = frame4;

        Dialog dialog = new Dialog(frame1);
        dialog.NameMap[1] = "Hermoine Granger";

        return dialog;
    }

}
