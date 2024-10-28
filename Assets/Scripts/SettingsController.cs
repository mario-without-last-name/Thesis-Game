using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("Setting Buttons")]
    [SerializeField] private Button buttonMusic;
    [SerializeField] private Button buttonSoundEffects;
    [SerializeField] private Button buttonTimer;
    [SerializeField] private Button buttonStats;
    [SerializeField] private Button buttonHints;
    [SerializeField] private Button buttonPowerup;
    [SerializeField] private Button buttonAI;
    [SerializeField] private Button buttonLimit;
    [Header("Setting Statuses")]
    [SerializeField] private bool isMusicChecked;
    [SerializeField] private bool isSoundEffectsChecked;
    [SerializeField] private bool isTimerChecked;
    [SerializeField] private bool isStatsChecked;
    [SerializeField] private bool isHintsChecked;
    [SerializeField] private bool isPowerupChecked;
    [SerializeField] private bool isAIChecked;
    [SerializeField] private bool isLimitChecked;
    [Header("Setting Images")]
    [SerializeField] private Image imageButtonMusic;
    [SerializeField] private Image imageButtonSoundEffects;
    [SerializeField] private Image imageButtonTimer;
    [SerializeField] private Image imageButtonStats;
    [SerializeField] private Image imageButtonHints;
    [SerializeField] private Image imageButtonPowerup;
    [SerializeField] private Image imageButtonAI;
    [SerializeField] private Image imageButtonLimit;
    [Header("Checkbox Images")]
    [SerializeField] private Sprite checkedSprite;
    [SerializeField] private Sprite uncheckedSprite;
    [Header("Controllers")]
    [SerializeField] private MusicController musicController;

    // IMPORTANT NOTE:
    // WHEN THE UNITY GAME IS BUILT, IT WILL BY DEFAULT TURN OFF ALL CHECKBOXES IN THE SETTINGS.
    // SO BEFORE SHARING THE GAME FILE TO OTHER PEOPLE, CHECK ALL THOSE BOXES FIRST (AFTER BUILDING IT)

    // Start is called before the first frame update
    private void Start()
    {
        imageButtonMusic        = buttonMusic.GetComponent<Image>();
        imageButtonSoundEffects = buttonSoundEffects.GetComponent<Image>();
        imageButtonTimer        = buttonTimer.GetComponent<Image>();
        imageButtonStats        = buttonStats.GetComponent<Image>();
        imageButtonHints        = buttonHints.GetComponent<Image>();
        imageButtonPowerup      = buttonPowerup.GetComponent<Image>();
        imageButtonAI           = buttonAI.GetComponent<Image>();
        imageButtonLimit        = buttonLimit.GetComponent<Image>();

        isMusicChecked        = PlayerPrefs.GetInt("isMusicChecked", 0) == 1;
        isSoundEffectsChecked = PlayerPrefs.GetInt("isSoundEffectsChecked", 0) == 1;
        isTimerChecked        = PlayerPrefs.GetInt("isTimerChecked", 0) == 1;
        isStatsChecked        = PlayerPrefs.GetInt("isStatsChecked", 0) == 1;
        isHintsChecked        = PlayerPrefs.GetInt("isHintsChecked", 0) == 1;
        isPowerupChecked      = PlayerPrefs.GetInt("isPowerupChecked", 0) == 1;
        isAIChecked           = PlayerPrefs.GetInt("isAIChecked", 0) == 1;
        isLimitChecked        = PlayerPrefs.GetInt("isLimitChecked", 0) == 1;

        UpdateButtonImage(buttonMusic,        isMusicChecked);
        UpdateButtonImage(buttonSoundEffects, isSoundEffectsChecked);
        UpdateButtonImage(buttonTimer,        isTimerChecked);
        UpdateButtonImage(buttonStats,        isStatsChecked);
        UpdateButtonImage(buttonHints,        isHintsChecked);
        UpdateButtonImage(buttonPowerup,      isPowerupChecked);
        UpdateButtonImage(buttonAI,           isAIChecked);
        UpdateButtonImage(buttonLimit,        isLimitChecked);
    }

    private void UpdateButtonImage(Button button, bool isChecked)
    {
        Image buttonImage = button.GetComponent<Image>();
        buttonImage.sprite = isChecked ? checkedSprite : uncheckedSprite;
    }

    public void ToggleButton(string buttonType)
    {
        musicController.PlayClickSoundEffect();

        if (buttonType == "music")
        {
            isMusicChecked = !isMusicChecked;
            PlayerPrefs.SetInt("isMusicChecked", isMusicChecked ? 1 : 0);
            UpdateButtonImage(buttonMusic, isMusicChecked);
            musicController.SetBackgroundMusic(isMusicChecked);
        }
        else if (buttonType == "soundEffects")
        {
            isSoundEffectsChecked = !isSoundEffectsChecked;
            PlayerPrefs.SetInt("isSoundEffectsChecked", isSoundEffectsChecked ? 1 : 0);
            UpdateButtonImage(buttonSoundEffects, isSoundEffectsChecked);
            musicController.SetSoundEffects(isSoundEffectsChecked);
        }
        else if (buttonType == "timer")
        {
            isTimerChecked = !isTimerChecked;
            PlayerPrefs.SetInt("isTimerChecked", isTimerChecked ? 1 : 0);
            UpdateButtonImage(buttonTimer, isTimerChecked);
        }
        else if (buttonType == "stats")
        {
            isStatsChecked = !isStatsChecked;
            PlayerPrefs.SetInt("isStatsChecked", isStatsChecked ? 1 : 0);
            UpdateButtonImage(buttonStats, isStatsChecked);
        }
        else if (buttonType == "hints")
        {
            isHintsChecked = !isHintsChecked;
            PlayerPrefs.SetInt("isHintsChecked", isHintsChecked ? 1 : 0);
            UpdateButtonImage(buttonHints, isHintsChecked);
        }
        else if (buttonType == "powerup")
        {
            isPowerupChecked = !isPowerupChecked;
            PlayerPrefs.SetInt("isPowerupChecked", isPowerupChecked ? 1 : 0);
            UpdateButtonImage(buttonPowerup, isPowerupChecked);
        }
        else if (buttonType == "AI")
        {
            isAIChecked = !isAIChecked;
            PlayerPrefs.SetInt("isAIChecked", isAIChecked ? 1 : 0);
            UpdateButtonImage(buttonAI, isAIChecked);
        }
        else if (buttonType == "limit")
        {
            isLimitChecked = !isLimitChecked;
            PlayerPrefs.SetInt("isLimitChecked", isLimitChecked ? 1 : 0);
            UpdateButtonImage(buttonLimit, isLimitChecked);
        }

    }

}