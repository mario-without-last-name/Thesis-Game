using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private MusicController musicController;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI modeDifficulty;
    [SerializeField] private TextMeshProUGUI roundNumber;
    [SerializeField] private TextMeshProUGUI moveCount;
    [SerializeField] private TextMeshProUGUI killCount;
    [SerializeField] private GameObject timeHeaderAndValue;
    [SerializeField] private TextMeshProUGUI timeDisplay;
    [Header("Other Controllers")]
    [SerializeField] private SideBarController sideBarController;
    [SerializeField] private PlayerAndEnemyStatusController playerAndEnemyStatusController;

    public void ChangeTextOf5GameOverStatistics()
    {
        modeDifficulty.text = PlayerPrefs.GetString("modeDifficulty", "???");
        roundNumber.text = "" + playerAndEnemyStatusController.GetRoundNumber();
        moveCount.text = "" + playerAndEnemyStatusController.GetMoveCount();
        killCount.text = "" + playerAndEnemyStatusController.GetKillCount();
        if (PlayerPrefs.GetInt("isTimerChecked", 0) == 1)
        {
            timeHeaderAndValue.SetActive(true);
        }
        else
        {
            timeHeaderAndValue.SetActive(false);
        }
        timeDisplay.text = sideBarController.GetSideBarTimerValue();
    }

    public void RetryBattle()
    {
        musicController.PlayClickSoundEffect();
        SceneManager.LoadSceneAsync("SceneFight"); // Does the same thing as the code below in this scenario
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Load the currently active scene, which will be SceneFight
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadSceneAsync("SceneMainMenu");
    }

    public void GenerateStatistics()
    {
        musicController.PlayClickSoundEffect();
        // Somehow get statistics
    }
}
