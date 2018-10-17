using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    void Start()
    {
        var play = transform.Find("Play").GetComponent<Button>();
        var hiscores = transform.Find("Highscores").GetComponent<Button>();
        var quit = transform.Find("Leave Game").GetComponent<Button>();

        var scene = Toolbox.Instance.GameManager.Player.Scene ?? "Engineering Lobby";

        // Setup buttons
        play.onClick.AddListener(() => { Toolbox.Instance.GameManager.ChangeScene(scene); });
        hiscores.onClick.AddListener(() => { Toolbox.Instance.GameManager.ChangeScene("Hiscores"); });
        quit.onClick.AddListener(() => { Toolbox.Instance.GameManager.QuitGame(); });
    }
}
