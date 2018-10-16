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
    private Sprite correctSprite;
    
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
        correctSprite = Resources.Load<Sprite>("ui/red_boxCheckmark");
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
            var achievements = Toolbox.Instance.AchievementsManager.All;
            var scale = transform.localScale.y;
            int i = 0;
            foreach (var achievement in achievements)
            {
                var achievementObject = Instantiate(achievementPrefab, achievementsTab.transform).gameObject;
                Vector3 initialPosition = achievementObject.GetComponent<RectTransform>().position;
                initialPosition.y -= i * 30 * scale;
                achievementObject.GetComponent<RectTransform>().position = initialPosition;
                achievementObject.transform.Find("Text").gameObject.GetComponent<Text>().text = achievement.Title;
                achievementObject.GetComponent<Image>().sprite = correctSprite;
                i++;
            }
        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
