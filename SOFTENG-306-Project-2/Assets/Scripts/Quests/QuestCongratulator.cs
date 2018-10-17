using System.Collections.Generic;
using GameDialog;
using UnityEngine;

namespace Quests
{
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

            if (QuestManager.Current.Id == trigger)
            {
                QuestManager.MarkFinished(trigger);
                ShowWorkshopCongratulatoryDialog();
            }
        }

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