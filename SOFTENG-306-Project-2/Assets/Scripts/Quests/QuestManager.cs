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
                    if (!IsFinished(quest))
                    {
                        return quest;
                    }
                }

                return null;
            }
        }

        public bool HasWorkshop
        {
            get { return Current.Id.EndsWith("-workshop"); }
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
                Description = "The civil instructor is waiting for me at the software workshop"
            });
            quests.Add(new Quest
            {
                Id = "electrical-workshop",
                Title = "Electrical Workshop",
                Description = "The electrical instructor is waiting for me at the software workshop"
            });
            quests.Add(new Quest
            {
                Id = "networking",
                Title = "Networking Event",
                Description = "Speak to undergraduate students and industry professionals!"
            });
            quests.Add(new Quest
            {
                Id = "naomi-final",
                Title = "Speak to Naomi",
                Description = "Naomi wants to speak to you one last time"
            });
            quests.Add(new Quest
            {
                Id = "free-roam",
                Title = "You're done!",
                Description = "Feel free to visit the various stalls and chat with other students"
            });
        }

        public bool IsFinished(Quest quest)
        {
            return IsFinished(quest.Id);
        }

        public bool IsFinished(string questId)
        {
            return Completed.ContainsKey(questId) && Completed[questId];
        }

        public void MarkFinished(Quest quest)
        {
            MarkFinished(quest.Id);
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
