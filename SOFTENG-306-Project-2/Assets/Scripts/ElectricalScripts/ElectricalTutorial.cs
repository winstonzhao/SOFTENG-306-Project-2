using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ElectricalTutorial : MonoBehaviour {

    //Make sure to attach these Buttons in the Inspector
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
