using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    private GameObject backpackButton;
    private GameObject achievementsButton;
    private GameObject backpackTabPrefab;
    private GameObject achievementsTabPrefab;
    private GameObject backpackTab;
    private GameObject achievementsTab;
    private GameObject achievementPrefab;
    
    enum Tab {
        BACKPACK, ACHIEVEMENTS
    }

	// Use this for initialization
	void Start () {
        GetComponent<Canvas>().sortingOrder = 500;
        backpackButton = transform.Find("Backpack Button").gameObject;
        achievementsButton = transform.Find("Achievements Button").gameObject;
        backpackTabPrefab = Resources.Load<GameObject>("Prefabs/User Interface/BackpackTab");
        achievementsTabPrefab = Resources.Load<GameObject>("Prefabs/User Interface/AchievementsTab");
        achievementPrefab = Resources.Load<GameObject>("Prefabs/User Interface/Achievement Text + Checkbox");
        backpackTab = Instantiate(backpackTabPrefab, transform);

        backpackButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Destroy(achievementsTab);
            if (!backpackTab)
            {
                backpackTab = Instantiate(backpackTabPrefab, transform);
            }
            backpackButton.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.yellow;
            achievementsButton.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.white;
        });

        achievementsButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Destroy(backpackTab);
            if (!achievementsTab)
            {
                achievementsTab = Instantiate(achievementsTabPrefab, transform);
            }
            backpackButton.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.white;
            achievementsButton.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.yellow;
            for (int i = 0; i < 2; i++)
            {
                var achievement = Instantiate(achievementPrefab, achievementsTab.transform).gameObject;
                Vector3 initialPosition = achievement.GetComponent<RectTransform>().position;
                initialPosition.y -= i * 30;
                achievement.GetComponent<RectTransform>().position = initialPosition;
                achievement.transform.Find("Text").gameObject.GetComponent<Text>().text = "fuck u";
            }
        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
