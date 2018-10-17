using System.Collections.Generic;
using GameDialog;
using Quests;
using UnityEngine.SceneManagement;

public class DoorNPC : NPC
{
    public string Level;

    public override Dialog GetDialog()
    {
        var me = Toolbox.Instance.GameManager.Player.Username;

        DialogFrame frame;

        if (!Toolbox.Instance.GameManager.Settings.HasBeenGreeted)
        {
            // Block the player from switching levels if they haven't been introduced to the game
            frame = new DialogFrame(me, "Maybe I should talk to Naomi first…");
        }
        else
        {
            frame = new DialogFrame(me, "This is going to take me to " + Level + ".")
            {
                Options = new Dictionary<string, DialogFrame>
                {
                    {
                        "Let's go!", new DialogFrame(me, "Let's go!")
                        {
                            TransitionToScene = Level
                        }
                    },
                    {
                        "I want to stay!", new DialogFrame(me, "On second thought…")
                    }
                }
            };
        }

        var directions = new Dictionary<string, DialogPosition>
        {
            { me, DialogPosition.Left }
        };

        return new Dialog(frame, directions);
    }

    public override void Start()
    {
        base.Start();

        // If the door is located at engineering leech, show the notification
        // This code is here because the notification canvas is per scene
        // And we want the notification to show when the user gets to leech 
        var activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "Engineering Leech")
        {
            QuestManager.Instance.MarkCurrentFinished("visit-leech");
        }
    }
}
