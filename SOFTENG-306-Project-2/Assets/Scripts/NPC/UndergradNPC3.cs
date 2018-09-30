using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergradNPC3 : NPC {

    // Use this for initialization
    public override Dialog GetDialog()
    {

        DialogFrame frame1 = new DialogFrame("Hello, I'm " + GameManager.instance.Player.Username, 0);
        DialogFrame frame2 = new DialogFrame("Nice to meet you!", 1);
        DialogFrame frame3 = new DialogFrame("My name's Saachi, and I study Mechantronics!", 1);
        DialogFrame frame4 = new DialogFrame("Hey, if you don't mind me asking?", 0);

        DialogFrame frame5 = new DialogFrame("The biggest concern I had about engineering wasn’t about the actual coursework, but about how I would be able to make friends if none of my school friends took my specialisation.", 1);
        DialogFrame frame52 = new DialogFrame(" Things like not having a lab partner, project partner, groups for teams etc.", 1);
        DialogFrame frame53 = new DialogFrame("Turns out that once you get into your specialisation, you spend a lot more time with them in labs, classes, tutorials etc and you naturally get to know them better.", 1);
        DialogFrame frame54 = new DialogFrame("Everyone is also really friendly and willing to help out, so there’s no need to worry about that after all!", 1);

        DialogFrame frame6 = new DialogFrame("I enjoyed both maths and physics at school, and also really enjoyed the design subjects that I had creative freedom with.", 1);
        DialogFrame frame62 = new DialogFrame("Engineering, in particular mechanical/mechatronics combines these 2 aspects well.", 1);


        DialogFrame frame7 = new DialogFrame("My advice is to make the most of the facilities and engineering opportunities offered by the faculty.", 1);
        DialogFrame frame72 = new DialogFrame("There’s heaps of events, workshops, meetups, information evenings and people to answer any questions you have.", 1);
        DialogFrame frame73 = new DialogFrame("WEN is also another great community which offers exclusive events for girls that build on your skills and also allows a forum to make great new friends", 1);

        DialogFrame frame8 = new DialogFrame("Oh wow, that's really interesting!", 0);

        DialogFrame frame9 = new DialogFrame("Thanks, no problem, come to talk me anytime!", 1);

        frame1.Next = frame2;
        frame2.Next = frame3;
        frame3.Next = frame4;

        Dictionary<string, DialogFrame> frameMap = new Dictionary<string, DialogFrame>();
        frameMap.Add("Wny Engineering?", frame6);
        frameMap.Add("Your experience?", frame5);
        frameMap.Add("Any advice?", frame7);
        frame4.MakeOptionFrame(frameMap);

        frame5.Next = frame52;
        frame52.Next = frame53;
        frame53.Next = frame54;
        frame54.Next = frame8;

        frame6.Next = frame62;
        frame62.Next = frame8;

        frame7.Next = frame72;
        frame72.Next = frame73;
        frame73.Next = frame8;

        frame8.Next = frame9;

        Dialog dialog = new Dialog(frame1);
        dialog.NameMap[1] = "Saachi";
      
        return dialog;
    }
}
