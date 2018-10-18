using System;
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// Representation of a message the user sends.
    ///
    /// Note: when sending a message to the server, the only field that needs to be set is <see cref="message"/>. The
    /// owner, id etc. are set by the server for security purposes.
    /// </summary>
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

        /// <summary>
        /// The id of the message - higher is newer; but no guarantees
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// The username of the person who sent this message
        /// </summary>
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        /// <summary>
        /// The text sent by the user through the game input field
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// When the server was sent - more likely when the user received the message.
        /// </summary>
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
