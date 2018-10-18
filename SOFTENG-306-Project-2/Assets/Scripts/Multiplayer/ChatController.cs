using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// Manages chat for the multiplayer module
    /// </summary>
    public class ChatController : Singleton<ChatController>
    {
        /// <summary>
        /// The number of seconds a message should be visible for after the user sends it
        /// </summary>
        [NonSerialized]
        public readonly TimeSpan ActiveDuration = TimeSpan.FromSeconds(4);

        /// <summary>
        /// The most recent messages from the server
        /// </summary>
        [NonSerialized]
        public readonly List<ChatMessage> Messages = new List<ChatMessage>();

        /// <summary>
        /// The last message id we've received from the server
        /// </summary>
        [NonSerialized]
        public int LastChatMessageId;

        /// <summary>
        /// Reference to the game manager for convenience
        /// </summary>
        private GameManager GameManager;

        /// <summary>
        /// Reference to the multiplayer controller for convenience
        /// </summary>
        private MultiplayerController MultiplayerController;

        /// <summary>
        /// A message the user has sent - this is used to display the message optimistically.
        ///
        /// That is, a message the user types in is automatically shown without delay.
        /// </summary>
        private ChatMessage OptimisticMessage;

        private void Awake()
        {
            GameManager = Toolbox.Instance.GameManager;
            MultiplayerController = Toolbox.Instance.MultiplayerController;
        }

        /// <summary>
        /// Ask the server for some number of messages after <paramref name="lastChatMessageId"/>
        ///
        /// The server determines how many messages are returned, but we can specify a limit
        /// </summary>
        /// <param name="lastChatMessageId"></param>
        public void Sync(int lastChatMessageId, int limit = 20)
        {
            // Do nothing if there are no new messages
            if (lastChatMessageId == LastChatMessageId)
            {
                return;
            }

            // Ask the server for the new messages
            var request = new GetMessages { SinceMessageId = LastChatMessageId, Limit = limit };
            var json = JsonUtility.ToJson(request);
            MultiplayerController.SendAsync("get-messages\n" + json, success => { });
        }

        /// <summary>
        /// Handle the response from the server when it returns a list of messages
        /// </summary>
        /// <param name="messages"></param>
        public void Sync(List<ChatMessage> messages)
        {
            if (messages.Count == 0)
            {
                return;
            }

            foreach (var message in messages)
            {
                // This assumes that the messages are received in ascending id order
                if (message.Id > LastChatMessageId)
                {
                    // Clear the optimistic message once we receive a message sent by our user
                    if (message.Owner == GameManager.Player.Username)
                    {
                        OptimisticMessage = null;
                    }

                    LastChatMessageId = message.Id;
                    Messages.Add(message);

                    // Offset by the time drift between client & server
                    message.SentAt += MultiplayerController.TimeDrift;
                }
            }
        }

        /// <summary>
        /// Send a message the user has typed to the server
        /// </summary>
        /// <param name="message">the text the user typed</param>
        public void Send(string message)
        {
            var chatMessage = new ChatMessage { Message = message, SentAt = DateTime.Now };

            var json = JsonUtility.ToJson(chatMessage);
            var payload = "send-chat-message\n" + json;
            MultiplayerController.SendAsync(payload, success => { });

            OptimisticMessage = chatMessage;
        }

        /// <summary>
        /// Gets the latest message by a specific user - use this to display messages above users
        /// </summary>
        /// <param name="username">the user we want to get chat messages for</param>
        /// <param name="sinceMessageId">only give me messages that have an id higher than this</param>
        /// <returns>a chat single chat message from the user, or null if none</returns>
        public ChatMessage GetLastMessageBy(string username, int sinceMessageId = int.MinValue)
        {
            // Show local/optimistic message if possible
            if (username == GameManager.Player.Username)
            {
                var om = OptimisticMessage;
                if (om != null)
                {
                    return om;
                }
            }

            // No new messages since the user last checked
            if (sinceMessageId == LastChatMessageId)
            {
                return null;
            }

            // Go through messages in descending order to find the latest
            for (var i = Messages.Count - 1; i >= 0; i--)
            {
                var message = Messages[i];

                if (message.Id <= sinceMessageId)
                {
                    return null;
                }

                if (message.Owner == username)
                {
                    return message;
                }
            }

            return null;
        }
    }
}
