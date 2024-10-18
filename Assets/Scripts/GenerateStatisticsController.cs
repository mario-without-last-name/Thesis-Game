using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using static UnityEditor.ShaderData;
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

    private void Start()
    {
        inputField.onValueChanged.AddListener(ValidateInput);

        submitButton.SetActive(false);
        textModeDifficulty.text = PlayerPrefs.GetString("modeDifficulty", "???");

        textDynamicSettingsActivated.text = "<line-height=130%>";
        if (PlayerPrefs.GetString("modeDifficulty", "???") != "Adaptive")
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

    private string formUrl = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSddwM-d-9_O-AujSfuFWOQQAsEBXBRjaeacqaCjFY5-g_qx_A/formResponse";

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
        string totalTimePlayed = PlayerPrefs.GetInt("isTimerChecked", 0) == 1 ? sideBarController.GetSideBarTimerValue() : "-";
        // Store the recorded Dynamic Difficulty Indices Over Time? As well as hp, damage dealt, etc.
        StartCoroutine(Post(playerName, modeDifficulty, activatedAdaptiveSettings, finalRoundNumber, isGameOverFrom0HpOrManualQuit, totalNumberOfMoves, totalNumberOfKills, totalTimePlayed));
    }

    private IEnumerator Post(string playerName, string modeDifficulty, string activatedAdaptiveSettings, string finalRoundNumber, string isGameOverFrom0HpOrManualQuit, string totalNumberOfMoves, string totalNumberOfKills, string totalTimePlayed)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1480577021", playerName);
        form.AddField("entry.2104693377", modeDifficulty);
        form.AddField("entry.292304105", activatedAdaptiveSettings);
        form.AddField("entry.1085819894", finalRoundNumber);
        form.AddField("entry.789620607", isGameOverFrom0HpOrManualQuit);
        form.AddField("entry.1165544974", totalNumberOfMoves);
        form.AddField("entry.209557373", totalNumberOfKills);
        form.AddField("entry.1445793752", totalTimePlayed);

        using (UnityWebRequest www = UnityWebRequest.Post(formUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                //Debug.Log("Feedback submitted successfully.");
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

    // https://chatgpt.com/share/bc4dee72-2830-44d0-82f5-1e4f6bd1d687
    // https://www.youtube.com/watch?v=WM7f4yN4ZHA
}
