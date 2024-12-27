using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class BattleModeController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textBonusGold;
    [Header("Battle Modes")]
    [SerializeField] private GameObject GetReadyMode;
    [SerializeField] private GameObject FightMode; // Contains 8x18 movement tiles for active powerups. Also the place where the 4 temporary buttons (to trigger sounds and skip level manually) is stored
    [SerializeField] private GameObject VictoryMode;
    [SerializeField] private GameObject ShopMode;
    [SerializeField] private GameObject gameOverMode;
    [Header("Controllers")]
    [SerializeField] private MusicController musicController;
    [SerializeField] private TurnController turnController;
    [SerializeField] private SideBarController sideBarController;
    [SerializeField] private PlayerAndEnemyStatusController playerAndEnemyStatusController;
    [SerializeField] private ShopController shopController;
    [SerializeField] private BottomBarController bottomBarController;
    [SerializeField] private GameOverController gameOverController;
    [SerializeField] private DynamicDifficultyController dynamicDifficultyController;
    [SerializeField] private GenerateStatisticsController generateStatisticsController;
    [Header("Temporary")]
    [SerializeField] private ExampleSpawner exampleSpawner; // Temporary

    public string currentBattleMode = "GetReady";


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
            currentBattleMode = "GetReady";
            generateStatisticsController.LogPerRoundTotalSpending(shopController.GetTotalGoldSpentSoFar().ToString());
            bottomBarController.PowerupsAreReadyForBattle(); // Should not be activated until the battle has actually begun
            ShopMode.SetActive(false);
            GetReadyMode.SetActive(true);
            // A function to PowerupsCatalogController to reset the status of some powerups
        }
        else if (battleMode == "Fight")
        {
            currentBattleMode = "Fight";
            GetReadyMode.SetActive(false);
            FightMode.SetActive(true);
            sideBarController.SetSideBarIsTimerRunning(true);
            //exampleSpawner.ExampleSpawnPlayerPiece(); // Temporary
            playerAndEnemyStatusController.SpawnPlayerAndEnemiesForNewRound();
            dynamicDifficultyController.ResetThisRoundInitialDynamicInputIndices();
        }
        else if(battleMode == "Victory")
        {
            currentBattleMode = "Victory";
            //DestoryAllExampleTags(); // Temporary
            playerAndEnemyStatusController.DestroyAllPlayerAndEnemyPrefabs();
            FightMode.SetActive(false);
            VictoryMode.SetActive(true);
            sideBarController.SetSideBarIsTimerRunning(false);
            turnController.setCanPlayerSelectActivePowerup(false);
        }
        else if(battleMode == "Shop")
        {
            currentBattleMode = "Shop";
            dynamicDifficultyController.SetDynamicInputChange("powerupUsage", -0.05f, false);
            playerAndEnemyStatusController.SetNextRoundNumber();
            shopController.ResetRerollPrice();
            shopController.ActivateAndRerollShopPowerupOptions();
            bottomBarController.OptionToSellPowerups();
            VictoryMode.SetActive(false);
            ShopMode.SetActive(true);
        }
        else if (battleMode == "GameOver")
        {
            currentBattleMode = "GameOver";
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
            currentBattleMode = "Unkown battle mode";
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

    // ====================================================
    // GETTER, SETTER
    
    public void SetTextBonusGold(int bonusGoldValue) // shown at the victory screen
    {
        textBonusGold.text = "$ " + bonusGoldValue;
    }

    public string GetCurrentBattleMode()
    {
        return currentBattleMode;
    }

}
