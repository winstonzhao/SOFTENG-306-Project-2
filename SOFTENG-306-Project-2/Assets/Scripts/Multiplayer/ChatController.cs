using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace Multiplayer
{
    public class ChatController : Singleton<ChatController>
    {
        [NonSerialized]
        public readonly TimeSpan ActiveDuration = TimeSpan.FromSeconds(4);

        [NonSerialized]
        public readonly List<ChatMessage> Messages = new List<ChatMessage>();

        [NonSerialized]
        public int LastChatMessageId;

        private GameManager GameManager;

        private MultiplayerController MultiplayerController;

        private ChatMessage OptimisticMessage;

        private void Awake()
        {
            GameManager = Toolbox.Instance.GameManager;
            MultiplayerController = Toolbox.Instance.MultiplayerController;
        }

        public void Sync(int lastChatMessageId)
        {
            // Do nothing if there are no new messages
            if (lastChatMessageId == LastChatMessageId)
            {
                return;
            }

            // Ask the server for the new messages
            var request = new GetMessages { SinceMessageId = LastChatMessageId, Limit = 20 };
            var json = JsonUtility.ToJson(request);
            MultiplayerController.SendAsync("get-messages\n" + json, success => { });
        }

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

                    // Todo - use the server time in the future - I suspect something's wrong
                    // Parse the ISO-8601 date into a workable format
                    message.SentAt = DateTime.Now;
                }
            }
        }

        public void Send(string message)
        {
            var chatMessage = new ChatMessage { Message = message, SentAt = DateTime.Now };

            var json = JsonUtility.ToJson(chatMessage);
            var payload = "send-chat-message\n" + json;
            MultiplayerController.SendAsync(payload, success => { });

            OptimisticMessage = chatMessage;
        }

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
