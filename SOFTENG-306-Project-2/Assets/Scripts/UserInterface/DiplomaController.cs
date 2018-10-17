using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UserInterface
{
    public class DiplomaController : MonoBehaviour
    {
        private readonly Dictionary<Minigames, string> MinigameMap = new Dictionary<Minigames, string>
        {
            { Minigames.Software, "Software  Engineering" },
            { Minigames.Civil, "Civil  Engineering" },
            { Minigames.Electrical, "Electrical  Engineering" }
        };

        private Text Recommendation;

        private void Start()
        {
            var name = transform.DeepFind("Username").GetComponent<Text>();
            Recommendation = transform.DeepFind("Recommendation").GetComponent<Text>();

            name.text = Toolbox.Instance.GameManager.Player.Username;

            StartCoroutine(BigBrainArtificialIntelligence());
        }

        private IEnumerator BigBrainArtificialIntelligence()
        {
            var hiscores = Toolbox.Instance.Hiscores;

            var highestAvgScore = float.MinValue;

            var recommendation = "CompSci";

            foreach (var entry in MinigameMap)
            {
                var avgScore = 0f;
                var scores = hiscores.Get(entry.Key);

                var i = 0;

                foreach (var score in scores)
                {
                    avgScore += score.Value;

                    // Take a break every so often
                    if (++i % 20 == 0)
                    {
                        yield return null;
                    }
                }

                avgScore /= scores.Count;

                // Update the best store & give preferential treatment to software
                if (avgScore > highestAvgScore || (avgScore == highestAvgScore && entry.Key == Minigames.Software))
                {
                    highestAvgScore = avgScore;
                    recommendation = entry.Value;
                }

                yield return null;
            }

            Recommendation.text = Recommendation.text.Replace("<RECOMMENDATION>", recommendation);
        }
    }
}
