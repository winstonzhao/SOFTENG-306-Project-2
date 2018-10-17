using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class NameSelectionController : MonoBehaviour
    {
        private Button.ButtonClickedEvent ClickContinue;

        private Button.ButtonClickedEvent ClickBack;

        private bool WasFocused;

        private void Start()
        {
            var textField = transform.Find("Name Input Field").Find("Text").GetComponent<Text>();
            var continueButton = transform.Find("Continue").GetComponent<Button>();
            var backButton = transform.Find("Back").GetComponent<Button>();
            ClickContinue = continueButton.onClick;
            ClickBack = backButton.onClick;

            ClickContinue.AddListener(() =>
            {
                var userInput = textField.text.Trim();

                if (string.IsNullOrEmpty(userInput))
                {
                    return;
                }

                var gameManager = Toolbox.Instance.GameManager;
                // Set the username
                gameManager.Player.Username = userInput;
                // Re-set the same object to try trigger a save as soon as possible
                gameManager.Player = gameManager.Player;
                // Go to the initial scene
                gameManager.ChangeScene("Engineering Lobby");
            });
        }

        public void Update()
        {
            // Enter to continue
            if (Input.GetKeyDown(KeyCode.Return) && WasFocused)
            {
                ClickContinue.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClickBack.Invoke();
            }

            // Record whether the input field was focused on the last frame tick
            var focus = Toolbox.Instance.FocusManager.Focus;
            WasFocused = focus != null && focus.gameObject.name == "Name Input Field";
        }
    }
}
