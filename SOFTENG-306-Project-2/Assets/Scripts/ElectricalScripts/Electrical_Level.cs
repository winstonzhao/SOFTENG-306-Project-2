using System.Collections;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Electrical_Level : MonoBehaviour, IDropZone
{
    public Sprite newSprite;
    private Draggable currentItem;
    public Draggable[] expectedGates;
    public Canvas endOfLevelCanvas;
    public int levelNumber;

    public int TimeLimit = 10;
    public int TimeMaxScore = 30;
    public int CompletionBaseScore = 20;

    private string PlayerName = "Anonymous";
    private TextMeshProUGUI timerArea;
    private bool timerNotStopped = true;

    private float currCountdownValueTenthSeconds;
    private float pausedValue;
    private bool paused = false;
    private bool expected = false;
    private bool secondDropZoneComplete = false;
    private bool levelCompleteCalled = false;
    private int tutorialLevelNumber = 4;
    private int levelScore = 100;


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
    }

    // Update is called once per frame
    void Update()
    {
        // check that a second drop zone exists
        if (GameObject.FindWithTag("DropZone2") == null)
        {
            // if the second dropzone doesn't exist set to true
            secondDropZoneComplete = true;
        }
        else
        {
            // check the status of the second dropzone
            secondDropZoneComplete = GameObject.FindWithTag("DropZone2").GetComponent<Second_DropZone>().GetExpected();
        }

        if (expected && secondDropZoneComplete)
        {
            if (!levelCompleteCalled)
            {
                levelCompleteCalled = true;
                completeLevel();
            }
            
        }
        // get paused value as the current countdown time
        pausedValue = currCountdownValueTenthSeconds;

        // check if the timer is deactivated
        if (timerArea.color == Color.gray) {
            timerNotStopped = false;
            paused = true;

            // stop all current timing
            StopAllCoroutines();

        } else if (timerArea.color == Color.black) {
            // check that the player is exiting a tutorial
            if (paused == true) {
                paused = false;
                timerNotStopped = true;

                // start new timing using current timer value on new background thread
                StartCoroutine(StartCountdown(Mathf.RoundToInt(1+ (pausedValue/10))));
            }
        }
    }

    public void OnDragEnter(Draggable item)
    {
       // GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
    }

    public void OnDragExit(Draggable item)
    {
     //   GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
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
                if (secondDropZoneComplete)
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
        //GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
        GameObject.FindWithTag("Light").GetComponent<SpriteRenderer>().sprite = newSprite;

        GameObject[] circuitPieces = GameObject.FindGameObjectsWithTag("Circuit");

        foreach (GameObject circ in circuitPieces)
        {
            circ.GetComponent<SpriteRenderer>().color = Color.yellow;
        }

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
        if (timeRemain < TimeLimit/2) {
            // reduce the player score
            levelScore = 99 - (TimeLimit/2 - timeRemain);
        }
        // set the level score in the end level dialog
        SetLevelScoreText(levelScore);
        Toolbox.Instance.Electrical_Scores.addScore(levelNumber, levelScore);
    }

    public void skipLevels()
    {
        for (int i = 1; i <= tutorialLevelNumber; i++)
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
        timerArea = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        string timerLabel = String.Format("{0:00}:00", (TimeLimit));
        timerArea.SetText(timerLabel);
    }

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
        nameArea.SetText(firstName[0]+"!");
    }

    /*
     * Set the level score for the end of level dialog
     */
    private void SetLevelScoreText(int score)
    {
        TextMeshProUGUI nameArea = GameObject.Find("Score_Text").GetComponent<TextMeshProUGUI>();
        nameArea.SetText(score + "");
    }

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
