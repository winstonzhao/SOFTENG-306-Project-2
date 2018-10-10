using System.Collections.Generic;
using GameDialog;

public class GreeterNPC : NPC
{
    // Use this for initialization
    public override Dialog GetDialog()
    {
        const string npc = "Hermoine Granger";

        var frame = new DialogFrame(npc, "Hi, welcome to One Day or Day One!")
        {
            Next = new DialogFrame(npc,
                "The point of this game is to help you gain some valuable knowledge about engineering concepts," +
                " women in engineering and other important ideas!")
            {
                Next = new DialogFrame(npc,
                    "Go around talk to the students around the area, they'll teach you a lot!")
                {
                    Next = new DialogFrame(npc,
                        "Afterwards you can go downstairs into the leech, where you can play some minigames and " +
                        "talk to the lecturers!")
                }
            }
        };

        var directions = new Dictionary<string, DialogPosition>
        {
            { npc, DialogPosition.Right }
        };

        return new Dialog(frame, directions);
    }
}
