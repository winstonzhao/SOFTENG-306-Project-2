using System.Collections;
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

    /**
     * Cars and goals
     */
    public List<CivilCarAgent> CarAgents = new List<CivilCarAgent>();
    public List<GoalAgent> Goals = new List<GoalAgent>();
    
    /**
     * Canvases
     */ 
    public Canvas Dialog;  // contains dialogs showing win or lose messages
    public Canvas Tutorial;  // contains tutorial slides
    public Canvas AlienInfo;  // the pop up info beside the alien (the button for opening tutorial)
    
    /**
     * Game and scoring values
     */
    public float TimeLimit = 10f;  // max time allowed for cars to reach their goals
    public int Budget = 1000;  // max budget allowed for building roads
    public float IdealTimeLeft;  // the estimated time left for the ideal solution
    public int IdealBudgetLeft;  // the estimated budget left for the ideal solution
    public int BudgetMaxScore = 50;  // the scoring weight (out of 100) for the budget
    public int TimeMaxScore = 30;  // the scoring weight (out of 100) for the time
    public int CompletionBaseScore = 20; // the score to be given to the player for completing the level

    /**
     * Level identity and interactions
     */
    public int ThisLevel;  // this level number
    public string CheatLevelName;  // the name for the cheating version of this level (fill if this level is not the cheating level)
    public string UndoCheatLevelName;  // the name for the raw version of this level (fill if this level is the cheating level)
    
    /**
     * Tutorial and pop up info showing settings
     */
    public bool ShowTutorial = false;  // set to true to make the tutorial showing automatically
    public bool ShowAlienInfo = false;  // set to true to make the alien pop up info box showing automatically
    
    /**
     * Player info
     */
    private string PlayerName;
    
    /**
     * Labels
     */
    private TextMesh timerArea;  // timer
    private TextMesh budgetArea;  // budget
    TextMeshProUGUI resultInfoArea;  // winning dialog main body
    TextMeshProUGUI nameArea;  // winning dialog player name label
    TextMeshProUGUI failInfoArea;  // losing dialog main body

    /**
     * For controlling tutorial slides
     */
    private int tutorialSlideNumber;  // current tutorial slide
    private const int TUTORIAL_SLIDE_COUNT = 9;
    
    /**
     * Timer
     */
    private bool timerNotStopped = true;
    private float currCountdownValueTenthSeconds;
    
    /**
     * Tile factories
     */
    private GameObject[] tileFactories;
    
    /**
     * Game control buttons
     */
    private List<GameObject> gameControlButtons = new List<GameObject>();

    private void Awake()
    {
        // get the player name
        PlayerName = CivilGameManager.instance.playerName;
        
        // get the winning dialog message text meshes
        resultInfoArea = GameObject.Find("ResultInfo").GetComponent<TextMeshProUGUI>();
        nameArea = GameObject.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        failInfoArea = GameObject.Find("FailInfo").GetComponent<TextMeshProUGUI>();
        
        // get the tile factories
        tileFactories = GameObject.FindGameObjectsWithTag("TileFactory");
        
        // initialise the labels
        // initialise time display
        timerArea = GameObject.Find("Timer").GetComponent<TextMesh>();
        string timerLabel = String.Format("{0:00}:00", (TimeLimit));
        timerArea.text = timerLabel;

        // initialise budget display
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

    /**
     * Call all cars to run to their goals, and start timer
     */
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
        // start the timer
        StartCoroutine(StartCountdown(TimeLimit));
        // create a thread for detecting cars stopped or timer reached zero
        StartCoroutine(WaitCarsStop());
    }

    /**
     * Detect cars stopped or timer reached zero. In either case, test has all cars reached Goal. If all reached,
     * display the winning dialog and send the high score, otherwise show the losing dialog. Then stop timer.
     */
    IEnumerator WaitCarsStop()
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
            // set the message to be displayed
            SetFailInfo("Make sure there are roads for all the cars to travel on to reach their destination within the time limit.");
            CivilGameManager.ToggleDialogDisplay(Dialog, "GoodPanel", false);
            CivilGameManager.ToggleDialogDisplay(Dialog, "BadPanel", true);
            
            // if there are cars not stopped yet, make them stop
            foreach (var carAgent in CarAgents)
            {
                carAgent.StopMoving();
            }
        }
        // stop the timer
        timerNotStopped = false;
        // make the dialog canvas visible
        Dialog.enabled = !Dialog.enabled;
        // disable game control buttons
        ToggleGameControlButtons(false);
    }

    /**
     * Calculate the high score and send to CivilGameManager.
     * Score = LT/ITL * TS + BT/IBL * BS + CS
     * Where LT = time left
     * ITL = ideal time left
     * TS = time max score (portion out of 100)
     * BL = budget left
     * IBL = ideal budget left
     * BS = budget max score (portion out of 100)
     * CS = completion score
     */
    private int AddHighScore(float timeLeft, int budget) // Level
    {
        float timeLeftPortion = IdealTimeLeft == 0 ? 1 : timeLeft / IdealTimeLeft;  // compare actual time left with the ideal value
        float budgetLeftPortion = IdealBudgetLeft == 0 ? 1 : budget / (float) IdealBudgetLeft;  // compare actual budget left with the ideal value

        float timeScore = timeLeftPortion * TimeMaxScore;  // scale the time score
        float budgetScore = budgetLeftPortion * BudgetMaxScore; // scale the budget score

        int highScore =(int) Math.Round(timeScore + budgetScore + CompletionBaseScore);
        highScore = highScore > 100 ? 100 : highScore;  // don't allow the score to be over 100
        CivilGameManager.instance.AddScore(highScore, ThisLevel);

        return highScore;
    }

    /**
     * Test is there any car still moving.
     * Return true if any car is still running.
     */
    private bool AreCarsMoving()
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

    /**
     * Test have all cars reached their corresponding goals.
     */
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
    }

    /**
     * Set the time, budget left, and score in the winning dialog.
     */
    private void SetTimeAmountAndScore(int timeInSeconds, int amount, int score)
    {
        string text = resultInfoArea.text;

        text = text.Replace("<time>", timeInSeconds.ToString() + (timeInSeconds == 1 ? " second" : " seconds"));
        text = text.Replace("<amount>", amount.ToString());
        text = text.Replace("<score>", score.ToString());

        resultInfoArea.SetText(text);
    }

    /**
     * Set the player name label on the winning dialog.
     */
    private void SetPlayerName(string name)
    {
        string text = nameArea.text;
        text = text.Replace("<name>", name);
        nameArea.SetText(text);
    }

    /**
     * Set the message body on the losing dialog
     */
    private void SetFailInfo(string failInfo)
    {
        failInfoArea.SetText(failInfo);
    }

    /**
     * Run the timer and update the timer label every 0.1s. The timeLimit which is the initial time for the
     * timer should be in seconds.
     * 
     */
    IEnumerator StartCountdown(float timeLimit)
    {
        // initialise counter
        float countdownValue = (TimeLimit - 1) * 10;
        currCountdownValueTenthSeconds = countdownValue;
        // decrement counter if counter has not reached 0 and the timer has not been stopped
        while (currCountdownValueTenthSeconds >= 0 && timerNotStopped)
        {
            // update timer label
            string timerLabel = String.Format("{0:00}:{1:00}", Math.Floor((currCountdownValueTenthSeconds) / 10), (currCountdownValueTenthSeconds % 10) * 10);
            timerArea.text = timerLabel;
            
            // wait for 0.1s and decrement counter
            yield return new WaitForSeconds(0.1f);
            currCountdownValueTenthSeconds--;
        }
    }

    /**
     * Update the budget. New budget = old budget + item price. Item price can be either positive or negative.
     */
    public void UpdateBudget(int itemPrice)
    {
        Budget += itemPrice;
        budgetArea.text = "$" + Budget;
        
        // update budget availability for the building blocks
        UpdateBudgetAvailability();
    } 

    /**
     * Check budget availabilities for the building blocks. Disable any factory that has item price over the budget
     * left.
     */
    private void UpdateBudgetAvailability() // Level
    {
        foreach (GameObject tileFactory in tileFactories)
        {
            // check is budget enough for the factory to build the tile
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

    /**
     * Check is the budget enough for the given item price
     */
    private bool IsBudgetAvailable(int itemPrice)
    {
        return itemPrice <= Budget;
    }

    /**
     * Reload the current scene.
     */
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /**
     * Reset the cars to their original positions and reinitialise the timer.
     */
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
    
    /**
     * Load the Civil Level scene for the given level number.
     */
    public void GoToLevel(int levelNumber)
    {
        SceneManager.LoadScene("Civil Level " + levelNumber);
    }

    /**
     * Load the cheating scene registered to this level controller
     */
    public void Cheat()
    {
        SceneManager.LoadScene(CheatLevelName);
    }

    /**
     * Load the raw scene register to this level controller
     */
    public void UndoCheat()
    {
        SceneManager.LoadScene(UndoCheatLevelName);
    }

    /**
     * Close the winning or losing dialogs and re-enable the control buttons
     */
    public void CloseDialog()
    {
        // set the canvas to be disabled, but make the panels inside it to be both available
        Dialog.enabled = !Dialog.enabled;
        CivilGameManager.ToggleDialogDisplay(Dialog, "BadPanel", true);
        CivilGameManager.ToggleDialogDisplay(Dialog, "GoodPanel", true);
        // enable game control buttons
        ToggleGameControlButtons(true);
    }   // Level, TODO rename


    /**
     * Make tutorial canvas visible and show the first slide of the tutorial.
     */
    public void StartTutorial()
    {
        // disable game control buttons
        ToggleGameControlButtons(false);
        
        // enable tutorial canvas
        Tutorial.gameObject.SetActive(true);
        
        // show slide one
        tutorialSlideNumber = 1;
        for (int i = 1; i < TUTORIAL_SLIDE_COUNT + 1; i++)
        {
            CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + i, false);
        }
        CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + tutorialSlideNumber, true);
    }

    /**
     * Close tutorial and re-enable control buttons
     */
    public void StopTutorial()
    {
        Tutorial.gameObject.SetActive(false);
        if (ShowAlienInfo)
        {
            DisplayAlienInfo();
        }
        // enable game control buttons
        ToggleGameControlButtons(true);
    }

    /**
     * Display the alien pop up info.
     */
    public void DisplayAlienInfo()
    {
        AlienInfo.enabled = true;
    }

    /**
     * Close the alien pop up info.
     */
    public void CloseAlienInfo()
    {
        // set canvas disabled to trigger OnMouseExit method on the close button's cursor styler instead of set the gameobject disabled
        AlienInfo.enabled = false;    
        // also set ShowAlienInfo to false so the AlienInfo won't popup until player restart level or go to the next level
        ShowAlienInfo = false;
    }

    /**
     * Go to next tutorial slide
     */
    public void NextTutorialSlide()
    {
        CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + tutorialSlideNumber, false);
        tutorialSlideNumber++;
        CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + tutorialSlideNumber, true);

    }

    /**
     * Go to previous tutorial slide
     */
    public void PreviousTutorialSlide()
    {
        CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + tutorialSlideNumber, false);
        tutorialSlideNumber--;
        CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + tutorialSlideNumber, true);
    }

    /**
     * Enable or disable game control buttons.
     */
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

    /**
     * Call the CiVilGameManager to calculate and add the high score for this game play to the main game.
     * And load the Engineering Leech scene.
     */
    public void ExitCivilMiniGame()
    {
        CivilGameManager.instance.AddHighScore();
        GameManager.Instance.ChangeScene("Engineering Leech");
    }

}
