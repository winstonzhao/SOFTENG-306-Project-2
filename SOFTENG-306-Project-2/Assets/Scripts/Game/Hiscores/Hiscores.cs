using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Game.Hiscores
{
    /// <summary>
    /// Stores the scores for the user
    /// </summary>
    public class Hiscores : Singleton<Hiscores>
    {
        /// <summary>
        /// Stores all of the scores, regardless of the minigame.
        ///
        /// This is used for serialization
        /// </summary>
        private readonly List<Score> All = new List<Score>();

        private readonly List<Score> Software = new List<Score>();

        private readonly List<Score> Civil = new List<Score>();

        private readonly List<Score> Electrical = new List<Score>();

        /// <summary>
        /// Debounce the save to avoid writing to disk too frequently
        /// </summary>
        private readonly DebounceAction DebounceSave;

        public Hiscores()
        {
            var every = TimeSpan.FromSeconds(3);
            DebounceSave = new DebounceAction(every, Save);
        }

        /// <summary>
        /// Add the given <paramref name="score"/>
        /// </summary>
        /// <param name="score">the score to add</param>
        public void Add(Score score)
        {
            var list = Get(score.Minigame);

            lock (this)
            {
                list.Add(score);

                All.Add(score);
            }

            DebounceSave.Run();
        }

        /// <summary>
        /// Get all the scores for the given <paramref name="minigame"/>
        /// </summary>
        /// <param name="minigame">the minigame to get all the scores for</param>
        /// <returns>the list of scores for that minigame</returns>
        /// <exception cref="ArgumentOutOfRangeException">if the minigame does not support scores</exception>
        public List<Score> Get(Minigames minigame)
        {
            switch (minigame)
            {
                case Minigames.Software:
                    return Software;
                case Minigames.Civil:
                    return Civil;
                case Minigames.Electrical:
                    return Electrical;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Read()
        {
            var values = Toolbox.Instance.JsonFiles.ReadList<Score>("scores.dat");

            if (values == null)
            {
                return;
            }

            lock (this)
            {
                All.AddRange(values);
                // Add to each section
                foreach (var score in values)
                {
                    Get(score.Minigame).Add(score);
                }
            }
        }

        /// <summary>
        /// Do not invoke directly unless required - use <see cref="DebounceSave"/>
        /// </summary>
        private void Save()
        {
            Toolbox.Instance.JsonFiles.WriteList("scores.dat", All);
        }

        private void Awake()
        {
            Read();
        }
    }
}
