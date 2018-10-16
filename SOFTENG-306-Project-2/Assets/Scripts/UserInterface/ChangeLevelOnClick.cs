using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLevelOnClick : MonoBehaviour {

    public string LevelName;

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Toolbox.Instance.GameManager.ChangeScene(LevelName);
        });
	}
	
	
}
