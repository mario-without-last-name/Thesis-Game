using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour // KEEP THIS PUBLIC
{
    // ALL OF THESE ARE INSERTED INTO BUTTONS WITHIN THE SceneMainMenu SCENE,
    // TO TELL THEM WHAT "PAGE" THEY MUST DIRECT TO WHEN CLICKED

    [Header("Menu Pages")]
    [SerializeField] private GameObject background1;
    [SerializeField] private GameObject background2;
    [SerializeField] private GameObject temporaryBlueprint;
    [SerializeField] private GameObject pageMainMenu;
    [SerializeField] private GameObject pageConfirmExitGame;
    [SerializeField] private GameObject pageFight;
    [SerializeField] private GameObject pageGuide;
    [SerializeField] private GameObject pageHowToPlay1;
    [SerializeField] private GameObject pageHowToPlay2;
    [SerializeField] private GameObject pageHowToPlay3;
    [SerializeField] private GameObject pageHowToPlay4;
    [SerializeField] private GameObject pageHowToPlay5;
    [SerializeField] private GameObject pageBestiary1;
    [SerializeField] private GameObject pageBestiary2;
    [SerializeField] private GameObject pageBestiary3;
    [SerializeField] private GameObject pageBestiary4;
    [SerializeField] private GameObject pageBestiary5;
    [SerializeField] private GameObject pageBestiary6;
    [SerializeField] private GameObject pageBestiary7;
    [SerializeField] private GameObject pageResearchInfo1;
    [SerializeField] private GameObject pageResearchInfo2;
    [SerializeField] private GameObject pageResearchInfo3;
    [SerializeField] private GameObject pageResearchInfo4;
    [SerializeField] private GameObject pageSettings;
    [SerializeField] private GameObject pageSettingsGeneral;
    [SerializeField] private GameObject pageSettingsAdaptive;
    [Header("Controllers")]
    [SerializeField] private MusicController musicController;

    // Start is called before the first frame update
    private void Start()
    {
        MenuVisiblityChanger("mainMenu");
    }

    // FUNCTION TO ENABLE/DISABLE PAGES ==========================================================
    public void MenuVisiblityChanger(string pageToShow)
    {
        musicController.PlayClickSoundEffect();

        background1.SetActive(false);
        background2.SetActive(false);
        temporaryBlueprint.SetActive(false);
        pageMainMenu.SetActive(false);
        pageConfirmExitGame.SetActive(false);
        pageFight.SetActive(false);
        pageGuide.SetActive(false);
        pageHowToPlay1.SetActive(false);
        pageHowToPlay2.SetActive(false);
        pageHowToPlay3.SetActive(false);
        pageHowToPlay4.SetActive(false);
        pageHowToPlay5.SetActive(false);
        pageBestiary1.SetActive(false);
        pageBestiary2.SetActive(false);
        pageBestiary3.SetActive(false);
        pageBestiary4.SetActive(false);
        pageBestiary5.SetActive(false);
        pageBestiary6.SetActive(false);
        pageBestiary7.SetActive(false);
        pageResearchInfo1.SetActive(false);
        pageResearchInfo2.SetActive(false);
        pageResearchInfo3.SetActive(false);
        pageResearchInfo4.SetActive(false);
        pageSettings.SetActive(false);
        pageSettingsGeneral.SetActive(false);
        pageSettingsAdaptive.SetActive(false);

        if (pageToShow == "mainMenu")
        {
            background1.SetActive(true);
            pageMainMenu.SetActive(true);
        }
        else if (pageToShow == "confirmExitGame")
        {
            background1.SetActive(true);
            pageConfirmExitGame.SetActive(true);
        }
        else if (pageToShow == "fight")
        {
            background1.SetActive(true);
            pageFight.SetActive(true);
        }
        else if (pageToShow == "guide")
        {
            background1.SetActive(true);
            pageGuide.SetActive(true);
        }
        else if (pageToShow == "howToPlay1")
        {
            background2.SetActive(true);
            pageHowToPlay1.SetActive(true);
        }
        else if (pageToShow == "howToPlay2")
        {
            background2.SetActive(true);
            pageHowToPlay2.SetActive(true);
        }
        else if (pageToShow == "howToPlay3")
        {
            background2.SetActive(true);
            pageHowToPlay3.SetActive(true);
        }
        else if (pageToShow == "howToPlay4")
        {
            background2.SetActive(true);
            pageHowToPlay4.SetActive(true);
        }
        else if (pageToShow == "howToPlay5")
        {
            background2.SetActive(true);
            pageHowToPlay5.SetActive(true);
        }
        else if (pageToShow == "bestiary1")
        {
            background2.SetActive(true);
            pageBestiary1.SetActive(true);
        }
        else if (pageToShow == "bestiary2")
        {
            background2.SetActive(true);
            pageBestiary2.SetActive(true);
        }
        else if (pageToShow == "bestiary3")
        {
            background2.SetActive(true);
            pageBestiary3.SetActive(true);
        }
        else if (pageToShow == "bestiary4")
        {
            background2.SetActive(true);
            pageBestiary4.SetActive(true);
        }
        else if (pageToShow == "bestiary5")
        {
            background2.SetActive(true);
            pageBestiary5.SetActive(true);
        }
        else if (pageToShow == "bestiary6")
        {
            background2.SetActive(true);
            pageBestiary6.SetActive(true);
        }
        else if (pageToShow == "bestiary7")
        {
            background2.SetActive(true);
            pageBestiary7.SetActive(true);
        }
        else if (pageToShow == "researchInfo1")
        {
            background2.SetActive(true);
            pageResearchInfo1.SetActive(true);
        }
        else if (pageToShow == "researchInfo2")
        {
            background2.SetActive(true);
            pageResearchInfo2.SetActive(true);
        }
        else if (pageToShow == "researchInfo3")
        {
            background2.SetActive(true);
            pageResearchInfo3.SetActive(true);
        }
        else if (pageToShow == "researchInfo4")
        {
            background2.SetActive(true);
            pageResearchInfo4.SetActive(true);
        }
        else if (pageToShow == "settings")
        {
            background1.SetActive(true);
            pageSettings.SetActive(true);
        }
        else if (pageToShow == "settingsGeneral")
        {
            background2.SetActive(true);
            pageSettingsGeneral.SetActive(true);
        }
        else if (pageToShow == "settingsAdaptive")
        {
            background2.SetActive(true);
            pageSettingsAdaptive.SetActive(true);
        }
        else
        {
            Debug.Log("Requested Menu Page Not Found, Check the string or check MenuController.cs ?");
        }
    }

    // GOING TO THE GAMEPLAY ==================================================================

    private void EnterFight(string difficulty) // "easy" / "medium" / "hard" / "adaptive" // SHOULD THIS BE PLAYER PREFS TOO?
    {
        PlayerPrefs.SetString("modeDifficulty", difficulty);
        Debug.Log("(Click on this console message to see more of the selected difficuly and settings)" +
            "\nDifficulty : " + PlayerPrefs.GetString("modeDifficulty", "???") +
            "\nMusic : " + PlayerPrefs.GetInt("isMusicChecked", 0) +
            "\nSound Effects : " + PlayerPrefs.GetInt("isSoundEffectsChecked", 0) +
            "\nShow Timer : " + PlayerPrefs.GetInt("isTimerChecked", 0) +
            "\nEnemy Stats : " + PlayerPrefs.GetInt("isStatsChecked", 0) +
            "\nVisual Hints : " + PlayerPrefs.GetInt("isHintsChecked", 0) +
            "\nPowerup Usage : " + PlayerPrefs.GetInt("isPowerupChecked", 0) +
            "\nEnemy AI : " + PlayerPrefs.GetInt("isAIChecked", 0) +
            "\nTime Limit : " + PlayerPrefs.GetInt("isLimitChecked", 0));
        SceneManager.LoadSceneAsync("SceneFight"); // Can also use LoadSceneAsync(1), a scene index as seen in the Build Settings
    }

    // EXIT ============================================================================
    private void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // For Unity Editor
#else
        Application.Quit(); // For Application Build
#endif
    }

}

// Example of adding a click detector on a button through code:
// redButton.onClick.AddListener(() => ToggleButton("Red"));