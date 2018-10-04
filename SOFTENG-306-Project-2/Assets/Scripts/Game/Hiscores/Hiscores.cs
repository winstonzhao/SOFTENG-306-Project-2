using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace Game.Hiscores
{
    public class Hiscores : Singleton<Hiscores>
    {
        private readonly List<Score> All = new List<Score>();

        private readonly List<Score> Software = new List<Score>();

        private readonly List<Score> Civil = new List<Score>();

        private readonly List<Score> Electrical = new List<Score>();

        private string PersistentDataPath;

        private object DirtyLock = new object();

        private bool Dirty;

        public void Add(Score score)
        {
            var list = Get(score.Minigame);

            lock (this)
            {
                list.Add(score);

                All.Add(score);
            }

            lock (DirtyLock)
            {
                Dirty = true;
            }
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
            var file = PersistentDataPath + "/scores.dat";
            var data = File.ReadAllLines(file);
            foreach (var line in data)
            {
                var score = JsonUtility.FromJson<Score>(line);
                Add(score);
            }
        }

        private void Write()
        {
            string json = "";

            lock (this)
            {
                foreach (var score in All)
                {
                    json += JsonUtility.ToJson(score) + "\n";
                }
            }

            var file = PersistentDataPath + "/scores.dat";

            File.WriteAllText(file, json);
        }

        private void Awake()
        {
            PersistentDataPath = Application.persistentDataPath;

            new Thread(Read).Start();
        }

        private void Update()
        {
            // Double check pattern
            // ReSharper disable once InconsistentlySynchronizedField
            if (Dirty)
            {
                lock (DirtyLock)
                {
                    if (Dirty)
                    {
                        new Thread(Write).Start();

                        Dirty = false;
                    }
                }
            }
        }
    }
}
