using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GameObject backpackButton;
    private GameObject achievementsButton;
    private GameObject closeButton;
    private GameObject timetableTabPrefab;
    private GameObject diplomaTabPrefab;
    private GameObject achievementsTabPrefab;
    private GameObject backpackTab;
    private GameObject achievementsTab;
    private GameObject rowPrefab;
    private Sprite correctSprite;
    private Sprite incorrectSprite;

    // Use this for initialization
    private void Start()
    {
        GetComponent<Canvas>().sortingOrder = 500;
        backpackButton = transform.Find("Backpack Button").gameObject;
        achievementsButton = transform.Find("Achievements Button").gameObject;
        closeButton = transform.Find("Close Button").gameObject;
        timetableTabPrefab = Resources.Load<GameObject>("Prefabs/User Interface/BackpackTab");
        diplomaTabPrefab = Resources.Load<GameObject>("Prefabs/User Interface/Diploma");
        achievementsTabPrefab = Resources.Load<GameObject>("Prefabs/User Interface/AchievementsTab");
        rowPrefab = Resources.Load<GameObject>("Prefabs/User Interface/Achievement Text + Checkbox");
        correctSprite = Resources.Load<Sprite>("ui/red_boxCheckmark");
        incorrectSprite = Resources.Load<Sprite>("ui/grey_boxCheckmark");

        // Click outside the interface to exit
        var overlay = transform.Find("Overlay");
        overlay.GetComponent<Button>().onClick.AddListener(Close);
        closeButton.GetComponent<Button>().onClick.AddListener(Close);

        // Setup tab buttons to switch to the appropriate tab
        backpackButton.GetComponent<Button>().onClick.AddListener(SwitchToBackpack);
        achievementsButton.GetComponent<Button>().onClick.AddListener(SwitchToAchievements);

        // Open backpack tab by default
        SwitchToBackpack();
    }

    private void Update()
    {
        // Todo - should probably do keyboard shortcuts in a centralized location so we can essentially prevent even propagation
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (backpackTab == null)
            {
                SwitchToBackpack();
            }
            else
            {
                SwitchToAchievements();
            }
        }
    }

    private void Close()
    {
        Toolbox.Instance.UIManager.ToggleUI();
    }

    private void SwitchToBackpack()
    {
        if (Toolbox.Instance.QuestManager.Current.Id == "free-roam")
        {
            SwitchToDiploma();
        }
        else
        {
            SwitchToTimetable();
        }

        backpackButton.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.yellow;
        achievementsButton.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.white;
    }

    private void SwitchToDiploma()
    {
        Destroy(achievementsTab);
        if (!backpackTab)
        {
            backpackTab = Instantiate(diplomaTabPrefab, transform);
        }
    }

    private void SwitchToTimetable()
    {
        Destroy(achievementsTab);
        if (!backpackTab)
        {
            backpackTab = Instantiate(timetableTabPrefab, transform);
        }

        var manager = Toolbox.Instance.QuestManager;
        var quests = Toolbox.Instance.QuestManager.Quests;
        var scale = transform.localScale.y;
        int i = 0;
        foreach (var quest in quests)
        {
            if (string.IsNullOrEmpty(quest.Title))
            {
                continue;
            }

            var row = Instantiate(rowPrefab, backpackTab.transform).gameObject;
            Vector3 initialPosition = row.GetComponent<RectTransform>().position;
            initialPosition.y -= i * 30 * scale;
            row.GetComponent<RectTransform>().position = initialPosition;
            row.transform.Find("Text").gameObject.GetComponent<Text>().text = quest.Title;
            row.GetComponent<Image>().sprite = manager.HasFinished(quest.Id) ? correctSprite : incorrectSprite;
            i++;
        }
    }

    private void SwitchToAchievements()
    {
        Destroy(backpackTab);
        if (!achievementsTab)
        {
            achievementsTab = Instantiate(achievementsTabPrefab, transform);
        }

        backpackButton.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.white;
        achievementsButton.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.yellow;

        var manager = Toolbox.Instance.AchievementsManager;
        var achievements = manager.All;
        var scale = transform.localScale.y;
        int i = 0;
        foreach (var achievement in achievements)
        {
            var row = Instantiate(rowPrefab, achievementsTab.transform).gameObject;
            Vector3 initialPosition = row.GetComponent<RectTransform>().position;
            initialPosition.y -= i * 30 * scale;
            row.GetComponent<RectTransform>().position = initialPosition;
            row.transform.Find("Text").gameObject.GetComponent<Text>().text = achievement.Title;
            row.GetComponent<Image>().sprite =
                manager.IsCompleted(achievement.Id) ? correctSprite : incorrectSprite;
            i++;
        }
    }
}
