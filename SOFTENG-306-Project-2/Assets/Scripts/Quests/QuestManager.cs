using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using UnityEngine;
using Utils;

namespace Quests
{
    public class QuestManager : Singleton<QuestManager>
    {
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

        private string PersistentDataPath;

        public QuestManager()
        {
            var every = TimeSpan.FromSeconds(3);
            DebounceSave = new DebounceAction(every, Save);
        }

        private void Awake()
        {
            PersistentDataPath = Application.persistentDataPath;

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
            var file = PersistentDataPath + "/quest-log.dat";

            if (!File.Exists(file))
            {
                return new QuestLog();
            }

            var serialized = File.ReadAllText(file);

            return JsonUtility.FromJson<QuestLog>(serialized);
        }

        /// <summary>
        /// Do not invoke directly unless required - use <see cref="DebounceSave"/>
        /// </summary>
        private void Save()
        {
            var serialized = JsonUtility.ToJson(QuestLog);

            var file = PersistentDataPath + "/quest-log.dat";

            File.WriteAllText(file, serialized);
        }
    }
}
