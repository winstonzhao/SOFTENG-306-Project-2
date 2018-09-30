using System;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class ChatController : MonoBehaviour
    {
        [NonSerialized]
        public readonly TimeSpan ActiveDuration = TimeSpan.FromSeconds(4);

        [NonSerialized]
        public readonly List<ChatMessage> Messages = new List<ChatMessage>();

        [NonSerialized]
        public int LastChatMessageId;

        private MultiplayerController MultiplayerController;

        private void Start()
        {
            MultiplayerController = FindObjectOfType<MultiplayerController>();
        }

        public void Sync(int lastChatMessageId)
        {
            // Do nothing if there are no new messages
            if (lastChatMessageId == LastChatMessageId)
            {
                return;
            }

            // Ask the server for the new messages
            var request = new GetMessages { sinceMessageId = LastChatMessageId, limit = 20 };
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
                if (message.id > LastChatMessageId)
                {
                    LastChatMessageId = message.id;
                    Messages.Add(message);

                    var sentAt = DateTime.Parse(message.sentAt);
                    message.sentAtDateTime = sentAt;
                }
            }
        }

        public void Send(string message)
        {
            var chatMessage = new ChatMessage { message = message };
            var json = JsonUtility.ToJson(chatMessage);
            var payload = "send-chat-message\n" + json;
            MultiplayerController.SendAsync(payload, success => { });
        }

        public ChatMessage GetLastMessageBy(string username, int sinceMessageId = int.MinValue)
        {
            // No new messages since the user last checked
            if (sinceMessageId == LastChatMessageId)
            {
                return null;
            }

            // Go through messages in descending order to find the latest
            for (var i = Messages.Count - 1; i >= 0; i--)
            {
                var message = Messages[i];

                if (message.id <= sinceMessageId)
                {
                    return null;
                }

                if (message.owner == username)
                {
                    return message;
                }
            }

            return null;
        }
    }
}
