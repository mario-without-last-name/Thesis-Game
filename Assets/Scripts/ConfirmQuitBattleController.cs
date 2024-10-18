using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;

public class ConfirmQuitBattleController : MonoBehaviour
{
    [Header("Game Over Status")]
    [SerializeField] private TextMeshProUGUI textOfExitBattleButton;
    [Header("Controllers")]
    [SerializeField] private MusicController musicController;
    [SerializeField] private BattleModeController battleModeController;

    private bool exitBattleButtonHasBeenClicked;
    private bool quitBattleManuallyAndNotFrom0Hp;

    private void Start()
    {
        textOfExitBattleButton.text = "Quit";
        exitBattleButtonHasBeenClicked = false;
        quitBattleManuallyAndNotFrom0Hp = false;
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
            Invoke(nameof(ButtonCancel), 5.0f);
        }
        else // When clicked within 5 seconds
        {
            exitBattleButtonHasBeenClicked = false;
            quitBattleManuallyAndNotFrom0Hp = true;
            CancelInvoke(nameof(ButtonCancel));
            battleModeController.BattleModeChanger("GameOver");
        }
    }

    public void ButtonCancel()
    {
        textOfExitBattleButton.text = "Quit";
        exitBattleButtonHasBeenClicked = false;
    }

    public bool GetQuitBattleManuallyAndNotFrom0Hp()
    {
        return quitBattleManuallyAndNotFrom0Hp;
    }
}
