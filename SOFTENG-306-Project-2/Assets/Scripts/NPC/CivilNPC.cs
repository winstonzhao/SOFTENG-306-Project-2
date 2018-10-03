using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilNPC : NPC {

    // Use this for initialization
    public override Dialog GetDialog()
    {

        DialogFrame frame1 = new DialogFrame("Hello, I'm " + GameManager.Instance.Player.Username, 0);
        DialogFrame frame2 = new DialogFrame("Hi, I'm the Civil Instructor!", 1);
        DialogFrame frame3 = new DialogFrame("Civil engineering is a professional engineering discipline that deals with the design, construction, and maintenance of the physical and naturally built environment, including works such as roads, bridges, canals, dams, airports, sewerage systems, pipelines, and railways.", 1);
        DialogFrame frame32 = new DialogFrame("Civil engineering is traditionally broken into a number of sub-disciplines. It is considered the second-oldest engineering discipline after military engineering,[3] and it is defined to distinguish non-military engineering from military engineering.", 1);
        DialogFrame frame33 = new DialogFrame("Civil engineering takes place in the public sector from municipal through to national governments, and in the private sector from individual homeowners through to international companies.", 1);
        DialogFrame frame34 = new DialogFrame("Wow, Civil sounds really great!", 0);
        DialogFrame frame4 = new DialogFrame("Alright let's play!", 1);
        DialogFrame frame5 = new DialogFrame("Bye!", 1);

        frame3.Next = frame32;
        frame32.Next = frame33;
        frame33.Next = frame34;

        Dictionary<string, DialogFrame> frameMap = new Dictionary<string, DialogFrame>();
        frameMap.Add("What is Civil?", frame3);
        frameMap.Add("Play Civil Game", frame4);
        frameMap.Add("Bye", frame5);

        frame2.MakeOptionFrame(frameMap);
        frame4.MakeTransitionFrame("Civil Level 1");

        frame1.Next = frame2;

        Dialog dialog = new Dialog(frame1);
        dialog.NameMap[1] = "Civil Instructor";
      
        return dialog;
    }
}
