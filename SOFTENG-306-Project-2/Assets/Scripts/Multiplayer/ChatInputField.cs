using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UserInterface;
using WebSocketSharp;

namespace Multiplayer
{
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

            if (Input.GetKeyDown(KeyCode.Return))
            {
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
