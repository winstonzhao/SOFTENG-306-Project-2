using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProspectiveNPC4 : NPC {

    // Use this for initialization
    public override Dialog GetDialog()
    {
        DialogFrame frame1 = new DialogFrame("Why hello there!", 1);
        DialogFrame frame2 = new DialogFrame("I'm Harry Potter (the chosen one)!",
            1);
        DialogFrame frame3 = new DialogFrame("The chosen what?",
            0);
        DialogFrame frame4 = new DialogFrame("...nevermind, how was your day?", 1);
        DialogFrame frame5 = new DialogFrame("It was good, I really enjoyed talking to all the other prospective students... like yourself! How about yourself?", 0);
        DialogFrame frame6 = new DialogFrame("Well, there are more opportunities in engineering than I previously realized – it’s more than Calculus and Physics, it’s practical.", 1);
        DialogFrame frame7 = new DialogFrame(" It can be a glove controlled shooter game or a smartphone gaming app, for example – engineering can be for pleasure.", 1);
        DialogFrame frame8 = new DialogFrame("This enginuity day showcased all the ‘hidden’ aspects of engineering, and promoted understanding in a helpful way that I would recommend to any students considering working in the field.", 1);
        DialogFrame frame9 = new DialogFrame(" I learnt that the future is limitless; it is not bound by gender or stereotypes.", 1);
        DialogFrame frame10 = new DialogFrame("Oh wow! Really interesting!", 0);


        Dictionary<int, string> nameMap = new Dictionary<int, string>();

        frame1.Next = frame2;
        frame2.Next = frame3;
        frame3.Next = frame4;
        frame4.Next = frame5;
        frame5.Next = frame6;
        frame6.Next = frame7;
        frame7.Next = frame8;
        frame8.Next = frame9;
        frame9.Next = frame10;

        Dialog dialog = new Dialog(frame1);
        dialog.NameMap[1] = "Harry Potter";

        return dialog;
    }
}
