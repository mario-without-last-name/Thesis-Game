using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // ALL OF THESE ARE INSERTED INTO BUTTONS WITHIN THE SceneMainMenu,
    // TO TELL THEM WHAT PAGE THEY MUST DIRECT TO

    // How to make buttons change cursor to the hand?



    // MAIN MENU ============================================================================

    public void MainMenu()
    {

    }
    


    // FIGHT ============================================================================

    public void Fight()
    {
        SceneManager.LoadSceneAsync("SceneFight"); // LATER CHANGE
    }

    public void EnterFight(int difficulty) // 1 = Easy, 2 = Medium, 3 = Hard, 4 = Adaptive
    {
        SceneManager.LoadSceneAsync("SceneFight"); // Can also use (1), as seen in the Build Settings
    }



    // GUIDE ============================================================================

    public void Guide()
    {

    }
    // What is a good way to handle the "next page" and "prev page" buttons?
    public void HowToPlay(int page)
    {
        
    }

    public void Bestiary(int page)
    {

    }

    public void ResearchInfo(int page)
    {

    }



    // SETTINGS ============================================================================

    public void Settings()
    {

    }

    public void SettingsGeneral()
    {

    }

    public void SettingsAdaptive()
    {

    }



    // EXIT ============================================================================
    public void ExitGame()
    {
        EditorApplication.isPlaying = false; // For Unity Editor
        // Application.Quit(); // For Application Build
    }
}
