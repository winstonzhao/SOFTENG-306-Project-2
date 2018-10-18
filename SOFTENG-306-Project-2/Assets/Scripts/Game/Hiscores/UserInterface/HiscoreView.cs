using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Hiscores.UserInterface
{
    /// <summary>
    /// Grabs and shows all of the scores for the currently selected minigame
    /// </summary>
    public class HiscoreView : MonoBehaviour
    {
        private GameObject ScorePrefab;
        private GameObject NoScoresTextPrefab;

        private void Start()
        {
            ScorePrefab = Resources.Load<GameObject>("Prefabs/Hiscores/Score");
            NoScoresTextPrefab = Resources.Load<GameObject>("Prefabs/Hiscores/NoScoresText");

            var controller = FindObjectOfType<HiscoreController>();

            // Listen for changes in the selected minigame tab and then display the new scores
            controller.OnMinigameChange += (s, e) => { StartCoroutine(Display(controller.Minigame)); };

            // Display the scores for the first time
            StartCoroutine(Display(controller.Minigame));
        }

        /// <summary>
        /// Display the scores for the given <paramref name="minigame"/>
        /// </summary>
        /// <param name="minigame">the minigame to show scores for</param>
        /// <returns>Co-routine to run</returns>
        private IEnumerator Display(Minigames minigame)
        {
            // Clear any existing children in the view
            while (transform.childCount != 0)
            {
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }

                yield return null;
            }

            var scores = Toolbox.Instance.Hiscores.Get(minigame);

            // Keep a track of the y position to place the new score - the initial value is the top padding
            var y = 32.0f;

            // Add each of the scores to the view
            foreach (var score in scores)
            {
                var view = Instantiate(ScorePrefab);

                view.transform.Find("Name").GetComponent<Text>().text = score.Minigame.ToString();
                view.transform.Find("Date").GetComponent<Text>().text = score.CreatedAt.ToShortDateString();

                var scoreText = view.transform.Find("Score").GetComponent<Text>();
                scoreText.text = score.Value < 1 ? "Completed" : "Scored " + score.Value;

                view.transform.SetParent(transform);

                var rt = view.GetComponent<RectTransform>();

                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, y, rt.sizeDelta.y);

                var offsetMax = rt.offsetMax;
                offsetMax.x = 0;
                rt.offsetMax = offsetMax;

                var offsetMin = rt.offsetMin;
                offsetMin.x = 0;
                rt.offsetMin = offsetMin;

                y += rt.sizeDelta.y;

                yield return null;
            }

            // Layout the parent & scroll view
            var rectTransform = transform.GetComponent<RectTransform>();
            var sizeDelta = rectTransform.sizeDelta;
            sizeDelta.y = y;
            rectTransform.sizeDelta = sizeDelta;
            transform.parent.GetComponent<RectTransform>().sizeDelta = sizeDelta;

            // If there were no scores found then display the empty view - tell the user there are no scores
            if (scores.Count == 0)
            {
                var view = Instantiate(NoScoresTextPrefab);
                view.transform.SetParent(transform);
                view.transform.localPosition = new Vector3(0, 25, 0);
                var rectParent = transform.parent.GetComponent<RectTransform>();
                rectParent.sizeDelta = new Vector2(rectParent.sizeDelta.x, 300);
            }
        }
    }
}
