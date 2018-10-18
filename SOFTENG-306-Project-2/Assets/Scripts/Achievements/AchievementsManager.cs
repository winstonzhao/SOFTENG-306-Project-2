using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Utils;

namespace Achievements
{
    /// <summary>
    /// Manages the achievements the user has gotten & which achievements are available
    /// </summary>
    public class AchievementsManager : Singleton<AchievementsManager>
    {
        /// <summary>
        /// The file to which the data is persisted to
        /// </summary>
        private const string JsonFile = "achievement-log.dat";

        private readonly List<Achievement> List = new List<Achievement>();
        /// <summary>
        /// Read only version of all of the available achievements
        /// </summary>
        public ReadOnlyCollection<Achievement> All
        {
            get { return List.AsReadOnly(); }
        }

        /// <summary>
        /// Whether an achievement (checked by id) has been completed by the user
        /// </summary>
        private Dictionary<string, bool> Completed = new Dictionary<string, bool>();

        private readonly DebounceAction DebounceSave;

        public AchievementsManager()
        {
            var every = TimeSpan.FromSeconds(3);
            DebounceSave = new DebounceAction(every, Save);

            // Add achievements here
            List.Add(new Achievement
            {
                Id = "timetable",
                Title = "Pick Up Your Timetable!",
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
                Description =
                    "I really want to know more about engineering, someone who teaches it would be the way to go!"
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
            Load();
        }

        /// <summary>
        /// <see cref="MarkCompleted(string,bool)"/>
        /// </summary>
        /// <param name="achievement">achievement to complete</param>
        /// <param name="completed">whether it should be completed</param>
        public void MarkCompleted(Achievement achievement, bool completed = true)
        {
            MarkCompleted(achievement.Id, completed);
        }

        /// <summary>
        /// Marks an achievement as completed or not
        /// </summary>
        /// <param name="achievementId">achievement to complete</param>
        /// <param name="completed">whether it should be completed</param>
        public void MarkCompleted(string achievementId, bool completed = true)
        {
            Completed[achievementId] = completed;

            // Special case for when user has completed/mastered all 3 workshops
            if (achievementId.EndsWith("-workshop") && !achievementId.StartsWith("master-"))
            {
                // Give the man his skill cape if he's finished every workshop
                if (IsCompleted("software-master-workshop")
                    && IsCompleted("civil-master-workshop")
                    && IsCompleted("electrical-master-workshop"))
                {
                    MarkCompleted("master-workshop");
                }
            }

            DebounceSave.Run();
        }

        /// <summary>
        /// <see cref="IsCompleted(string)"/>
        /// </summary>
        /// <param name="achievement">the achievement to check</param>
        /// <returns>whether the user has completed the <paramref name="achievement"/></returns>
        public bool IsCompleted(Achievement achievement)
        {
            return IsCompleted(achievement.Id);
        }

        /// <summary>
        /// Check whether an achievement has been completed
        /// </summary>
        /// <param name="achievement">the achievement to check</param>
        /// <returns>whether the user has completed the <paramref name="achievement"/></returns>
        public bool IsCompleted(string achievementId)
        {
            return Completed.ContainsKey(achievementId) && Completed[achievementId];
        }

        private void Load()
        {
            Completed = Toolbox.Instance.JsonFiles.ReadDictionary<string, bool>(JsonFile) ?? Completed;
        }

        /// <summary>
        /// Do not invoke directly unless required - use <see cref="DebounceSave"/>
        /// </summary>
        private void Save()
        {
            Toolbox.Instance.JsonFiles.WriteDictionary(JsonFile, Completed);
        }
    }
}
