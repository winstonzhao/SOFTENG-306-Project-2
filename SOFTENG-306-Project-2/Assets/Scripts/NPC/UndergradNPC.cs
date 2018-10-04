using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergradNPC : NPC {

    // Use this for initialization
    public override Dialog GetDialog()
    {

        DialogFrame frame1 = new DialogFrame("Hi, I'm " + Toolbox.Instance.GameManager.Player.Username, 0);
        DialogFrame frame2 = new DialogFrame("Nice to meet you!", 1);
        DialogFrame frame3 = new DialogFrame("My name's Tina Chen, and I study Software Engineering!", 1);
        DialogFrame frame4 = new DialogFrame("Hey, if you don't mind me asking?", 0);

        DialogFrame frame5 = new DialogFrame("I chose engineering as I enjoyed design and science at school.", 1);
        DialogFrame frame52 = new DialogFrame("I had researched engineering and found that it offered a good combination of both theoretical and " +
            "practical skills that would be useful for many career paths. ", 1);

        DialogFrame frame6 = new DialogFrame("I was nervous about the programming paper that is taken by all engineering students in their first year.", 1);
        DialogFrame frame62 = new DialogFrame("I had never done any coding before and was worried that I may fall behind the other students.", 1);
        DialogFrame frame63 = new DialogFrame("However, the class ended up being a very good introduction to the basics of programming, and I " +
            "enjoyed it a lot and chose to continue in software as my specialisation.", 1);

        DialogFrame frame7 = new DialogFrame("I would encourage everyone to stay open-minded and give everything a try. There are many clubs and events" +
            " that university offers, both within and outside of engineering.", 1);
        DialogFrame frame72 = new DialogFrame("They are a great place to meet new people and make friends.", 1);
        DialogFrame frame73 = new DialogFrame("Engineering especially is a degree that focuses a lot on teamwork and collaboration, so having a good group" +
            " of friends is really important.", 1);

        DialogFrame frame8 = new DialogFrame("Oooo, and by the way, you might want to head to the leech and to talk to Katherine, the software advisor!", 1);

        DialogFrame frame9 = new DialogFrame("Thanks alot! Your insight will definitely be super helpful in helping me decide whether engineering is for me!", 0);

        frame1.Next = frame2;
        frame2.Next = frame3;
        frame3.Next = frame4;

        Dictionary<string, DialogFrame> frameMap = new Dictionary<string, DialogFrame>();
        frameMap.Add("Wny Engineering?", frame5);
        frameMap.Add("Your experience?", frame6);
        frameMap.Add("Any advice?", frame7);
        frame4.MakeOptionFrame(frameMap);

        frame5.Next = frame52;
        frame52.Next = frame8;

        frame6.Next = frame62;
        frame62.Next = frame63;
        frame63.Next = frame8;

        frame7.Next = frame72;
        frame72.Next = frame73;
        frame73.Next = frame8;

        frame8.Next = frame9;

        Dialog dialog = new Dialog(frame1);
        dialog.NameMap[1] = "Tina Chen";
      
        return dialog;
    }
}
