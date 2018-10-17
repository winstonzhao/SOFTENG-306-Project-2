using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class NameSelectionController : MonoBehaviour
    {
        private void Start()
        {
            var textField = transform.Find("InputField").Find("Text").GetComponent<Text>();
            var continueButton = transform.Find("Continue").GetComponent<Button>();

            continueButton.onClick.AddListener(() =>
            {
                var gameManager = Toolbox.Instance.GameManager;
                // Set the username
                gameManager.Player.Username = textField.text;
                // Re-set the same object to try trigger a save as soon as possible
                gameManager.Player = gameManager.Player;
                // Go to the initial scene
                gameManager.ChangeScene("Engineering Lobby");
            });
        }
    }
}
