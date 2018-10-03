using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftwareNPC : NPC {

    // Use this for initialization
    public override Dialog GetDialog()
    {

        DialogFrame frame1 = new DialogFrame("Hello, I'm " + GameManager.Instance.Player.Username, 0);
        DialogFrame frame2 = new DialogFrame("Hi, I'm the Software Instructor!", 1);
        DialogFrame frame3 = new DialogFrame("A Bachelor of Engineering (Honours) in Software Engineering focuses on giving you the skills to engineer large, complex and fault-tolerant systems that function reliably, are effectively developed, and can be maintained efficiently.", 1);
        DialogFrame frame32 = new DialogFrame("Beyond imparting fundamental knowledge, the specialisation also prepares you for the technological environment ahead...", 1);
        DialogFrame frame33 = new DialogFrame("...so you can pursue courses and applications in areas such as human-computer interaction, serious games, smart energy consumption...", 1);
        DialogFrame frame34 = new DialogFrame("...learning aids, autonomous robots, and intelligent software agents.", 1);
        DialogFrame frame35 = new DialogFrame("Wow that's really interesting! Thanks!", 0);
        DialogFrame frame4 = new DialogFrame("Alright let's play!", 1);
        DialogFrame frame5 = new DialogFrame("Bye!", 1);

        frame3.Next = frame32;
        frame32.Next = frame33;
        frame33.Next = frame34;
        frame34.Next = frame35;


        Dictionary<string, DialogFrame> frameMap = new Dictionary<string, DialogFrame>();
        frameMap.Add("What is SOFTENG?", frame3);
        frameMap.Add("Play Software Game", frame4);
        frameMap.Add("Bye", frame5);

        frame2.MakeOptionFrame(frameMap);
        frame4.MakeTransitionFrame("SoftwareMinigameWithInstructions");

        frame1.Next = frame2;

        Dialog dialog = new Dialog(frame1);
        dialog.NameMap[1] = "Software Instructor";
      
        return dialog;
    }
}
