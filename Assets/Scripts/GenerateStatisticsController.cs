using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

using System.Collections;

public class GenerateStatisticsController : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject GenerateStatisticsScreen;
    [SerializeField] private GameObject textEnterNameAndInputTextbox;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI textModeDifficulty;
    [SerializeField] private TextMeshProUGUI textDynamicSettingsActivated;
    [SerializeField] private TextMeshProUGUI textOfTextbox;
    [SerializeField] private GameObject submitButton;
    [SerializeField] private GameObject status;
    [SerializeField] private TextMeshProUGUI textStatus;
    [Header("Controllers")]
    [SerializeField] private MusicController musicController;
    [SerializeField] private SideBarController sideBarController;
    [SerializeField] private PlayerAndEnemyStatusController playerAndEnemyStatusController;
    [SerializeField] private GenerateStatisticsController generateStatisticsController;
    [SerializeField] private ConfirmQuitBattleController confirmQuitBattleController;

    private string dataLogPerTurnDGBInputDamageReceivedAndDealt;
    private string dataLogPerTurnDGBInputHealthLeft;
    private string dataLogPerTurnDGBInputPowerupUsage;
    private string dataLogPerTurnDGBInputTimeThinkingAndStepsTaken;
    private string dataLogPerTurnDGBOutputDifficultyIndex;

    private string dataLogPerTurnHpLeft;
    private string dataLogPerTurnEnemiesKilled;
    private string dataLogPerTurnEnemyPointsObtained;
    private string dataLogPerTurnGoldEarned;
    private string dataLogPerTurnCurrentGold;

    private string dataLogPerRoundDGBInputDamageReceivedAndDealt;
    private string dataLogPerRoundDGBInputHealthLeft;
    private string dataLogPerRoundDGBInputPowerupUsage;
    private string dataLogPerRoundDGBInputTimeThinkingAndStepsTaken;
    private string dataLogPerRoundDGBOutputDifficultyIndex;

    private string dataLogPerRoundHpLeft;
    private string dataLogPerRoundEnemiesKilled;
    private string dataLogPerRoundEnemyPointsObtained;
    private string dataLogPerRoundGoldEarned;
    private string dataLogPerRoundCurrentGold;

    private string dataLogPerRoundGoldSpent;
    private string dataLogPerRoundTotalMoves;


    private string selectedDifficulty;

    // I don't think keeping track of exactly which powerups were used will be necessary.

    //private string formUrl = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSddwM-d-9_O-AujSfuFWOQQAsEBXBRjaeacqaCjFY5-g_qx_A/formResponse";

    private void Start()
    {
        selectedDifficulty = PlayerPrefs.GetString("modeDifficulty", "Adaptive");
        if (selectedDifficulty == "Adaptive")
        {
            dataLogPerTurnDGBInputDamageReceivedAndDealt = "333,";
            dataLogPerTurnDGBInputHealthLeft = "1000,";
            dataLogPerTurnDGBInputPowerupUsage = "333,";
            dataLogPerTurnDGBInputTimeThinkingAndStepsTaken = "333,";
            dataLogPerTurnDGBOutputDifficultyIndex = "500,";

            dataLogPerTurnHpLeft = "50,";
            dataLogPerTurnEnemiesKilled = "0,";
            dataLogPerTurnEnemyPointsObtained = "0,";
            dataLogPerTurnGoldEarned = "0,"; // This also includes gold earned from selling powerups
            dataLogPerTurnCurrentGold = "0,";

            dataLogPerRoundDGBInputDamageReceivedAndDealt = "333,";
            dataLogPerRoundDGBInputHealthLeft = "1000,";
            dataLogPerRoundDGBInputPowerupUsage = "333,";
            dataLogPerRoundDGBInputTimeThinkingAndStepsTaken = "333,";
            dataLogPerRoundDGBOutputDifficultyIndex = "500,";

            dataLogPerRoundHpLeft = "50,";
            dataLogPerRoundEnemiesKilled = "0,";
            dataLogPerRoundEnemyPointsObtained = "0,";
            dataLogPerRoundGoldEarned = "0,";
            dataLogPerRoundCurrentGold = "0,";

            dataLogPerRoundTotalMoves = "0,";
            dataLogPerRoundGoldSpent = ""; // this one starts off with no string as at the "get ready" battle mode, it is already set to "0,"
        }


    inputField.onValueChanged.AddListener(ValidateInput);

        submitButton.SetActive(false);
        textModeDifficulty.text = PlayerPrefs.GetString("modeDifficulty", "Adaptive");

        textDynamicSettingsActivated.text = "<line-height=130%>";
        if (PlayerPrefs.GetString("modeDifficulty", "Adaptive") != "Adaptive")
        {
            textDynamicSettingsActivated.text += "Adaptive difficulty was not selected\n\n\n\n";
        }
        else
        {
            if (PlayerPrefs.GetInt("isStatsChecked", 0) == 1) { textDynamicSettingsActivated.text += "1. Enemies’ health, attack, quantity, and variants"; }
            else { textDynamicSettingsActivated.text += "1. ---"; }
            if (PlayerPrefs.GetInt("isHintsChecked", 0) == 1) { textDynamicSettingsActivated.text += "\n2. Visual hints of enemy attack and movement areas"; }
            else { textDynamicSettingsActivated.text += "\n2. ---"; }
            if (PlayerPrefs.GetInt("isPowerupChecked", 0) == 1) { textDynamicSettingsActivated.text += "\n3. Quality and price of powerups in the shop"; }
            else { textDynamicSettingsActivated.text += "\n3. ---"; }
            if (PlayerPrefs.GetInt("isAIChecked", 0) == 1) { textDynamicSettingsActivated.text += "\n4. Enemy AI"; }
            else { textDynamicSettingsActivated.text += "\n4. ---"; }
            if (PlayerPrefs.GetInt("isLimitChecked", 0) == 1) { textDynamicSettingsActivated.text += "\n5. Time limit to decide your next move"; }
            else { textDynamicSettingsActivated.text += "\n5. ---"; }
        }

        GenerateStatisticsScreen.SetActive(false);
    }

    // This method will restrict the input to letters, spaces, and max 20 characters
    private void ValidateInput(string input)
    {
        // Use a regular expression to filter only letters and spaces
        string validInput = Regex.Replace(input, @"[^a-zA-Z\s]", "");

        if (validInput.Length > 20)
        {
            validInput = validInput.Substring(0, 20);
        }
        inputField.text = validInput;

        if (string.IsNullOrEmpty(validInput.Trim()))
        {
            submitButton.SetActive(false);
            textStatus.text = "<line-height=130%>Enter your name first before you\ncan submit the gameplay data";
        }
        else
        {
            submitButton.SetActive(true);
            textStatus.text = "* Needs active internet connection";
        }
        musicController.PlayClickSoundEffect();
    }

    public void HideGenerateStatisticsScreen()  // Called in the Back button's inspector
    {
        musicController.PlayClickSoundEffect();
        GenerateStatisticsScreen.SetActive(false);;
    }

    public void ShowGenerateStatisticsScreen()
    {
        GenerateStatisticsScreen.SetActive(true);
        submitButton.SetActive(false);
        inputField.text = "";
        textStatus.text = "<line-height=130%>Enter your name first before you\ncan submit the gameplay data";
    }

    //public void PrintTextboxStringIntoConsole() // Called in the Submit button's inspector
    //{
    //    //musicController.PlayClickSoundEffect();
    //    string userInput = inputField.text;
    //    Debug.Log("User Input: " + userInput);
    //    submitButtonAndText.SetActive(false);
    //    textStatus.text = "Sending...";
    //    status.SetActive(true);
    //}

    public void LogPerTurnDGBInputsAndOutputs(string inputDGB1, string inputDGB2, string inputDGB3, string inputDGB4, string outputDGB) // Called in TurnController.NextEnemysTurnOneByOneForYellowTiles() when round is not over, then called in PlayerAndEnemyStatusController.AnEnemyWasKilledAndEarnGold() when round is over
    {
        dataLogPerTurnDGBInputDamageReceivedAndDealt    += inputDGB1 + ",";
        dataLogPerTurnDGBInputHealthLeft += inputDGB2 + ",";
        dataLogPerTurnDGBInputPowerupUsage += inputDGB3 + ",";
        dataLogPerTurnDGBInputTimeThinkingAndStepsTaken += inputDGB4 + ",";
        dataLogPerTurnDGBOutputDifficultyIndex += outputDGB + ",";
    }

    public void LogPerTurnHealthKillsPointsGold(string stat1, string stat2, string stat3, string stat4, string stat5) // Called in PlayerAndEnemyStatusController.AnEnemyWasKilledAndEarnGold() when round is over
    {
        dataLogPerTurnHpLeft += stat1 + ",";
        dataLogPerTurnEnemiesKilled += stat2 + ",";
        dataLogPerTurnEnemyPointsObtained += stat3 + ",";
        dataLogPerTurnGoldEarned += stat4 + ",";
        dataLogPerTurnCurrentGold += stat5 + ",";
    }

    public void LogPerRoundDGBInputsAndOutputs(string inputDGB1, string inputDGB2, string inputDGB3, string inputDGB4, string outputDGB) // Called in TurnController.NextEnemysTurnOneByOneForYellowTiles() when round is not over, then called in PlayerAndEnemyStatusController.AnEnemyWasKilledAndEarnGold() when round is over
    {
        dataLogPerRoundDGBInputDamageReceivedAndDealt += inputDGB1 + ",";
        dataLogPerRoundDGBInputHealthLeft += inputDGB2 + ",";
        dataLogPerRoundDGBInputPowerupUsage += inputDGB3 + ",";
        dataLogPerRoundDGBInputTimeThinkingAndStepsTaken += inputDGB4 + ",";
        dataLogPerRoundDGBOutputDifficultyIndex += outputDGB + ",";
    }

    public void LogPerRoundHealthKillsPointsGoldMoves(string stat1, string stat2, string stat3, string stat4, string stat5, string stat6) // Called in PlayerAndEnemyStatusController.AnEnemyWasKilledAndEarnGold() when round is over
    {
        dataLogPerRoundHpLeft += stat1 + ",";
        dataLogPerRoundEnemiesKilled += stat2 + ",";
        dataLogPerRoundEnemyPointsObtained += stat3 + ",";
        dataLogPerRoundGoldEarned += stat4 + ",";
        dataLogPerRoundCurrentGold += stat5 + ",";

        dataLogPerRoundTotalMoves += stat6 + ",";
    }

    public void LogPerRoundTotalSpending(string stat1) // Called every "Get Ready" screen when round begins
    {
        dataLogPerRoundGoldSpent += stat1 + ",";
    }


    public void SubmitFeedback() // Called in the Submit button's inspector
    {
        musicController.PlayClickSoundEffect();
        textStatus.text = "Sending...";
        submitButton.SetActive(false);

        string playerName = inputField.text;
        string modeDifficulty = textModeDifficulty.text;
        string activatedAdaptiveSettings = textDynamicSettingsActivated.text.Substring(18);
        string finalRoundNumber = "" + playerAndEnemyStatusController.GetRoundNumber();
        string isGameOverFrom0HpOrManualQuit = confirmQuitBattleController.GetQuitBattleManuallyAndNotFrom0Hp() ? "Manual Quit" : "Hp reached 0";
        string totalNumberOfMoves = "" + Mathf.Max(playerAndEnemyStatusController.GetTotalMoveCount(), 0);
        string totalNumberOfKills = "" + playerAndEnemyStatusController.GetKillCount();
        string totalTimePlayed = PlayerPrefs.GetInt("isTimerChecked", 1) == 1 ? sideBarController.GetSideBarTimerValue() : "-";
        // Store the recorded Dynamic Difficulty Indices Over Time? As well as hp, damage dealt, etc.
        StartCoroutine(Post(playerName, modeDifficulty, activatedAdaptiveSettings, finalRoundNumber, isGameOverFrom0HpOrManualQuit, totalNumberOfMoves, totalNumberOfKills, totalTimePlayed,
                            dataLogPerTurnDGBInputDamageReceivedAndDealt, dataLogPerTurnDGBInputHealthLeft, dataLogPerTurnDGBInputPowerupUsage, dataLogPerTurnDGBInputTimeThinkingAndStepsTaken, dataLogPerTurnDGBOutputDifficultyIndex,
                            dataLogPerTurnHpLeft, dataLogPerTurnEnemiesKilled, dataLogPerTurnEnemyPointsObtained, dataLogPerTurnGoldEarned, dataLogPerTurnCurrentGold,
                            dataLogPerRoundDGBInputDamageReceivedAndDealt, dataLogPerRoundDGBInputHealthLeft, dataLogPerRoundDGBInputPowerupUsage, dataLogPerRoundDGBInputTimeThinkingAndStepsTaken, dataLogPerRoundDGBOutputDifficultyIndex,
                            dataLogPerRoundHpLeft, dataLogPerRoundEnemiesKilled, dataLogPerRoundEnemyPointsObtained, dataLogPerRoundGoldEarned, dataLogPerRoundCurrentGold,
                            dataLogPerRoundGoldSpent, dataLogPerRoundTotalMoves));
    }

    private IEnumerator Post(string playerName, string isGameOverFrom0HpOrManualQuit, string modeDifficulty, string activatedAdaptiveSettings, string finalRoundNumber, string totalNumberOfMoves, string totalNumberOfKills, string totalTimePlayed,
                             string dataLogPerTurnDGBInputDamageReceivedAndDealt, string dataLogDGBInputHealthLeft, string dataLogDGBInputPowerupUsage, string dataLogDGBInputTimeThinkingAndStepsTaken, string dataLogDGBOutputDifficultyIndex,
                             string dataLogPerTurnHpLeft, string dataLogPerTurnEnemiesKilled, string dataLogPerTurnEnemyPointsObtained, string dataLogPerTurnGoldEarned, string dataLogPerTurnCurrentGold,
                             string dataLogPerRoundDGBInputDamageReceivedAndDealt, string dataLogPerRoundDGBInputHealthLeft, string dataLogPerRoundDGBInputPowerupUsage, string dataLogPerRoundDGBInputTimeThinkingAndStepsTaken, string dataLogPerRoundDGBOutputDifficultyIndex,
                             string dataLogPerRoundHpLeft, string dataLogPerRoundEnemiesKilled, string dataLogPerRoundEnemyPointsObtained, string dataLogPerRoundGoldEarned, string dataLogPerRoundCurrentGold,
                             string dataLogPerRoundGoldSpent, string dataLogPerRoundTotalMoves)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1480577021", playerName);
        form.AddField("entry.789620607" , isGameOverFrom0HpOrManualQuit);
        form.AddField("entry.2104693377", modeDifficulty);
        form.AddField("entry.292304105" , activatedAdaptiveSettings);
        form.AddField("entry.1085819894", finalRoundNumber);
        form.AddField("entry.1165544974", totalNumberOfMoves);
        form.AddField("entry.209557373" , totalNumberOfKills);
        form.AddField("entry.1445793752", totalTimePlayed);

        form.AddField("entry.95619748"  , dataLogPerTurnDGBInputDamageReceivedAndDealt);
        form.AddField("entry.1889820499", dataLogDGBInputHealthLeft);
        form.AddField("entry.525159224" , dataLogDGBInputPowerupUsage);
        form.AddField("entry.1048428466", dataLogDGBInputTimeThinkingAndStepsTaken);
        form.AddField("entry.183444338" , dataLogDGBOutputDifficultyIndex);

        form.AddField("entry.1351446707", dataLogPerTurnHpLeft);
        form.AddField("entry.968578843" , dataLogPerTurnEnemiesKilled);
        form.AddField("entry.319869713" , dataLogPerTurnEnemyPointsObtained);
        form.AddField("entry.361863272" , dataLogPerTurnGoldEarned);
        form.AddField("entry.338449906" , dataLogPerTurnCurrentGold);

        form.AddField("entry.1264335556", dataLogPerRoundDGBInputDamageReceivedAndDealt);
        form.AddField("entry.1133282211", dataLogPerRoundDGBInputHealthLeft);
        form.AddField("entry.1421523695", dataLogPerRoundDGBInputPowerupUsage);
        form.AddField("entry.636112759" , dataLogPerRoundDGBInputTimeThinkingAndStepsTaken);
        form.AddField("entry.160590269" , dataLogPerRoundDGBOutputDifficultyIndex);

        form.AddField("entry.1233825531", dataLogPerRoundHpLeft);
        form.AddField("entry.990743365" , dataLogPerRoundEnemiesKilled);
        form.AddField("entry.1497042868", dataLogPerRoundEnemyPointsObtained);
        form.AddField("entry.68674253"  , dataLogPerRoundGoldEarned);
        form.AddField("entry.1072868343", dataLogPerRoundCurrentGold);

        form.AddField("entry.1413542101", dataLogPerRoundTotalMoves);

        form.AddField("entry.1387347716", dataLogPerRoundGoldSpent);

        using (UnityWebRequest www = UnityWebRequest.Post("https://docs.google.com/forms/u/0/d/e/1FAIpQLSddwM-d-9_O-AujSfuFWOQQAsEBXBRjaeacqaCjFY5-g_qx_A/formResponse", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                textStatus.text = "Success! Thank you for your time!";
            }
            else
            {
                Debug.LogError("Error in feedback submission: " + www.error);
                textStatus.text = "<line-height=130%>Oh no, the data cannot be sent!\nCheck your internet and try again?";
                submitButton.SetActive(true);
            }
        }
    }

    // ChatGPT code:     https://chatgpt.com/share/bc4dee72-2830-44d0-82f5-1e4f6bd1d687
    // Youtube Tutorial: https://www.youtube.com/watch?v=WM7f4yN4ZHA
    // My Google Form:   https://forms.gle/9pQhyKCvxsV8cjR79
}
