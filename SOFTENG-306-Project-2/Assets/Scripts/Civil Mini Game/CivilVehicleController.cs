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

public class CivilVehicleController : MonoBehaviour {

    public static CivilVehicleController instance;
    public List<AstarAgent> AstarAgents = new List<AstarAgent>();
    public List<GoalAgent> Goals = new List<GoalAgent>();
    public Canvas Dialog;
    public int TimeLimit = 10;
    public int Budget = 1000;

    private TextMeshProUGUI timerArea;
    private bool timerNotStopped = true;
    private TextMeshProUGUI budgetArea;

    private void Awake()

    {
        Debug.Log("CivilVehicleController Alive");

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        timerArea = GetTextArea("Timer");
        string timerLabel = String.Format("{0:00}:00", (TimeLimit));
        timerArea.SetText(timerLabel);

        budgetArea = GetTextArea("Budget");
        budgetArea.SetText("$" + Budget);
    }

    public void run()
    {
        foreach (GoalAgent goal in instance.Goals)
        {
            foreach (AstarAgent agent in instance.AstarAgents)
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

        // set parameters in the result info
        SetPlayerName("Sean");
        SetTimeAndAmount(3, 300);

        if (AllCarsReachedGoal()) // Win
        {
            Debug.Log("Win!!!!!!");
            ToggleCanvasGroup("Bad", false);
            ToggleCanvasGroup("Good", true);
        }
        else // Lose
        {
            Debug.Log("Lose:(");
            ToggleCanvasGroup("Good", false);
            ToggleCanvasGroup("Bad", true);
        }
        timerNotStopped = false;
        instance.Dialog.enabled = !instance.Dialog.enabled;
    }

    private bool AreCarsMoving()
    {
     
        foreach (AstarAgent agent in instance.AstarAgents)
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
        foreach (AstarAgent agent in instance.AstarAgents)
        {
            if (!agent.hasReachedGoal)
            {
                return false;
            }
        }

        return true;
    }

    private void ToggleCanvasGroup(string tagName, bool show)
    {
        CanvasGroup group = GameObject.FindGameObjectWithTag(tagName).GetComponent<CanvasGroup>();
        group.alpha = show ? 1 : 0;
        group.interactable = show;

    }

    private void SetTimeAndAmount(int timeInSeconds, int amount)
    {
        TextMeshProUGUI resultInfoArea = GetTextArea("ResultInfo");

        string text = resultInfoArea.text;

        text = text.Replace("<time>", timeInSeconds.ToString() + (timeInSeconds > 1 ? " seconds" : " second"));
        text = text.Replace("<amount>", amount.ToString());

        resultInfoArea.SetText(text);
    }

    private void SetPlayerName(string name)
    {
        TextMeshProUGUI nameArea = GetTextArea("PlayerName");
        nameArea.SetText(name);
    }

    float currCountdownValueTenthSeconds;
    public IEnumerator StartCountdown(int timeLimit)
    {
        float countdownValue = (TimeLimit - 1) * 10;
        currCountdownValueTenthSeconds = countdownValue;
        while (currCountdownValueTenthSeconds >= 0 && timerNotStopped)
        {
            string timerLabel = String.Format("{0:00}:{1:00}", (currCountdownValueTenthSeconds) / 10, (currCountdownValueTenthSeconds % 10) * 10);
            timerArea.SetText(timerLabel);
            yield return new WaitForSeconds(0.1f);
            currCountdownValueTenthSeconds--;
        }
    }

    private TextMeshProUGUI GetTextArea(string tagName)
    {
        return GameObject.FindGameObjectWithTag(tagName).GetComponent<TextMeshProUGUI>();
    }

    public void UpdateBudget(int itemPrice)
    {
        instance.Budget += itemPrice;
        instance.budgetArea.SetText("$" + instance.Budget);
        Debug.Log("updated budget to " + instance.Budget);
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
        return itemPrice <= instance.Budget;
    }
}
