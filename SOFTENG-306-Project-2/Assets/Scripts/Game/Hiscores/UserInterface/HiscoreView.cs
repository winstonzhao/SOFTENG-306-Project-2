using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Hiscores.UserInterface
{
    public class HiscoreView : MonoBehaviour
    {
        private GameObject ScorePrefab;
        private GameObject NoScoresTextPrefab;

        private void Start()
        {
            ScorePrefab = Resources.Load<GameObject>("Prefabs/Hiscores/Score");
            NoScoresTextPrefab = Resources.Load<GameObject>("Prefabs/Hiscores/NoScoresText");

            var controller = FindObjectOfType<HiscoreController>();

            controller.OnMinigameChange += (s, e) => { StartCoroutine("Display", controller.Minigame); };

            StartCoroutine("Display", controller.Minigame);
        }

        private IEnumerator Display(Minigames minigame)
        {
            while (transform.childCount != 0)
            {
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }

                yield return null;
            }

            var scores = Toolbox.Instance.Hiscores.Get(minigame);

            var y = 32.0f;

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

            var rectTransform = transform.GetComponent<RectTransform>();
            var sizeDelta = rectTransform.sizeDelta;
            sizeDelta.y = y;
            rectTransform.sizeDelta = sizeDelta;
            transform.parent.GetComponent<RectTransform>().sizeDelta = sizeDelta;

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
