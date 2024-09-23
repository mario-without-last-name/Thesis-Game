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
    [SerializeField] private SideBarController SideBarController;
    [SerializeField] private PlayerAndEnemyStatusController PlayerAndEnemyStatusController;

    public void ChangeTextOf5GameOverStatistics()
    {
        modeDifficulty.text = PlayerPrefs.GetString("modeDifficulty", "???");
        roundNumber.text = "999"; // Later add this dynamically
        moveCount.text = "999"; // Later add this dynamically
        killCount.text = "999"; // Later add this dynamically
        if (PlayerPrefs.GetInt("isTimerChecked", 0) == 1)
        {
            timeHeaderAndValue.SetActive(true);
        }
        else
        {
            timeHeaderAndValue.SetActive(false);
        }
        timeDisplay.text = SideBarController.GetSideBarTimerValue();
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

    }
}
