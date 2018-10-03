using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergradNPC2 : NPC {

    // Use this for initialization
    public override Dialog GetDialog()
    {

        DialogFrame frame1 = new DialogFrame("Hello, I'm " + GameManager.Instance.Player.Username, 0);
        DialogFrame frame2 = new DialogFrame("Nice to meet you!", 1);
        DialogFrame frame3 = new DialogFrame("My name's Megan, and I study Software Engineering!", 1);
        DialogFrame frame4 = new DialogFrame("Hey, if you don't mind me asking?", 0);

        DialogFrame frame5 = new DialogFrame("My choice was less traditional than most, I wasn’t sure what I wanted to do, and Engineering is such a vast industry, which has so much to offer.", 1);
        DialogFrame frame52 = new DialogFrame(" Another major consideration was that Engineering gave me structure, once I chose Engineering I only had to choose one general...", 1);
        DialogFrame frame53 = new DialogFrame("... education paper and then my next decision was my specialisation at the end of first year.", 1);
        DialogFrame frame54 = new DialogFrame("I chose Engineering because I loved problem solving and enjoyed Calculus, and Engineering would allow open many different doors for me.", 1);
        DialogFrame frame55 = new DialogFrame("Engineering allows a lot of choice, without an overwhelming vastness of options.", 1);

        DialogFrame frame6 = new DialogFrame("Honestly, I was worried that I wouldn’t be smart enough. I loved Calculus and English, but I was not a fan of Physics, so I was worried that Engineering wouldn’t be the right choice for me.", 1);
        DialogFrame frame62 = new DialogFrame(" While it’s true some specialisations require you to know Physics, but many like Software don’t require it after first year.", 1);


        DialogFrame frame7 = new DialogFrame("University is not the same as highschool, it demands more attention than you gave high school.", 1);
        DialogFrame frame72 = new DialogFrame("University gives you a lot of freedom, and it’s important that you focus on what is most important to you.", 1);
        DialogFrame frame73 = new DialogFrame(" In first year I tried to do uni, work and spend a lot of time with my friends - you can’t have it all.", 1);
        DialogFrame frame74 = new DialogFrame("But you do have a very long summer to catch up with all your friends.", 1);
        DialogFrame frame75 = new DialogFrame("Balance is important and if you are working part-time, as much as you can try and focus on your studies because at the end of the day it is more important. ", 1);

        DialogFrame frame8 = new DialogFrame("Oh wow, that's really interesting!", 0);

        DialogFrame frame9 = new DialogFrame("Thanks, no problem, come to talk me anytime!", 1);

        frame1.Next = frame2;
        frame2.Next = frame3;
        frame3.Next = frame4;

        Dictionary<string, DialogFrame> frameMap = new Dictionary<string, DialogFrame>();
        frameMap.Add("Wny Engineering?", frame5);
        frameMap.Add("Your experience?", frame6);
        frameMap.Add("Any advice?", frame7);
        frame4.MakeOptionFrame(frameMap);

        frame5.Next = frame52;
        frame52.Next = frame53;
        frame53.Next = frame54;
        frame54.Next = frame55;
        frame55.Next = frame8;

        frame6.Next = frame62;
        frame62.Next = frame8;

        frame7.Next = frame72;
        frame72.Next = frame73;
        frame73.Next = frame74;
        frame74.Next = frame75;
        frame75.Next = frame8;

        frame8.Next = frame9;

        Dialog dialog = new Dialog(frame1);
        dialog.NameMap[1] = "Megan";
      
        return dialog;
    }
}
