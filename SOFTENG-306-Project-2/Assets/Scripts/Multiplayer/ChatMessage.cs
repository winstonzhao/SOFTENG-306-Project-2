using System;
using UnityEngine;

namespace Multiplayer
{
    [Serializable]
    public class ChatMessage
    {
        [SerializeField]
        private int id;

        [SerializeField]
        private string owner;

        [SerializeField]
        private string message;

        [SerializeField]
        private string sentAt;

        private DateTime sentAtDateTime = DateTime.MinValue;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public DateTime SentAt
        {
            get
            {
                // Lazily parse the value
                if (sentAtDateTime == DateTime.MinValue && sentAt != null)
                {
                    sentAtDateTime = DateTime.Parse(sentAt);
                }

                return sentAtDateTime;
            }
            set
            {
                sentAtDateTime = value;
                // Convert to ISO-8601 format
                sentAt = value.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            }
        }
    }
}
