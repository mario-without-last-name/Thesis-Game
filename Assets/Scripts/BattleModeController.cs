using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleModeController : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private MusicController musicController;
    [Header("Battle Modes")]
    [SerializeField] private GameObject GetReadyMode;
    [SerializeField] private GameObject FightMode;
    [SerializeField] private GameObject VictoryMode;
    [SerializeField] private GameObject ShopMode;
    [SerializeField] private GameObject gameOverMode;
    [Header("Other Controllers")]
    [SerializeField] private TurnController turnController;
    [SerializeField] private SideBarController sideBarController;
    [SerializeField] private PlayerAndEnemyStatusController playerAndEnemyStatusController;
    [SerializeField] private ShopController shopController;
    [SerializeField] private BottomBarController bottomBarController;
    [SerializeField] private GameOverController gameOverController;
    [Header("Temporary")]
    [SerializeField] private ExampleSpawner exampleSpawner; // Temporary


    //[SerializeField] private GameObject enemyPrefab;
    //private CodeForPrefabEnemy codeForPrefabEnemy;

    // Start is called before the first frame update
    void Start()
    {
        musicController.SetBackgroundMusic(true);
        GetReadyMode.SetActive(false);
        FightMode.SetActive(false);
        VictoryMode.SetActive(false);
        ShopMode.SetActive(false);
        BattleModeChanger("GetReady");
    }

    // Update is called once per frame
    public void BattleModeChanger(string battleMode) // GetReady - Fight - Victory - Shop
    {
        musicController.PlayClickSoundEffect();

        if (battleMode == "GetReady")
        {
            bottomBarController.PowerupsAreReadyForBattle(); // Should not be activated until the battle has actually begun
            ShopMode.SetActive(false);
            GetReadyMode.SetActive(true);
        }
        else if (battleMode == "Fight")
        {
            GetReadyMode.SetActive(false);
            FightMode.SetActive(true);
            sideBarController.SetSideBarIsTimerRunning(true);
            //exampleSpawner.ExampleSpawnPlayerPiece(); // Temporary
            playerAndEnemyStatusController.SpawnPlayerAndEnemiesForNewRound();
        }
        else if(battleMode == "Victory")
        {
            //DestoryAllExampleTags(); // Temporary
            playerAndEnemyStatusController.DestroyAllPlayerAndEnemyPrefabs();
            FightMode.SetActive(false);
            VictoryMode.SetActive(true);
            sideBarController.SetSideBarIsTimerRunning(false);
        }
        else if(battleMode == "Shop")
        {
            playerAndEnemyStatusController.SetNextRoundNumber();
            shopController.RefreshShopPowerupOptions();
            bottomBarController.OptionToSellPowerups();
            VictoryMode.SetActive(false);
            ShopMode.SetActive(true);
        }
        else if (battleMode == "GameOver")
        {
            playerAndEnemyStatusController.DestroyAllPlayerAndEnemyPrefabs();
            FightMode.SetActive(false);
            VictoryMode.SetActive(false);
            sideBarController.SetSideBarIsTimerRunning(false);
            playerAndEnemyStatusController.DestroyAllPlayerAndEnemyPrefabs();
            musicController.PlayClickSoundEffect();
            musicController.SetBackgroundMusic(false);
            musicController.PlayGameOverSoundEffectSource();
            sideBarController.SetSideBarIsTimerRunning(false);
            gameOverController.ChangeTextOf5GameOverStatistics();
            gameOverMode.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Unkown battle mode");
        }
    }

    //public void DestoryAllExampleTags() // Temporary
    //{
    //    GameObject[] examplesToDestroy = GameObject.FindGameObjectsWithTag("dummyTag");
    //    Debug.Log("DestroyAllExampleTags() has been called");
    //    for (int i = 0; i < examplesToDestroy.Length; i++)
    //    {
    //        if (examplesToDestroy[i] != null)
    //        {
    //            Debug.Log(examplesToDestroy[i]);
    //            Destroy(examplesToDestroy[i]);
    //            Debug.Log("Destroyed 1 object with dummyTag tag");
    //        }
    //    }
    //}
}
