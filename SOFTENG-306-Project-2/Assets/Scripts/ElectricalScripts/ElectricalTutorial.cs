using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ElectricalTutorial : MonoBehaviour {

    public Button start;
    public string path;

    void Start()
    {
        start = start.GetComponent<Button>();
        start.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene(path);
    }
}
