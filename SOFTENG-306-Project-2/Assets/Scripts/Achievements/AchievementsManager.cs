using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Utils;

namespace Achievements
{
    [Serializable]
    public class AchievementsManager : Singleton<AchievementsManager>
    {
        private const string JsonFile = "achievement-log.dat";

        private List<Achievement> List = new List<Achievement>();
        public ReadOnlyCollection<Achievement> All
        {
            get { return List.AsReadOnly(); }
        }

        [SerializeField]
        private Dictionary<string, bool> Completed;

        private readonly DebounceAction DebounceSave;

        public AchievementsManager()
        {
            var every = TimeSpan.FromSeconds(3);
            DebounceSave = new DebounceAction(every, Save);

            // Add achievements here

            // Load state after adding achievements
            var achievements = Load();
            Completed = achievements != null ? achievements.Completed : new Dictionary<string, bool>();
        }

        public void MarkCompleted(Achievement achievement, bool completed = true)
        {
            MarkCompleted(achievement.Id, completed);
        }

        public void MarkCompleted(string achievementId, bool completed = true)
        {
            Completed[achievementId] = completed;

            DebounceSave.Run();
        }

        public bool IsCompleted(Achievement achievement)
        {
            return IsCompleted(achievement.Id);
        }

        public bool IsCompleted(string achievementId)
        {
            return Completed[achievementId];
        }

        private AchievementsManager Load()
        {
            return Toolbox.Instance.JsonFiles.Read<AchievementsManager>(JsonFile);
        }

        /// <summary>
        /// Do not invoke directly unless required - use <see cref="DebounceSave"/>
        /// </summary>
        private void Save()
        {
            Toolbox.Instance.JsonFiles.Write(JsonFile, this);
        }
    }
}
