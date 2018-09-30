using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergradNPC : NPC {

    // Use this for initialization
    public override Dialog GetDialog()
    {

        DialogFrame frame1 = new DialogFrame("Hi, I'm " + GameManager.instance.Player.Username, 0);
        DialogFrame frame2 = new DialogFrame("Nice to meet you!", 1);
        DialogFrame frame3 = new DialogFrame("My name's Tina Chen, and I study Software Engineering!", 1);
        DialogFrame frame4 = new DialogFrame("Hey, if you don't mind me asking?", 0);

        DialogFrame frame5 = new DialogFrame("I choose engineering because, ever since I was young I had a passion creating things" +
            "and getting my hands dirty, when it came time to pick a degree to study, engineering was only natural!", 1);

        DialogFrame frame6 = new DialogFrame("My biggest concern was that there would be no girls in engineering and I would feel... out of place," +
            "but honestly, those claims are unfounded, while sometimes there are small instances here and there, the great support system at the" +
            " university makes up for it!", 1);

        DialogFrame frame7 = new DialogFrame("My advice would be to just be yourself, the world is your oyster and you're its pearl, make the most" +
            " out of your time university, because honestly, it'll be the best time of your life!", 1);

        DialogFrame frame8 = new DialogFrame("Oooo, and by the way, you might want to head to the leech and to talk to Katherine, the software advisor!", 1);

        DialogFrame frame9 = new DialogFrame("Thanks alot! Your insight will definitely be super helpful in helping me decide whether engineering is for me!", 0);

        frame1.Next = frame2;
        frame2.Next = frame3;
        frame3.Next = frame4;

        Dictionary<string, DialogFrame> frameMap = new Dictionary<string, DialogFrame>();
        frameMap.Add("Wny Engineering?", frame5);
        frameMap.Add("What concerns did you have?", frame6);
        frameMap.Add("Any advice?", frame7);
        frame4.MakeOptionFrame(frameMap);

        frame5.Next = frame8;
        frame6.Next = frame8;
        frame7.Next = frame8;

        frame8.Next = frame9;

        Dialog dialog = new Dialog(frame1);
        dialog.NameMap[1] = "Tina Chen";
      
        return new Dialog(frame1);
    }
}
