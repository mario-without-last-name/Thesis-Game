using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;

public class SideBarController : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI roundNumber;
    [SerializeField] private TextMeshProUGUI enemyCount;
    [SerializeField] private TextMeshProUGUI modeDifficulty;
    [SerializeField] private TextMeshProUGUI healthPoint;
    [SerializeField] private TextMeshProUGUI goldCount;
    [SerializeField] private GameObject timeHeaderAndValue;
    [SerializeField] private TextMeshProUGUI timeDisplay;

    private int roundNumberValue;
    private int currEnemiesLeftValue;
    private int totalEnemiesThisRoundValue;
    private int currPlayerHealthPointValue;
    private int maxPlayerHealthPointValue;
    private int currGoldValue;

    private bool isTimerRunning;
    private TimeSpan elapsedTime;  // To track total elapsed time
    private float timer; // Used to increment every second

    // Start is called before the first frame update
    void Start()
    {
        SetSideBarRoundNumber(1);
        modeDifficulty.text = PlayerPrefs.GetString("modeDifficulty", "???");

        if (PlayerPrefs.GetInt("isTimerChecked", 0) == 1)
        {
            timeHeaderAndValue.SetActive(true);
        }
        else
        {
            timeHeaderAndValue.SetActive(false);
        }

        elapsedTime = TimeSpan.Zero;  // Timer starts at 00:00:00
        timeDisplay.text = "00:00:00";
        isTimerRunning = false;
        timer = 0f;  // Initialize timer for seconds accumulation
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;  // Add the time passed since the last frame
            if (timer >= 1f)  // If one second or more has passed
            {
                elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));  // Increment elapsed time by one second
                timeDisplay.text = elapsedTime.ToString(@"hh\:mm\:ss");  // Update the timer display
                timer = 0f;  // Reset the timer to accumulate next second
            }
        }
    }

    // GETTERS AND SETTERS, but maybe the gettersshould not be here? This script is only for the sidebar text display?

    //public int GetSideBarRoundNumber()
    //{
    //    return roundNumberValue;
    //}
    public void SetSideBarRoundNumber(int newInt)
    {
        roundNumberValue = newInt;
        roundNumber.text = roundNumberValue.ToString();
    }

    //public int GetSideBarCurrEnemiesLeftValue()
    //{
    //    return currEnemiesLeftValue;
    //}
    public void SetSideBarCurrEnemiesLeftValue(int newInt)
    {
        currEnemiesLeftValue = newInt;
        enemyCount.text = currEnemiesLeftValue.ToString() + "/" + totalEnemiesThisRoundValue.ToString();
    }

    //public int GetSideBarTotalEnemiesThisRoundValue()
    //{
    //    return totalEnemiesThisRoundValue;
    //}
    public void SetSideBarTotalEnemiesThisRoundValue(int newInt)
    {
        totalEnemiesThisRoundValue = newInt;
        enemyCount.text = currEnemiesLeftValue.ToString() + "/" + totalEnemiesThisRoundValue.ToString();
    }

    //public int GetSideBarcurrPlayerHealthPointValue()
    //{
    //    return currPlayerHealthPointValue;
    //}
    public void SetSideBarCurrPlayerHealthPointValue(int newInt)
    {
        currPlayerHealthPointValue = newInt;
        healthPoint.text = currPlayerHealthPointValue.ToString() + "/" + maxPlayerHealthPointValue.ToString();
    }

    //public int GetSideBarmaxPlayerHealthPointValue()
    //{
    //    return maxPlayerHealthPointValue;
    //}
    public void SetSideBarMaxPlayerHealthPointValue(int newInt)
    {
        maxPlayerHealthPointValue = newInt;
        healthPoint.text = currPlayerHealthPointValue.ToString() + "/" + maxPlayerHealthPointValue.ToString();
    }

    //public int GetSideBarCurrGoldValue()
    //{
    //    return currGoldValue;
    //}
    public void SetSideBarCurrGoldValue(int newInt)
    {
        currGoldValue = newInt;
        goldCount.text = "$ " + currGoldValue.ToString();
    }

    //public bool GetSideBarIsTimerRunning()
    //{
    //    return isTimerRunning;
    //}
    public void SetSideBarIsTimerRunning(bool newBool)
    {
        isTimerRunning = newBool;
    }

    public string GetSideBarTimerValue()
    {
        return timeDisplay.text;
    }

}