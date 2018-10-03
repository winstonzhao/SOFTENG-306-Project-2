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

        private void Start()
        {
            ScorePrefab = Resources.Load<GameObject>("Prefabs/Hiscores/Score");

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
        }
    }
}
