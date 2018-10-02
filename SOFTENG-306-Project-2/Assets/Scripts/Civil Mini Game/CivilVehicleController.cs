using System.Collections;
using System.Collections.Generic;
using UltimateIsometricToolkit.physics;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.Pathfinding;
using UnityEngine;
using System;
using System.Diagnostics;
using TMPro;
using Debug = UnityEngine.Debug;
using UnityEngine.SceneManagement;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using Ultimate_Isometric_Toolkit.Scripts.physics;
using UnityEngine.Analytics;

public class CivilVehicleController : MonoBehaviour {

    //public static CivilVehicleController instance;
    public List<AstarAgent> AstarAgents = new List<AstarAgent>();
    public List<GoalAgent> Goals = new List<GoalAgent>();
    public Canvas Dialog;
    public Canvas Tutorial;
    
    public int TimeLimit = 10;
    public int Budget = 1000;
    public string NextLevelName;

    private string PlayerName = "Anonymous";
    private TextMeshProUGUI timerArea;
    private bool timerNotStopped = true;
    private TextMeshProUGUI budgetArea;
    private float currCountdownValueTenthSeconds;
    private float lastClickTime = 0;
    private float catchTime = 0.2f;
    

    private const int TUTORIAL_SLIDE_COUNT = 8; 
    private int tutorialSlideNumber;

    private void Awake()

    {
        timerArea = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        string timerLabel = String.Format("{0:00}:00", (TimeLimit));
        timerArea.SetText(timerLabel);

        budgetArea = GameObject.Find("Budget").GetComponent<TextMeshProUGUI>();
        budgetArea.SetText("$" + Budget);
    }

    void Update()
    {
        CheckMouseClickForRotation();
    }

    private void CheckMouseClickForRotation()
    {
        //mouse ray in isometric coordinate system 
        var isoRay = Isometric.MouseToIsoRay();

        //do an isometric raycast on double left mouse click 
        if(Input.GetMouseButtonDown(0)){
            if(Time.time-lastClickTime < catchTime){
                //double click
                Debug.Log("double click");
                IsoRaycastHit isoRaycastHit;
                if (IsoPhysics.Raycast(isoRay, out isoRaycastHit))
                {
                    GameObject hitObject = isoRaycastHit.Collider.gameObject;
                    Debug.Log("we clicked on " + hitObject.name + " at " + isoRaycastHit.Point + " tagged as " + hitObject.tag);
                    if (hitObject.tag == "BuildingBlock")
                    {
                        hitObject.GetComponent<DraggableIsoItem>().Rotate();
                        Debug.Log(hitObject.name + " rotated to direction " + hitObject.GetComponent<DraggableIsoItem>().direction);
                    }
                }
            }
            lastClickTime=Time.time;
        }
        
        
/*        if (Input.GetMouseButtonDown(0))
        {
            IsoRaycastHit isoRaycastHit;
            if (IsoPhysics.Raycast(isoRay, out isoRaycastHit))
            {
                GameObject hitObject = isoRaycastHit.Collider.gameObject;
                Debug.Log("we clicked on " + hitObject.name + " at " + isoRaycastHit.Point + " tagged as " + hitObject.tag);
                if (hitObject.tag == "BuildingBlock")
                {
                    hitObject.GetComponent<DraggableIsoItem>().Rotate();
                    Debug.Log(hitObject.name + " rotated to direction " + hitObject.GetComponent<DraggableIsoItem>().direction);
                }
            }
        }*/
    }

    public void run()
    {
        foreach (GoalAgent goal in Goals)
        {
            foreach (AstarAgent agent in AstarAgents)
            {
                if (agent.Type == goal.GoalType)
                {
                    agent.MoveTo(goal.GetComponentInParent<IsoTransform>().Position);
                }
            }
        }
        StartCoroutine(StartCountdown(TimeLimit));
        StartCoroutine(WaitCarsStop());
    }

    IEnumerator WaitCarsStop()
    {
        yield return new WaitUntil(() => !AreCarsMoving());

        if (AllCarsReachedGoal()) // Win
        {
            Debug.Log("Win!!!!!!");
            // set parameters in the result info
            SetPlayerName(PlayerName);
            SetTimeAndAmount((int) Math.Round(TimeLimit - currCountdownValueTenthSeconds / 10), Budget);
            ToggleDialogDisplay(Dialog, "BadPanel", false);
            ToggleDialogDisplay(Dialog, "GoodPanel", true);
        }
        else // Lose
        {
            Debug.Log("Lose:(");
            SetFailInfo("Make sure there are roads for all the cars to travel on to reach their destination withing the time limit.");
            ToggleDialogDisplay(Dialog, "GoodPanel", false);
            ToggleDialogDisplay(Dialog, "BadPanel", true);
        }
        timerNotStopped = false;
        Dialog.enabled = !Dialog.enabled;
    }

    

