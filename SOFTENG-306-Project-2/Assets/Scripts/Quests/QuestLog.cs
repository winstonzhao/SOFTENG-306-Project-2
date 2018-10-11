using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    [Serializable]
    public class QuestLog
    {
        [NonSerialized]
        public bool Dirty;

        [SerializeField]
        private Dictionary<string, bool> completed = new Dictionary<string, bool>();

        public bool IsFinished(Quest quest)
        {
            return IsFinished(quest.Id);
        }

        public bool IsFinished(string questId)
        {
            return completed[questId];
        }

        public void MarkFinished(Quest quest)
        {
            MarkFinished(quest.Id);
        }

        public void MarkFinished(string questId)
        {
            completed[questId] = true;
            Dirty = true;
        }
    }
}
