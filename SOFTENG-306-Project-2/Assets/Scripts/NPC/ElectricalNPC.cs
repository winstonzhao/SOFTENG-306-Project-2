using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalNPC : NPC {

    // Use this for initialization
    public override Dialog GetDialog()
    {

        DialogFrame frame1 = new DialogFrame("Hello, I'm " + GameManager.Instance.Player.Username, 0);
        DialogFrame frame2 = new DialogFrame("Hi, I'm the Electrical Instructor!", 1);
        DialogFrame frame3 = new DialogFrame("Electrical engineering is a professional engineering discipline that generally deals with the study and application of electricity, electronics, and electromagnetism.", 1);
        DialogFrame frame32 = new DialogFrame("Electrical engineers work in a very wide range of industries and the skills required are likewise variable.", 1);
        DialogFrame frame33 = new DialogFrame("These range from basic circuit theory to the management skills required of a project manager. ", 1);
        DialogFrame frame34 = new DialogFrame("The tools and equipment that an individual engineer may need are similarly variable, ranging from a simple voltmeter to a top end analyzer to sophisticated design and manufacturing software", 1);
        DialogFrame frame35 = new DialogFrame("Wow! That's really cool!", 0);
        DialogFrame frame4 = new DialogFrame("Alright let's play!", 1);
        DialogFrame frame5 = new DialogFrame("Bye!", 1);

        frame3.Next = frame32;
        frame32.Next = frame33;
        frame33.Next = frame34;
        frame34.Next = frame35;

        Dictionary<string, DialogFrame> frameMap = new Dictionary<string, DialogFrame>();
        frameMap.Add("What is ElECTENG?", frame3);
        frameMap.Add("Play Electrical Game", frame4);
        frameMap.Add("Bye", frame5);

        frame2.MakeOptionFrame(frameMap);
        frame4.MakeTransitionFrame("Welcome Screen");

        frame1.Next = frame2;

        Dialog dialog = new Dialog(frame1);
        dialog.NameMap[1] = "Electrical Instructor";
      
        return dialog;
    }
}
