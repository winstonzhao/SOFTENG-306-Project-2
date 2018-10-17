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

public class Timing : MonoBehaviour
{
    public int TimeLimit = 10;
    public int TimeMaxScore = 30;
    public int CompletionBaseScore = 20;

    private string PlayerName = "Anonymous";
    private TextMeshProUGUI timerArea;
    private bool timerNotStopped = true;

    private float currCountdownValueTenthSeconds;

    private void Awake()

    {
        timerArea = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        string timerLabel = String.Format("{0:00}:00", (TimeLimit));
        timerArea.SetText(timerLabel);
    }


    public void Start()
    {
        StartCoroutine(StartCountdown(TimeLimit));
    }

    private void AddHighScore(float timeLeft, int budget)
    {
        float timeLeftPortion = timeLeft / (float)(TimeLimit * 10);
        Debug.Log(timeLeftPortion);

        float timeScore = timeLeftPortion * TimeMaxScore;
        Debug.Log(timeScore);

        Score score = new Score();
        score.Minigame = Minigames.Civil;
        score.Value = timeScore + CompletionBaseScore;
        score.CreatedAt = DateTime.Now;
        Debug.Log(DateTime.Now);

        Debug.Log(score);
        Toolbox.Instance.Hiscores.Add(score);
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
