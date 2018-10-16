﻿using System.Collections;
using System.Collections.Generic;
using UltimateIsometricToolkit.physics;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.Pathfinding;
using UnityEngine;
using System;
using System.Diagnostics;
using Game;
using Game.Hiscores;
using TMPro;
using Debug = UnityEngine.Debug;
using UnityEngine.SceneManagement;
using Ultimate_Isometric_Toolkit.Scripts.Utils;
using Ultimate_Isometric_Toolkit.Scripts.physics;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class CivilLevelController : MonoBehaviour {

    public List<CivilCarAgent> CarAgents = new List<CivilCarAgent>();
    public List<GoalAgent> Goals = new List<GoalAgent>();
    public Canvas Dialog;
    public Canvas Tutorial;
    public Canvas AlienInfo;
    
    public int TimeLimit = 10;
    public int Budget = 1000;
    public int BudgetMaxScore = 50;
    public int TimeMaxScore = 30;
    public int CompletionBaseScore = 20;
    public float IdealTimeLeft;
    public float IdealBudgetLeft;
    
    public int ThisLevel;
    public string CheatLevelName;
    public string UndoCheatLevelName;
    
    public bool ShowTutorial = false;
    public bool ShowAlienInfo = false;
    

    private string PlayerName;
    private TextMesh timerArea;
    private bool timerNotStopped = true;
    private TextMesh budgetArea;
    private float currCountdownValueTenthSeconds;
    private int maxBudget;
    private int tutorialSlideNumber;
    private const int TUTORIAL_SLIDE_COUNT = 9;
    
    // game control buttons
    private List<GameObject> gameControlButtons = new List<GameObject>();
    private GameObject runButton;
    private GameObject restartButton;
    private GameObject exitButton;
    private GameObject cheatButton;

    private void Awake()
    {
        PlayerName = CivilGameManager.instance.playerName;
        maxBudget = Budget;
        timerArea = GameObject.Find("Timer").GetComponent<TextMesh>();
        string timerLabel = String.Format("{0:00}:00", (TimeLimit));
        timerArea.text = timerLabel;

        budgetArea = GameObject.Find("Budget").GetComponent<TextMesh>();
        budgetArea.text = "$" + Budget;
        
        // get references to the game control buttons
        gameControlButtons.Add(GameObject.Find("RunButton"));
        gameControlButtons.Add(GameObject.Find("RestartButton"));
        gameControlButtons.Add(GameObject.Find("ExitButton"));
        gameControlButtons.Add(GameObject.Find("CheatButton"));

        if (ShowTutorial)
        {
            StartTutorial();
        }
    }

    public void run()
    {
        foreach (GoalAgent goal in Goals)
        {
            foreach (CivilCarAgent agent in CarAgents)
            {
                if (agent.Type == goal.GoalType)
                {
                    agent.StartMoving(goal.GetComponentInParent<IsoTransform>().Position);
                }
            }
        }
        StartCoroutine(StartCountdown(TimeLimit));
        StartCoroutine(WaitCarsStop());
    } // Level

    IEnumerator WaitCarsStop() // level
    {
        yield return new WaitUntil(() => currCountdownValueTenthSeconds <= 0 || !AreCarsMoving());

        if (AllCarsReachedGoal()) // Win
        {
            // add high score based on the time left (in seconds) and the budget left
            int score = AddHighScore(currCountdownValueTenthSeconds / 10.0f, Budget);
            
            // set parameters in the result info
            SetPlayerName(PlayerName);
            SetTimeAmountAndScore((int) Math.Round(TimeLimit - currCountdownValueTenthSeconds / 10), Budget, score);
            CivilGameManager.ToggleDialogDisplay(Dialog, "BadPanel", false);
            CivilGameManager.ToggleDialogDisplay(Dialog, "GoodPanel", true);
        }
        else // Lose
        {
            SetFailInfo("Make sure there are roads for all the cars to travel on to reach their destination within the time limit.");
            CivilGameManager.ToggleDialogDisplay(Dialog, "GoodPanel", false);
            CivilGameManager.ToggleDialogDisplay(Dialog, "BadPanel", true);
            // if there are cars not stopped yet, make them stop
            foreach (var carAgent in CarAgents)
            {
                carAgent.StopMoving();
            }
        }
        timerNotStopped = false;
        Dialog.enabled = !Dialog.enabled;
        // disable game control buttons
        ToggleGameControlButtons(false);
    }

    private int AddHighScore(float timeLeft, int budget) // Level
    {
        float timeLeftPortion = timeLeft / IdealTimeLeft;
        float budgetLeftPortion = budget / (float) IdealBudgetLeft;

        float timeScore = timeLeftPortion * TimeMaxScore;
        float budgetScore = budgetLeftPortion * BudgetMaxScore;

        int highScore =(int) Math.Round(timeScore + budgetScore + CompletionBaseScore);
        highScore = highScore > 100 ? 100 : highScore;
        CivilGameManager.instance.AddScore(highScore, ThisLevel);

        return highScore;
    }

    private bool AreCarsMoving() // Level
    {
     
        foreach (CivilCarAgent agent in CarAgents)
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
        foreach (CivilCarAgent agent in CarAgents)
        {
            if (!agent.hasReachedGoal)
            {
                return false;
            }
        }

        return true;
    } // Level

    private void SetTimeAmountAndScore(int timeInSeconds, int amount, int score)
    {
        TextMeshProUGUI resultInfoArea = GameObject.Find("ResultInfo").GetComponent<TextMeshProUGUI>();

        string text = resultInfoArea.text;

        text = text.Replace("<time>", timeInSeconds.ToString() + (timeInSeconds == 1 ? " second" : " seconds"));
        text = text.Replace("<amount>", amount.ToString());
        text = text.Replace("<score>", score.ToString());

        resultInfoArea.SetText(text);
    }   // Level

    private void SetPlayerName(string name)
    {
        TextMeshProUGUI nameArea = GameObject.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        string text = nameArea.text;
        text = text.Replace("<name>", name);
        nameArea.SetText(text);
    }   // Level, TODO rename

    private void SetFailInfo(string failInfo) // Level
    {
        TextMeshProUGUI failInfoArea = GameObject.Find("FailInfo").GetComponent<TextMeshProUGUI>();
        failInfoArea.SetText(failInfo);
    }

    IEnumerator StartCountdown(int timeLimit)
    {
        float countdownValue = (TimeLimit - 1) * 10;
        currCountdownValueTenthSeconds = countdownValue;
        while (currCountdownValueTenthSeconds >= 0 && timerNotStopped)
        {
            string timerLabel = String.Format("{0:00}:{1:00}", Math.Floor((currCountdownValueTenthSeconds) / 10), (currCountdownValueTenthSeconds % 10) * 10);
            timerArea.text = timerLabel;
            yield return new WaitForSeconds(0.1f);
            currCountdownValueTenthSeconds--;
        }
    }   // Level

    public void UpdateBudget(int itemPrice)
    {
        Budget += itemPrice;
        budgetArea.text = "$" + Budget;
        UpdateBudgetAvailability();
    }   // Level

    private void UpdateBudgetAvailability() // Level
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

    public bool IsBudgetAvailable(int itemPrice)  // Level
    {
        return itemPrice <= Budget;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }   // Level

    public void ResetTimerAndCars()
    {
        // reset cars
        foreach (var carAgent in CarAgents)
        {
            carAgent.ResetCar();
        }
        
        // reset timer
        timerNotStopped = true;
        string timerLabel = String.Format("{0:00}:00", (TimeLimit));
        timerArea.text = timerLabel;
    }
    
    public void GoToLevel(int levelNumber)
    {
        SceneManager.LoadScene("Civil Level " + levelNumber);
    }

    public void Cheat()
    {
        SceneManager.LoadScene(CheatLevelName);
    }   // Level

    public void UndoCheat()
    {
        SceneManager.LoadScene(UndoCheatLevelName);
    } // Level

    public void CloseDialog()
    {
        // set the canvas to be disabled, but make the panels inside it to be both available
        Dialog.enabled = !Dialog.enabled;
        CivilGameManager.ToggleDialogDisplay(Dialog, "BadPanel", true);
        CivilGameManager.ToggleDialogDisplay(Dialog, "GoodPanel", true);
        // enable game control buttons
        ToggleGameControlButtons(true);
    }   // Level, TODO rename


    public void StartTutorial() // Super
    {
        // disable game control buttons
        ToggleGameControlButtons(false);
        Tutorial.gameObject.SetActive(true);
        tutorialSlideNumber = 1;
        for (int i = 1; i < TUTORIAL_SLIDE_COUNT + 1; i++)
        {
            CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + i, false);
        }
        CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + tutorialSlideNumber, true);
    }


    public void StopTutorial()
    {
        Tutorial.gameObject.SetActive(false);
        if (ShowAlienInfo)
        {
            DisplayAlienInfo();
        }
        // enable game control buttons
        ToggleGameControlButtons(true);
    }   // Super

    public void DisplayAlienInfo()
    {
        AlienInfo.enabled = true;
    }

    public void CloseAlienInfo()
    {
        // set canvas disabled to trigger OnMouseExit method on the close button's cursor styler instead of set the gameobject disabled
        AlienInfo.enabled = false;    
        // also set ShowAlienInfo to false so the AlienInfo won't popup until player restart level or go to the next level
        ShowAlienInfo = false;
    }

    public void NextTutorialSlide()
    {
        CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + tutorialSlideNumber, false);
        tutorialSlideNumber++;
        CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + tutorialSlideNumber, true);

    } // Super

    public void PreviousTutorialSlide()
    {
        CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + tutorialSlideNumber, false);
        tutorialSlideNumber--;
        CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + tutorialSlideNumber, true);
    }   // Super

    public void ToggleGameControlButtons(bool enable)
    {
        foreach (var button in gameControlButtons)
        {
            if (button != null)
            {
                button.GetComponent<Button>().interactable = enable;
                button.GetComponent<CursorStyler>().enabled = enable;
            }
        }
    }

    public void ExitCivilMiniGame()
    {
        CivilGameManager.instance.AddHighScore();
        GameManager.Instance.ChangeScene("Engineering Leech");
    }

}
