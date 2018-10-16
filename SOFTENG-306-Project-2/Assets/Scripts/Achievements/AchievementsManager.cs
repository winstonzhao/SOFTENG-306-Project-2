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
            List.Add(new Achievement
            {
                Id = "timetable",
                Title= "Pick Up Your Timetable!",
                Description = "You need to know where to be, don't want to miss anything related to this great day!"
            });

            List.Add(new Achievement
            {
                Id = "talk-to-student",
                Title = "Talk to a prospective student!",
                Description = "Wow, there's a lot of other girls here like me! I wonder what they think of today?"
            });

            List.Add(new Achievement
            {
                Id = "complete-workshop",
                Title = "Complete a workshop!",
                Description = "Those workshops look really interesting, I should try one out!"
            });

            List.Add(new Achievement
            {
                Id = "complete-all-workshops",
                Title = "Complete all workshops!",
                Description = "Wow, look at the variety of workshops I could try out!"
            });

            List.Add(new Achievement
            {
                Id = "find-lecturer",
                Title = "Find a lecturer!",
                Description = "I really want to know more about engineering, someone who teaches it would be the way to go!"
            });

            List.Add(new Achievement
            {
                Id = "speak-undergrad",
                Title = "Speak to an undergraduate student!",
                Description = "I wonder what being a student at the University of Auckland would be like?"
            });

            List.Add(new Achievement
            {
                Id = "master-workshop",
                Title = "Master a workshop!",
                Description = "I wonder how hard it is to pass all the levels?"
            });

            List.Add(new Achievement
            {
                Id = "master-all-workshops",
                Title = "Master all workshops!",
                Description = "I wonder how difficult it would be to pass all the levels of every game?"
            });

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
