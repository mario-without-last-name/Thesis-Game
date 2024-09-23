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
    [Header("Other Controllers")]
    [SerializeField] private SideBarController SideBarController;
    [SerializeField] private PlayerAndEnemyStatusController PlayerAndEnemyStatusController;
    [SerializeField] private ShopController ShopController;
    [SerializeField] private BottomBarController BottomBarController;
    [Header("Temporary")]
    [SerializeField] private ExampleSpawner ExampleSpawner; // Temporary

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
            BottomBarController.powerupsAreReadyForBattle(); // Should not be activated until the battle has actually begun
            ShopMode.SetActive(false);
            GetReadyMode.SetActive(true);
        }
        else if (battleMode == "Fight")
        {
            GetReadyMode.SetActive(false);
            FightMode.SetActive(true);
            SideBarController.SetSideBarIsTimerRunning(true);
            //ExampleSpawner.ExampleSpawnPlayerPiece(); // Temporary
            PlayerAndEnemyStatusController.SpawnPlayerAndEnemiesForNewRound();
        }
        else if(battleMode == "Victory")
        {
            //DestoryAllExampleTags(); // Temporary
            PlayerAndEnemyStatusController.DestroyAllPlayerAndEnemyPrefabs();
            FightMode.SetActive(false);
            VictoryMode.SetActive(true);
            SideBarController.SetSideBarIsTimerRunning(false);
        }
        else if(battleMode == "Shop")
        {
            ShopController.refreshShopPowerupOptions();
            BottomBarController.optionToSellPowerups();
            VictoryMode.SetActive(false);
            ShopMode.SetActive(true);
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