    private bool AreCarsMoving()
    {
     
        foreach (AstarAgent agent in AstarAgents)
        {
            if (!agent.hasReachedGoal && !agent.noPathFound)
            {
                return true;
            }
        }

        return false;
    }

    private bool AllCarsReachedGoal()
    {
        foreach (AstarAgent agent in AstarAgents)
        {
            if (!agent.hasReachedGoal)
            {
                return false;
            }
        }

        return true;
    }

    private void ToggleDialogDisplay(Canvas canvas, string groupName, bool show)
    {
        GameObject group = FindObject(canvas.gameObject, groupName).gameObject;
        if (group != null)
        {
            group.SetActive(show);
        }
    }

    private void SetTimeAndAmount(int timeInSeconds, int amount)
    {
        TextMeshProUGUI resultInfoArea = GameObject.Find("ResultInfo").GetComponent<TextMeshProUGUI>();

        string text = resultInfoArea.text;

        text = text.Replace("<time>", timeInSeconds.ToString() + (timeInSeconds == 1 ? " second" : " seconds"));
        text = text.Replace("<amount>", amount.ToString());

        resultInfoArea.SetText(text);
    }

    private void SetPlayerName(string name)
    {
        TextMeshProUGUI nameArea = GameObject.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        nameArea.SetText(name);
    }

    private void SetFailInfo(string failInfo)
    {
        TextMeshProUGUI failInfoArea = GameObject.Find("FailInfo").GetComponent<TextMeshProUGUI>();
        failInfoArea.SetText(failInfo);
    }

    
    public IEnumerator StartCountdown(int timeLimit)
    {
        float countdownValue = (TimeLimit - 1) * 10;
        currCountdownValueTenthSeconds = countdownValue;
        while (currCountdownValueTenthSeconds >= 0 && timerNotStopped)
        {
            Debug.Log((currCountdownValueTenthSeconds) / 10);
            string timerLabel = String.Format("{0:00}:{1:00}", Math.Floor((currCountdownValueTenthSeconds) / 10), (currCountdownValueTenthSeconds % 10) * 10);
            timerArea.SetText(timerLabel);
            yield return new WaitForSeconds(0.1f);
            currCountdownValueTenthSeconds--;
        }
    }

    public void UpdateBudget(int itemPrice)
    {
        Budget += itemPrice;
        budgetArea.SetText("$" + Budget);
        Debug.Log("updated budget to " + Budget);
        UpdateBudgetAvailability();
    }

    private void UpdateBudgetAvailability()
    {
        GameObject[] tileFactories = GameObject.FindGameObjectsWithTag("TileFactory");
        foreach (GameObject tileFactory in tileFactories)
        {
            IsoDropZone isoDropZone = tileFactory.GetComponent<IsoDropZone>();
            if (!IsBudgetAvailable(isoDropZone.ItemPrice))
            {
                isoDropZone.setEnable(false);
            }
            else
            {
                isoDropZone.setEnable(true);
            }
        }
    }

    public bool IsBudgetAvailable(int itemPrice)
    {
        return itemPrice <= Budget;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Debug.Log("Next level " + NextLevelName);
        SceneManager.LoadScene(NextLevelName);
    }

    public void StartTutorial()
    {
        Tutorial.gameObject.SetActive(true);
        tutorialSlideNumber = 1;
        for (int i = 1; i < TUTORIAL_SLIDE_COUNT + 1; i++)
        {
            ToggleDialogDisplay(Tutorial, "Slide" + i, false);
        }
        ToggleDialogDisplay(Tutorial, "Slide" + tutorialSlideNumber, true);

    }

    public void StopTutorial()
    {
        Tutorial.gameObject.SetActive(false);
    }

    public void NextTutorialSlide()
    {
        ToggleDialogDisplay(Tutorial, "Slide" + tutorialSlideNumber, false);
        tutorialSlideNumber++;
        ToggleDialogDisplay(Tutorial, "Slide" + tutorialSlideNumber, true);
        
    }
    
    public void PreviousTutorialSlide()
    {
        ToggleDialogDisplay(Tutorial, "Slide" + tutorialSlideNumber, false);
        tutorialSlideNumber--;
        ToggleDialogDisplay(Tutorial, "Slide" + tutorialSlideNumber, true);
    }

    public void CloseDialog()
    {
        Dialog.enabled = !Dialog.enabled;
    }
    
    
    /**
     * Find an object in a parent object including parent objects
     */
    public GameObject FindObject(GameObject parent, string name)
    {
        Transform[] trs= parent.GetComponentsInChildren<Transform>(true);
        foreach(Transform t in trs){
            if(t.name == name){
                return t.gameObject;
            }
        }
        return null;
    }

}
