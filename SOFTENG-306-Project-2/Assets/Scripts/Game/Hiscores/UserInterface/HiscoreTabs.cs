using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace Game.Hiscores.UserInterface
{
    /// <summary>
    /// Component to programatically add all of our games to the hiscore view
    ///
    /// This uses the <see cref="Minigames"/> enum to figure out what our minigames are. 
    /// </summary>
    public class HiscoreTabs : MonoBehaviour
    {
        private HiscoreController HiscoreController;

        [SerializeField]
        private Font Font;

        [SerializeField]
        private Color TextColor = Color.black;

        private void Start()
        {
            HiscoreController = FindObjectOfType<HiscoreController>();

            StartCoroutine(Layout());
        }

        /// <summary>
        /// Programatically add all of the tabs
        /// </summary>
        /// <returns>Co-routine to run</returns>
        private IEnumerator Layout()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Hiscores/Tab");

            var minigames = EnumUtils.GetValues<Minigames>();

            var hPadding = 24.0f;

            var x = hPadding * 2;

            var parentHeight = GetComponent<RectTransform>().sizeDelta.y;

            var i = 0;

            foreach (var minigame in minigames)
            {
                var tab = Instantiate(prefab);
                tab.transform.SetParent(transform);
                var button = tab.GetComponent<Button>();
                var rt = tab.GetComponent<RectTransform>();
                var text = tab.transform.GetChild(0).GetComponent<Text>();

                text.text = ToText(minigame);

                // Base the button size on the text
                var width = text.preferredWidth + 32.0f;
                var height = text.preferredHeight + 24.0f;

                // Draw in center of parent
                var y = parentHeight / 2 - height / 2;

                // Set the location of the button
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, x, width);
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, y, height);

                // Move next button to the right
                x += width + hPadding;

                var finalMinigame = minigame;
                button.onClick.AddListener(() => { ChooseTab(finalMinigame); });

                i++;

                yield return null;
            }
        }

        /// <summary>
        /// Converts an enum to its string equivalent e.g. Minigames.Civil -> "Civil" 
        /// </summary>
        /// <param name="minigame">the minigame to get a string representation of</param>
        /// <returns>the text representation of that <paramref name="minigame"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">if the minigame cannot be mapped to a string</exception>
        private string ToText(Minigames minigame)
        {
            switch (minigame)
            {
                case Minigames.Software:
                    return "Software";
                case Minigames.Civil:
                    return "Civil";
                case Minigames.Electrical:
                    return "Electrical";
                default:
                    throw new ArgumentOutOfRangeException("minigame", minigame, null);
            }
        }

        /// <summary>
        /// Requests the view to switch tab to the provided <paramref name="minigame"/>
        /// </summary>
        /// <param name="minigame">the minigame to show hiscores for</param>
        private void ChooseTab(Minigames minigame)
        {
            HiscoreController.Minigame = minigame;
        }
    }
}
