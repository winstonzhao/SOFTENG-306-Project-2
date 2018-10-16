using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Utils;

namespace Quests
{
    public class QuestManager : Singleton<QuestManager>
    {
        private const string JsonFile = "quest-log.dat";

        private QuestLog questLog;
        public QuestLog QuestLog
        {
            get
            {
                if (questLog == null)
                {
                    questLog = Load();
                }

                return questLog;
            }
        }

        private readonly List<Quest> quests = new List<Quest>();
        public ReadOnlyCollection<Quest> Quests
        {
            get { return quests.AsReadOnly(); }
        }

        public Quest Current
        {
            get
            {
                foreach (var quest in Quests)
                {
                    if (!QuestLog.IsFinished(quest))
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
                Description = "Catherine is waiting for me at the software workshop"
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

        private void Awake()
        {
            // Load the quest log upon wake
            var x = QuestLog;
        }

        private void Update()
        {
            if (QuestLog.Dirty)
            {
                QuestLog.Dirty = false;
                DebounceSave.Run();
            }
        }

        private QuestLog Load()
        {
            return Toolbox.Instance.JsonFiles.Read<QuestLog>(JsonFile) ?? new QuestLog();
        }

        /// <summary>
        /// Do not invoke directly unless required - use <see cref="DebounceSave"/>
        /// </summary>
        private void Save()
        {
            Toolbox.Instance.JsonFiles.Write(JsonFile, questLog);
        }
    }
}
