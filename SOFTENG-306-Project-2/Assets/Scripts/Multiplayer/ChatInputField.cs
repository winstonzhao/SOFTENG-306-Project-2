using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UserInterface;
using WebSocketSharp;

namespace Multiplayer
{
    /// <summary>
    /// Script to handle the chat for the input field.
    ///
    /// Handles things like chat to enter & sending the message to the chat controller - which sends it to the server.
    /// </summary>
    public class ChatInputField : MonoBehaviour
    {
        private ChatController ChatController;

        private InputField InputField;

        private bool WasFocused;

        private void Start()
        {
            ChatController = Toolbox.Instance.ChatController;

            if (ChatController == null)
            {
                Debug.LogError("ChatController not found in scene, destroying chat input field");

                DestroyImmediate(gameObject);

                return;
            }

            InputField = GetComponent<InputField>();
        }

        public void Update()
        {
            var isFocused = InputField.isFocused;

            // Press enter to chat - or send message if already focused
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // If WasFocused - send the message
                if (WasFocused)
                {
                    // Note: pressing enter relinquishes focus; this is why we check WasFocused
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        var text = InputField.text;

                        if (!text.IsNullOrEmpty())
                        {
                            InputField.text = "";
                            ChatController.Send(text);
                        }

                        // Use this to make sure pressing space doesn't focus the input field again
                        EventSystem.current.SetSelectedGameObject(null);
                    }
                }
                else
                {
                    // Press enter to chat
                    if (Toolbox.Instance.FocusManager.Focus == null)
                    {
                        InputField.Select();
                    }
                }
            }

            WasFocused = isFocused;
        }
    }
}
