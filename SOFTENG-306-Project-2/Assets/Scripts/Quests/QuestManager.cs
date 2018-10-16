using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Utils;

namespace Quests
{
    public class QuestManager : Singleton<QuestManager>
    {
        private const string JsonFile = "quest-log.dat";

        private readonly List<Quest> quests = new List<Quest>();
        public ReadOnlyCollection<Quest> Quests
        {
            get { return quests.AsReadOnly(); }
        }

        public Dictionary<string, bool> completed = new Dictionary<string, bool>();
        public Dictionary<string, bool> Completed
        {
            get { return completed; }
        }

        public Quest Current
        {
            get
            {
                foreach (var quest in Quests)
                {
                    if (!HasFinished(quest.Id))
                    {
                        return quest;
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
                Description = "Naomi has asked me to speak to her next to the student services stall"
            });
            quests.Add(new Quest
            {
                Id = "visit-leech",
                Title = "Visit Engineering Leech",
                Description = "I should go to the engineering leech to speak to the instructors"
            });
            quests.Add(new Quest
            {
                Id = "software-workshop",
                Title = "Software Workshop",
                Description = "The software instructor is waiting for me at the software workshop"
            });
            quests.Add(new Quest
            {
                Id = "civil-workshop",
                Title = "Civil Workshop",
                Description = "The civil instructor is waiting for me at the civil workshop"
            });
            quests.Add(new Quest
            {
                Id = "electrical-workshop",
                Title = "Electrical Workshop",
                Description = "The electrical instructor is waiting for me at the electrical workshop"
            });
            quests.Add(new Quest
            {
                Id = "post-workshops",
                Title = "Speak to Naomi",
                Description = "Head to the engineering lobby to claim your prize"
            });
            quests.Add(new Quest
            {
                Id = "networking",
                Title = "Networking Event",
                Description = "Speak to undergraduates and industry professionals"
            });
            quests.Add(new Quest
            {
                Id = "free-roam",
                Title = "You're done!",
                Description = "Feel free to revisit the stalls and chat with others"
            });
        }

        public bool HasFinishedOrIsCurrent(string questId)
        {
            return HasFinished(questId) || Current.Id == questId;
        }

        public bool HasFinished(string questId)
        {
            return Completed.ContainsKey(questId) && Completed[questId];
        }

        public void MarkFinished(string questId)
        {
            Completed[questId] = true;

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
