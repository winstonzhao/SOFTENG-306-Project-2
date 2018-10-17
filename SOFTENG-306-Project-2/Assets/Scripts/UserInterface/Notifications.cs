using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UserInterface
{
    public class Notifications : Singleton<Notifications>
    {
        /// <summary>
        /// Store messages that can't be displayed when requested - e.g. in a scene without the notifications canvas
        /// </summary>
        private readonly List<string> Queue = new List<string>();

        /// <summary>
        /// The notification canvas for the current screen
        /// </summary>
        private GameObject Canvas;

        /// <summary>
        /// Shows a notification with the given <paramref name="text"/>
        /// </summary>
        /// <param name="text">the text to display in the notification</param>
        public void Show(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (Canvas == null)
            {
                Queue.Add(text);
                return;
            }

            StartCoroutine(Render(text));
        }

        /// <summary>
        /// Animates in and displays a notification with the given <paramref name="text"/>
        /// </summary>
        /// <param name="text">the text to display in the notification</param>
        /// <returns>the co-routine that displays the animation</returns>
        private IEnumerator Render(string text)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Notification");
            var notification = Instantiate(prefab, Canvas.transform);
            var speechBubble = notification.GetComponent<SpeechBubble>();

            // var achievement = Achievement;
            speechBubble.Message = text;

            var transform = notification.GetComponent<RectTransform>();

            var height = transform.sizeDelta.y;
            var position = transform.anchoredPosition;

            const float duration = 0.6f;

            for (var i = -1; i <= 1; i += 2)
            {
                var fromY = position.y;
                var toY = i * height / 2;
                var yDelta = toY - fromY;

                var time = 0f;

                while (time < duration)
                {
                    time += Time.deltaTime;

                    position.y = fromY + yDelta * Math.Min(time / duration, 1f);

                    if (transform == null)
                    {
                        yield break;
                    }

                    transform.anchoredPosition = position;

                    yield return null;
                }

                // After showing the notification, sleep before hiding it to let user read the notification
                if (i < 0)
                {
                    yield return new WaitForSeconds(3f);
                }
            }

            Destroy(notification);
        }

        private string LastScene;

        private void Awake()
        {
            // When the scene changes - find the new notifications canvas if present
            SceneManager.activeSceneChanged += (prev, next) => { OnSceneChange(); };
        }

        private void Start()
        {
            // Setup the script for the first time
            OnSceneChange();
        }

        /// <summary>
        /// Handle scene changes - i.e. display the queued notifications & grab the canvas in the new scene
        /// </summary>
        private void OnSceneChange()
        {
            Canvas = GameObject.Find("Notifications");

            // Display all messages in queue
            if (Canvas != null)
            {
                foreach (var message in Queue)
                {
                    Show(message);
                }

                Queue.Clear();
            }
        }
    }
}
