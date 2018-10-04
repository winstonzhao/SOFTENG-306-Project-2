using System;
using System.Collections;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace Game.Hiscores.UserInterface
{
    public class HiscoreView : MonoBehaviour
    {
        private GameObject ScorePrefab;
        private GameObject TextPrefab;

        private void Start()
        {
            ScorePrefab = Resources.Load<GameObject>("Prefabs/Hiscores/Score");
            TextPrefab = Resources.Load<GameObject>("Prefabs/Hiscores/NoScoresText");

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
                    DestroyImmediate(child.gameObject);
                }

                yield return null;
            }

            var scores = Toolbox.Instance.Hiscores.Get(minigame);

            var y = 0.0f;

            var newSize = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, scores.Count * 200);

            transform.GetComponent<RectTransform>().sizeDelta = newSize;
            transform.parent.GetComponent<RectTransform>().sizeDelta = newSize;

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

                y += rt.sizeDelta.y;

                yield return null;
            }

            if (scores.Count == 0)
            {
                var view = Instantiate(TextPrefab);
                view.transform.SetParent(transform);
                view.transform.localPosition = new Vector3(0, 0, 0);
                var rectParent = transform.parent.GetComponent<RectTransform>();
                rectParent.sizeDelta = new Vector2(rectParent.sizeDelta.x, 300);
            }


        }
    }
}
