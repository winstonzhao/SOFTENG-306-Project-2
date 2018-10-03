using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergradNPC4 : NPC {

    // Use this for initialization
    public override Dialog GetDialog()
    {

        DialogFrame frame1 = new DialogFrame("Hello, I'm " + GameManager.Instance.Player.Username, 0);
        DialogFrame frame2 = new DialogFrame("Nice to meet you!", 1);
        DialogFrame frame3 = new DialogFrame("My name's Susanne, and I study CHEMMAT!", 1);
        DialogFrame frame4 = new DialogFrame("Hey, if you don't mind me asking?", 0);

        DialogFrame frame5 = new DialogFrame("I really liked the idea of helping other people and making a change in the world. I think that engineering shows so much growth and potential and I really wished to be a part of that. ", 1);
        DialogFrame frame52 = new DialogFrame("I had talked to a few female students beforehand and they also really enjoyed the experience and that inspired me to take this course", 1);

        DialogFrame frame6 = new DialogFrame("I was really worried that perhaps my skills weren't satisfactory enough to be able to show my full potential as an engineer.", 1);
        DialogFrame frame62 = new DialogFrame("However, I ended up making good friends who I have established a study group with in which us as a group can grow together and inspire one another.", 1);


        DialogFrame frame7 = new DialogFrame("I would advise younger students to go out and talk to those who are taking the course now.", 1);
        DialogFrame frame72 = new DialogFrame(" I'm sure that it would be inspiring to them and would help them decide on where their future paths might lead them.", 1);
        DialogFrame frame73 = new DialogFrame(" I would also advise the younger female students to get involved with WEN and see the future of engineering for females.", 1);
 
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
        frame52.Next = frame8;

        frame6.Next = frame62;
        frame62.Next = frame8;

        frame7.Next = frame72;
        frame72.Next = frame73;
        frame73.Next = frame8;

        frame8.Next = frame9;

        Dialog dialog = new Dialog(frame1);
        dialog.NameMap[1] = "Susanne";
      
        return dialog;
    }
}
