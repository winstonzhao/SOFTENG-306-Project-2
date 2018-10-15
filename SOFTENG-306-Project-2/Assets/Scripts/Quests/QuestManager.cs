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
            return Toolbox.Instance.JsonFiles.Read<QuestLog>(JsonFile);
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
