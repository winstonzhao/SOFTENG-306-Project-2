using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * Used to transition between scenes for the Electrical minigame
 */
public class Scene_Transition : MonoBehaviour {

    public Button start;
    public string path;

    /*
     * Get the button that is clicked
     */
    void Start()
    {
        start = start.GetComponent<Button>();
        start.onClick.AddListener(TaskOnClick);
    }

    /*
     * Load the input scene on click
     */
    void TaskOnClick()
    {
        SceneManager.LoadScene(path);
    }
}
