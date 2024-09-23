using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;

public class QuitBattleButton : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private MusicController musicController;
    [Header("Game Over Status")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI textOfExitBattleButton;
    [Header("Other Controllers")]
    [SerializeField] private SideBarController sideBarController;
    [SerializeField] private GameOverController gameOverController;


    private bool exitBattleButtonHasBeenClicked;

    private void Start()
    {
        textOfExitBattleButton.text = "Quit";
        exitBattleButtonHasBeenClicked = false;
    }

    // CLICK ON "QUIT" BUTTON ONCE, ITS TEXT BECOMES "CONFIRM?" fOR 5 SECONDS
    // IF CLICKED WITHIN 5 SECONDS, THE GAMEOVER SCREEN POPS UP AND THE GAME ENDS
    // IF 5 SECONDS HAS PASSED, THE BUTTON TEXT GOES BACK TO "QUIT"

    public void ClickOnQuit() // the button when clicked will cal this function
    {
        if(!exitBattleButtonHasBeenClicked) // when clicked the first time or after the 5 second delay has passed
        {
            musicController.PlayClickSoundEffect();
            textOfExitBattleButton.text = "Confirm?";
            exitBattleButtonHasBeenClicked = true;
            Invoke("ButtonCancel", 5.0f);
        }
        else // When clicked within 5 seconds
        {
            musicController.PlayClickSoundEffect();
            musicController.SetBackgroundMusic(false);
            musicController.PlayGameOverSoundEffectSource();
            sideBarController.SetSideBarIsTimerRunning(false);
            gameOverController.ChangeTextOf5GameOverStatistics();
            gameOverScreen.SetActive(true);
            exitBattleButtonHasBeenClicked = false;
            CancelInvoke("ButtonCancel");
        }
    }

    public void ButtonCancel()
    {
        textOfExitBattleButton.text = "Quit";
        exitBattleButtonHasBeenClicked = false;
    }
}
