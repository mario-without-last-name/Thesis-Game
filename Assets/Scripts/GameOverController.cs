using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI modeDifficulty;
    [SerializeField] private TextMeshProUGUI roundNumber;
    [SerializeField] private TextMeshProUGUI moveCount;
    [SerializeField] private TextMeshProUGUI killCount;
    [SerializeField] private GameObject timeHeaderAndValue;
    [SerializeField] private TextMeshProUGUI timeDisplay;
    [Header("Controllers")]
    [SerializeField] private MusicController musicController;
    [SerializeField] private SideBarController sideBarController;
    [SerializeField] private PlayerAndEnemyStatusController playerAndEnemyStatusController;
    [SerializeField] private GenerateStatisticsController generateStatisticsController;

    public void ChangeTextOf5GameOverStatistics()
    {
        modeDifficulty.text = PlayerPrefs.GetString("modeDifficulty", "???");
        roundNumber.text = "" + playerAndEnemyStatusController.GetRoundNumber();
        moveCount.text = "" + Mathf.Max(playerAndEnemyStatusController.GetTotalMoveCount(),0);
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

    public void RetryBattle() // Called from a UI button
    {
        musicController.PlayClickSoundEffect();
        SceneManager.LoadSceneAsync("SceneFight"); // Does the same thing as the code below in this scenario
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Load the currently active scene, which will be SceneFight
    }

    public void GoBackToMainMenu() // Called from a UI button
    {
        SceneManager.LoadSceneAsync("SceneMainMenu");
    }

    public void GenerateStatistics() // Called from a UI button
    {
        musicController.PlayClickSoundEffect();
        generateStatisticsController.ShowGenerateStatisticsScreen();
    }
}
