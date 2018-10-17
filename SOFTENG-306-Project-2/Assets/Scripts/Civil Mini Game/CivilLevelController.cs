using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using Ultimate_Isometric_Toolkit.Scripts.Pathfinding;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CivilLevelController : MonoBehaviour
{
    /**
     * Cars and goals
     */
    public List<CivilCarAgent> CarAgents = new List<CivilCarAgent>();
    public List<GoalAgent> Goals = new List<GoalAgent>();

    /**
     * Canvases
     */
    public Canvas Dialog; // contains dialogs showing win or lose messages
    public Canvas Tutorial; // contains tutorial slides
    public Canvas AlienInfo; // the pop up info beside the alien (the button for opening tutorial)

    /**
     * Game and scoring values
     */
    public float TimeLimit = 10f; // max time allowed for cars to reach their goals
    public int Budget = 1000; // max budget allowed for building roads
    public float IdealTimeLeft; // the estimated time left for the ideal solution
    public int IdealBudgetLeft; // the estimated budget left for the ideal solution
    public int BudgetMaxScore = 50; // the scoring weight (out of 100) for the budget
    public int TimeMaxScore = 30; // the scoring weight (out of 100) for the time
    public int CompletionBaseScore = 20; // the score to be given to the player for completing the level

    /**
     * Level identity and interactions
     */
    public int ThisLevel; // this level number

    public string
        CheatLevelName; // the name for the cheating version of this level (fill if this level is not the cheating level)

    public string
        UndoCheatLevelName; // the name for the raw version of this level (fill if this level is the cheating level)

    /**
     * Tutorial and pop up info showing settings
     */
    public bool ShowTutorial = false; // set to true to make the tutorial showing automatically
    public bool ShowAlienInfo = false; // set to true to make the alien pop up info box showing automatically

    /**
     * Player info
     */
    private string _playerName;

    /**
     * Labels
     */
    private TextMesh _timerArea; // timer
    private TextMesh _budgetArea; // budget
    private TextMeshProUGUI _resultInfoArea; // winning dialog main body
    private TextMeshProUGUI _nameArea; // winning dialog player name label
    private TextMeshProUGUI _failInfoArea; // losing dialog main body

    /**
     * For controlling tutorial slides
     */
    private int _tutorialSlideNumber; // current tutorial slide
    private const int TUTORIAL_SLIDE_COUNT = 9;

    /**
     * Timer
     */
    private bool _timerNotStopped = true;
    private float _currCountdownValueTenthSeconds;

    /**
     * Tile factories
     */
    private GameObject[] _tileFactories;

    /**
     * Game control buttons
     */
    private List<GameObject> gameControlButtons = new List<GameObject>();

    private void Awake()
    {
        // get the player name
        _playerName = CivilGameManager.Instance.PlayerName;

        // get the winning dialog message text meshes
        _resultInfoArea = GameObject.Find("ResultInfo").GetComponent<TextMeshProUGUI>();
        _nameArea = GameObject.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        _failInfoArea = GameObject.Find("FailInfo").GetComponent<TextMeshProUGUI>();

        // get the tile factories
        _tileFactories = GameObject.FindGameObjectsWithTag("TileFactory");

        // initialise the labels
        // initialise time display
        _timerArea = GameObject.Find("Timer").GetComponent<TextMesh>();
        string timerLabel = String.Format("{0:00}:00", (TimeLimit));
        _timerArea.text = timerLabel;

        // initialise budget display
        _budgetArea = GameObject.Find("Budget").GetComponent<TextMesh>();
        _budgetArea.text = "$" + Budget;

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
        foreach (var goal in Goals)
        {
            foreach (var agent in CarAgents)
            {
                if (agent.Type == goal.GoalType)
                {
                    agent.StartMoving(goal.GetComponentInParent<IsoTransform>().Position);
                }
            }
        }

        // start the timer
        StartCoroutine(StartCountdown());
        // create a thread for detecting cars stopped or timer reached zero
        StartCoroutine(WaitCarsStop());
    }

    /**
     * Detect cars stopped or timer reached zero. In either case, test has all cars reached Goal. If all reached,
     * display the winning dialog and send the high score, otherwise show the losing dialog. Then stop timer.
     */
    IEnumerator WaitCarsStop()
    {
        yield return new WaitUntil(() => _currCountdownValueTenthSeconds <= 0 || !AreCarsMoving());

        if (AllCarsReachedGoal()) // Win
        {
            // add high score based on the time left (in seconds) and the budget left
            int score = AddHighScore(_currCountdownValueTenthSeconds / 10.0f, Budget);

            // set parameters in the result info
            SetPlayerName(_playerName);
            SetTimeAmountAndScore((int) Math.Round(TimeLimit - _currCountdownValueTenthSeconds / 10), Budget, score);
            CivilGameManager.ToggleDialogDisplay(Dialog, "BadPanel", false);
            CivilGameManager.ToggleDialogDisplay(Dialog, "GoodPanel", true);
        }
        else // Lose
        {
            // set the message to be displayed
            SetFailInfo(
                "Make sure there are roads for all the cars to travel on to reach their destination within the time limit.");
            CivilGameManager.ToggleDialogDisplay(Dialog, "GoodPanel", false);
            CivilGameManager.ToggleDialogDisplay(Dialog, "BadPanel", true);

            // if there are cars not stopped yet, make them stop
            foreach (var carAgent in CarAgents)
            {
                carAgent.StopMoving();
            }
        }

        // stop the timer
        _timerNotStopped = false;
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
        float timeLeftPortion =
            IdealTimeLeft == 0 ? 1 : timeLeft / IdealTimeLeft; // compare actual time left with the ideal value
        float budgetLeftPortion =
            IdealBudgetLeft == 0
                ? 1
                : budget / (float) IdealBudgetLeft; // compare actual budget left with the ideal value

        float timeScore = timeLeftPortion * TimeMaxScore; // scale the time score
        float budgetScore = budgetLeftPortion * BudgetMaxScore; // scale the budget score

        int highScore = (int) Math.Round(timeScore + budgetScore + CompletionBaseScore);
        highScore = highScore > 100 ? 100 : highScore; // don't allow the score to be over 100
        CivilGameManager.Instance.AddScore(highScore, ThisLevel);

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
        string text = _resultInfoArea.text;

        text = text.Replace("<time>", timeInSeconds.ToString() + (timeInSeconds == 1 ? " second" : " seconds"));
        text = text.Replace("<amount>", amount.ToString());
        text = text.Replace("<score>", score.ToString());

        _resultInfoArea.SetText(text);
    }

    /**
     * Set the player name label on the winning dialog.
     */
    private void SetPlayerName(string playerName)
    {
        var text = _nameArea.text;
        text = text.Replace("<name>", playerName);
        _nameArea.SetText(text);
    }

    /**
     * Set the message body on the losing dialog
     */
    private void SetFailInfo(string failInfo)
    {
        _failInfoArea.SetText(failInfo);
    }

    /**
     * Run the timer and update the timer label every 0.1s.
     * 
     */
    private IEnumerator StartCountdown()
    {
        // initialise counter
        float countdownValue = (TimeLimit - 1) * 10;
        _currCountdownValueTenthSeconds = countdownValue;
        // decrement counter if counter has not reached 0 and the timer has not been stopped
        while (_currCountdownValueTenthSeconds >= 0 && _timerNotStopped)
        {
            // update timer label
            string timerLabel = String.Format("{0:00}:{1:00}", Math.Floor((_currCountdownValueTenthSeconds) / 10),
                (_currCountdownValueTenthSeconds % 10) * 10);
            _timerArea.text = timerLabel;

            // wait for 0.1s and decrement counter
            yield return new WaitForSeconds(0.1f);
            _currCountdownValueTenthSeconds--;
        }
    }

    /**
     * Update the budget. New budget = old budget + item price. Item price can be either positive or negative.
     */
    public void UpdateBudget(int itemPrice)
    {
        Budget += itemPrice;
        _budgetArea.text = "$" + Budget;

        // update budget availability for the building blocks
        UpdateBudgetAvailability();
    }

    /**
     * Check budget availabilities for the building blocks. Disable any factory that has item price over the budget
     * left.
     */
    private void UpdateBudgetAvailability() // Level
    {
        foreach (GameObject tileFactory in _tileFactories)
        {
            // check is budget enough for the factory to build the tile
            IsoDropZone isoDropZone = tileFactory.GetComponent<IsoDropZone>();
            isoDropZone.SetEnable(IsBudgetAvailable(isoDropZone.ItemPrice));
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
        _timerNotStopped = true;
        string timerLabel = String.Format("{0:00}:00", (TimeLimit));
        _timerArea.text = timerLabel;
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
    }


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
        _tutorialSlideNumber = 1;
        for (int i = 1; i < TUTORIAL_SLIDE_COUNT + 1; i++)
        {
            CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + i, false);
        }

        CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + _tutorialSlideNumber, true);
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
        CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + _tutorialSlideNumber, false);
        _tutorialSlideNumber++;
        CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + _tutorialSlideNumber, true);
    }

    /**
     * Go to previous tutorial slide
     */
    public void PreviousTutorialSlide()
    {
        CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + _tutorialSlideNumber, false);
        _tutorialSlideNumber--;
        CivilGameManager.ToggleDialogDisplay(Tutorial, "Slide" + _tutorialSlideNumber, true);
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
        CivilGameManager.Instance.AddHighScore();
        GameManager.Instance.ChangeScene("Engineering Leech");
    }
}