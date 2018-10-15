using System;
using UnityEngine;

namespace Achievements
{
    [Serializable]
    public class Achievement
    {
        [SerializeField]
        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [SerializeField]
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        [SerializeField]
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
