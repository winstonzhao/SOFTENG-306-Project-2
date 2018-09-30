using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorNPC : NPC {

    public string Level;

    public override Dialog GetDialog()
    {

        DialogFrame frame1 = new DialogFrame("This is going to take me to " + Level + ".", 0);
        DialogFrame frame2 = new DialogFrame("Let's go!", 0);
        DialogFrame frame3 = new DialogFrame("On second thought....", 0);
      
        Dictionary<string, DialogFrame> frameMap = new Dictionary<string, DialogFrame>();
        frameMap.Add("Let's go!", frame2);
        frameMap.Add("I want to stay!", frame3);
        frame2.MakeTransitionFrame(Level);

        frame1.MakeOptionFrame(frameMap);

        return new Dialog(frame1);
    }
}
