using System.Collections.Generic;
using GameDialog;
using UnityEngine;

namespace Quests
{
    /// <summary>
    /// Detects when the user has finished the workshops to handle achievemenets & dialog after the user has
    /// finished all workshops i.e. tell them to come upstairs to the networking event
    /// </summary>
    public class QuestCongratulator : MonoBehaviour
    {
        public Sprite NpcSprite;

        private QuestManager QuestManager;

        private void Start()
        {
            QuestManager = Toolbox.Instance.QuestManager;
        }

        private void Update()
        {
            const string trigger = "post-workshops-trigger";

            switch (QuestManager.Current.Id)
            {
                case "software-workshop":
                    Toolbox.Instance.AchievementsManager.MarkCompleted("complete-workshop");
                    break;
                case trigger:
                    // Auto complete the quest
                    Toolbox.Instance.AchievementsManager.MarkCompleted("complete-all-workshops");
                    QuestManager.MarkFinished(trigger);
                    ShowWorkshopCongratulatoryDialog();
                    break;
            }
        }

        /// <summary>
        /// Show dialog to tell the user to come upstairs for the networking event
        /// </summary>
        private void ShowWorkshopCongratulatoryDialog()
        {
            var me = Toolbox.Instance.GameManager.Player.Username;

            const string npc = "Naomi";

            var frame = new DialogFrame(npc,
                "Nice one " + me +
                ", you’ve completed all your workshops today! Let’s head to the lobby for the networking event");

            var directions = new Dictionary<string, DialogPosition>
            {
                { npc, DialogPosition.Right }
            };

            var canvas = FindObjectOfType<DialogCanvasManager>();

            var dialog = new Dialog(frame, directions);
            canvas.ShowDialog(dialog, null, NpcSprite);
        }
    }
}
