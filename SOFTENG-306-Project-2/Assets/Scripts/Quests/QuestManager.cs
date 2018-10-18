using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Utils;

namespace Quests
{
    /// <summary>
    /// Quest manager - stores the available quests & whether the user has completed them
    /// </summary>
    public class QuestManager : Singleton<QuestManager>
    {
        private const string JsonFile = "quest-log.dat";

        private readonly List<Quest> quests = new List<Quest>();
        public ReadOnlyCollection<Quest> Quests
        {
            get { return quests.AsReadOnly(); }
        }

        private Dictionary<string, bool> completed = new Dictionary<string, bool>();
        public Dictionary<string, bool> Completed
        {
            get { return completed; }
        }

        private Quest current;
        /// <summary>
        /// Get the current quest the user has yet to complete
        /// </summary>
        public Quest Current
        {
            get
            {
                if (current != null)
                {
                    return current;
                }

                foreach (var quest in Quests)
                {
                    if (!HasFinished(quest.Id))
                    {
                        return current = quest;
                    }
                }

                return null;
            }
        }

        private readonly DebounceAction DebounceSave;

        public QuestManager()
        {
            var every = TimeSpan.FromSeconds(3);
            DebounceSave = new DebounceAction(every, Save);

            quests.Add(new Quest
            {
                Id = "get-timetable",
                Title = "Speak to Naomi",
                Description = "Naomi has asked me to speak to her next to the student services stall",
                NotificationText = "Open your backpack to view your timetable"
            });
            quests.Add(new Quest
            {
                Id = "visit-leech",
                Title = "Visit Engineering Leech",
                Description = "I should go to the engineering leech to speak to the instructors",
                NotificationText = "You made it to Engineering Leech!"
            });
            quests.Add(new Quest
            {
                Id = "software-workshop",
                Title = "Software Workshop",
                Description = "The software instructor is waiting for me at the software workshop",
                NotificationText = "You finished the Software workshop!"
            });
            quests.Add(new Quest
            {
                Id = "electrical-workshop",
                Title = "Electrical Workshop",
                Description = "The electrical instructor is waiting for me at the electrical workshop",
                NotificationText = "You finished the Electrical workshop!"
            });
            quests.Add(new Quest
            {
                Id = "civil-workshop",
                Title = "Civil Workshop",
                Description = "The civil instructor is waiting for me at the civil workshop",
                NotificationText = "You finished the Civil workshop!"
            });
            quests.Add(new Quest
            {
                Id = "post-workshops-trigger"
            });
            quests.Add(new Quest
            {
                Id = "networking",
                Title = "Networking Event",
                Description = "Speak to undergraduates and industry professionals",
                NotificationText = "Speak to Naomi for a prize, or keep talking to other students"
            });
            quests.Add(new Quest
            {
                Id = "collect-prize",
                Title = "Speak to Naomi",
                Description = "Head to the engineering lobby to claim your prize",
                NotificationText = "Check your backpack for your prize!"
            });
            quests.Add(new Quest
            {
                Id = "free-roam",
                Title = "You're done!",
                Description = "Feel free to revisit the stalls and chat with others"
            });
        }

        /// <summary>
        /// Ask whether a quest has been finished or if it is the current quest
        /// </summary>
        /// <param name="questId">the quest to check</param>
        /// <returns>whether it has been finished or if it is the current quest</returns>
        public bool HasFinishedOrIsCurrent(string questId)
        {
            return HasFinished(questId) || Current.Id == questId;
        }

        /// <summary>
        /// Ask whether a quest has been finished
        /// </summary>
        /// <param name="questId">the quest to check</param>
        /// <returns>whether it has been finished</returns>
        public bool HasFinished(string questId)
        {
            return Completed.ContainsKey(questId) && Completed[questId];
        }

        /// <summary>
        /// Marks the quest as finished if the current quest matches the given <paramref name="questId"/>
        /// </summary>
        /// <param name="questId"></param>
        public void MarkCurrentFinished(string questId)
        {
            if (Current.Id == questId)
            {
                MarkFinished(questId);
            }
        }

        /// <summary>
        /// Mark the quest as finished
        /// </summary>
        /// <param name="questId">the quest to mark as completed</param>
        public void MarkFinished(string questId)
        {
            // Do nothing if already marked as finished
            if (Completed.ContainsKey(questId) && Completed[questId])
            {
                return;
            }

            foreach (var quest in Quests)
            {
                if (quest.Id == questId)
                {
                    if (string.IsNullOrEmpty(quest.Title))
                    {
                        break;
                    }

                    Toolbox.Instance.Notifications.Show(quest.NotificationText);
                }
            }

            Completed[questId] = true;

            // Clear the memoized value if it's no longer the current quest 
            if (current != null && current.Id == questId)
            {
                current = null;
            }

            DebounceSave.Run();
        }

        private void Awake()
        {
            Load();
        }

        private void Load()
        {
            completed = Toolbox.Instance.JsonFiles.ReadDictionary<string, bool>(JsonFile) ?? completed;
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
