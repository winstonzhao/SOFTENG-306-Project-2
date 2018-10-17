using System.Collections;
using UnityEngine;
using TMPro;
using System;

/*
 * A controller for the electrical minigame, includes logic for the draggable
 * objects and the dropzones, timer, and highscore features
 */ 
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Electrical_Level : MonoBehaviour, IDropZone
{
    public Sprite newSprite;
    public Draggable[] expectedGates;
    public Canvas endOfLevelCanvas;
    private Draggable currentItem;

    public int levelNumber;
    public int TimeLimit = 10;
    private int levelScore = 100;

    private TextMeshProUGUI timerArea;
    private bool timerNotStopped = true;
    private bool paused = false;
    private bool expected = false;
    private bool otherDropZonesComplete = false;
    private bool levelCompleteCalled = false;
    private float currCountdownValueTenthSeconds;
    private float pausedValue;
    private GameObject[] dropZones;

    /*
     * Start called at beginning of level, initialises the rigidbodies in the
     * scene and player name, and begins countdown
     */
    public void Start()
    {
        var rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
        //timerArea.color = Color.black;
        StartCoroutine(StartCountdown(TimeLimit));

        SetPlayerName(Toolbox.Instance.GameManager.Player.Username);

        // check that another dropZone exists
        if (GameObject.FindWithTag("DropZone2") == null)
        {
            // if the second dropzone doesn't exist set to true
            otherDropZonesComplete = true;
        }
        else
        {
            dropZones = GameObject.FindGameObjectsWithTag("DropZone2");
        }
    }

    /*
     * Update called once per frame
     */
    void Update()
    {
        // if dropzones are currently incomplete iterate through to check
        if (!otherDropZonesComplete)
        {
            // set completed to true
            bool completedDropZones = true;
            foreach (GameObject dropZone in dropZones)
            {
                // if a drop zone returns false set completed to false
                if (!dropZone.GetComponent<Second_DropZone>().GetExpected())
                {
                    completedDropZones = false;
                }
            }

            // if no dropzone is incomplete, set true
            if (completedDropZones)
            {
                otherDropZonesComplete = true;
            }
        }

        // if the the circuit is in the correct state
        if (expected && otherDropZonesComplete)
        {
            // if level complete has not been called
            if (!levelCompleteCalled)
            {
                levelCompleteCalled = true;
                completeLevel();
            }

        }
        // get paused value as the current countdown time
        pausedValue = currCountdownValueTenthSeconds;

        // check if the timer is deactivated
        if (timerArea.color == Color.gray)
        {
            timerNotStopped = false;
            paused = true;

            // stop all current timing
            StopAllCoroutines();

        }
        else if (timerArea.color == Color.black)
        {
            // check that the player is exiting a tutorial
            if (paused == true)
            {
                paused = false;
                timerNotStopped = true;

                // start new timing using current timer value on new background thread
                StartCoroutine(StartCountdown(Mathf.RoundToInt(1 + (pausedValue / 10))));
            }
        }
    }


    public void OnDragEnter(Draggable item)
    {

    }

    public void OnDragExit(Draggable item)
    {
        expected = false;
        currentItem = null;

    }

    public void OnDragFinish(Draggable item)
    {
        if (!item.GetComponent<Collider2D>().IsTouching(GetComponent<Collider2D>()))
        {
            item.GetComponent<DraggableItemReturn>().SetDropZone(null);
        }
    }

    public void OnDragStart(Draggable item)
    {

    }

    public void OnDrop(Draggable item)
    {
        currentItem = item;
        currentItem.HomePos = transform.position;

        foreach (Draggable gate in expectedGates)
        {
            if (currentItem == gate)
            {
                if (otherDropZonesComplete)
                {
                    levelCompleteCalled = true;
                    completeLevel();
                }
                else
                {
                    expected = true;
                }
            }
        }

    }

    /*
     * Method that is called when the ciruit is completed correctly
     */
    private void completeLevel()
    {
        // find the light and circuit pieces in the level board
        GameObject.FindWithTag("Light").GetComponent<SpriteRenderer>().sprite = newSprite;
        GameObject[] circuitPieces = GameObject.FindGameObjectsWithTag("Circuit");

        // colour the circuit pieces yellow
        foreach (GameObject circ in circuitPieces)
        {
            circ.GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        // delay the time until the the end of level canvas is shown
        timerNotStopped = false;
        StartCoroutine(endOfLevel((delayTime) =>
        {
            if (delayTime)
            {
                endOfLevelCanvas.enabled = true;
            }
        }));

        // get time remaining
        int timeRemain = Mathf.RoundToInt(currCountdownValueTenthSeconds / 10);

        // if time remaining is less than 50% of time limit
        if (timeRemain < TimeLimit - 5)
        {
            // reduce the player score
            levelScore = 100 - (TimeLimit - timeRemain);
        }
        // set the level score in the end level dialog
        SetLevelScoreText(levelScore);
        Toolbox.Instance.Electrical_Scores.addScore(levelNumber, levelScore);
    }

    /*
     * Adds the scores from skipped levels to the high score board
     */
    public void skipLevels(int levels)
    {
        for (int i = 1; i <= levels; i++)
        {
            Toolbox.Instance.Electrical_Scores.addScore(i, levelScore);
        }
    }

    private IEnumerator endOfLevel(System.Action<bool> callback)
    {
        yield return new WaitForSeconds(2);
        callback(true);
    }

    public void OnItemDrag(Draggable item)
    {

    }

    public void OnItemRemove(Draggable item)
    {
        currentItem = null;
    }

    public bool CanDrop(Draggable item)
    {
        return true;
    }

    private void Awake()
    {
        // find the timer area and set the label to the countdown
        timerArea = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        string timerLabel = String.Format("{0:00}:00", (TimeLimit));
        timerArea.SetText(timerLabel);
    }

    /*
     * Set the timer to the amount of time specified
     */
    private void SetTimeAndAmount(int timeInSeconds, int amount)
    {
        TextMeshProUGUI resultInfoArea = GameObject.Find("ResultInfo").GetComponent<TextMeshProUGUI>();

        string text = resultInfoArea.text;

        text = text.Replace("<time>", timeInSeconds.ToString() + (timeInSeconds == 1 ? " second" : " seconds"));
        text = text.Replace("<amount>", amount.ToString());

        resultInfoArea.SetText(text);
    }

    /*
     * Set the player name in end level dialog
     */
    private void SetPlayerName(string name)
    {
        string[] firstName = name.Split(null);
        TextMeshProUGUI nameArea = GameObject.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        nameArea.SetText(firstName[0] + "!");
    }

    /*
     * Set the level score for the end of level dialog
     */
    private void SetLevelScoreText(int score)
    {
        TextMeshProUGUI nameArea = GameObject.Find("Score_Text").GetComponent<TextMeshProUGUI>();
        nameArea.SetText(score + "");
    }

    /*
     * Begin the timer and refresh the timer text every 0.1 seconds
     */
    public IEnumerator StartCountdown(int timeLimit)
    {
        float countdownValue = (timeLimit - 1) * 10;
        currCountdownValueTenthSeconds = countdownValue;
        while (currCountdownValueTenthSeconds >= 0 && timerNotStopped)
        {
            //Debug.Log((currCountdownValueTenthSeconds) / 10);
            string timerLabel = String.Format("{0:00}:{1:00}", Math.Floor((currCountdownValueTenthSeconds) / 10), (currCountdownValueTenthSeconds % 10) * 10);
            timerArea.SetText(timerLabel);
            yield return new WaitForSeconds(0.1f);
            currCountdownValueTenthSeconds--;
        }
    }

    /**
     * Find an object in a parent object including parent objects
     */
    public GameObject FindObject(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }

}
