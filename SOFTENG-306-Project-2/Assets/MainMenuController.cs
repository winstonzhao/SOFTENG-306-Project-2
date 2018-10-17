using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private void Start()
    {
        var play = transform.Find("Play").GetComponent<Button>();
        var hiscores = transform.Find("Highscores").GetComponent<Button>();
        var quit = transform.Find("Leave Game").GetComponent<Button>();

        var player = Toolbox.Instance.GameManager.Player;
        var scene = string.IsNullOrEmpty(player.Username) ? "Name Selection" : player.Scene ?? "Engineering Lobby";

        // Setup buttons
        play.onClick.AddListener(() => { Toolbox.Instance.GameManager.ChangeScene(scene); });
        hiscores.onClick.AddListener(() => { Toolbox.Instance.GameManager.ChangeScene("Hiscores"); });
        quit.onClick.AddListener(() => { Toolbox.Instance.GameManager.QuitGame(); });
    }
}
