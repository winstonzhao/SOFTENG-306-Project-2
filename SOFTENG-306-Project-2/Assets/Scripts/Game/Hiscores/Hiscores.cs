using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Game.Hiscores
{
    public class Hiscores : Singleton<Hiscores>
    {
        private readonly List<Score> All = new List<Score>();

        private readonly List<Score> Software = new List<Score>();

        private readonly List<Score> Civil = new List<Score>();

        private readonly List<Score> Electrical = new List<Score>();

        private readonly DebounceAction DebounceSave;

        public Hiscores()
        {
            var every = TimeSpan.FromSeconds(3);
            DebounceSave = new DebounceAction(every, Save);
        }

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
