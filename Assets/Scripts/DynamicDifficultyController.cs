using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicDifficultyController : MonoBehaviour
{
    // For all of these 4 dynamic inputs... 0 -> easy (lower bound), 1 -> hard (upper bound), 0.5 -> medium
    private float dynamicInputIndexDamageReceivedAndDealt;
    private float dynamicInputIndexHealthLeft;
    private float dynamicInputIndexPowerupUsage;
    private float dynamicInputIndexTimeThinkingAndStepsTaken;

    // So that it cannot be exploited too much... every round, these indices cannot be changed by more than / less than 3 of their starting values.
    private float ThisRoundInitialdynamicInputIndexDamageReceived;
    private float ThisRoundInitialdynamicInputIndexHealthLeft;
    private float ThisRoundInitialdynamicInputIndexPowerupUsage;
    private float ThisRoundInitialdynamicInputIndexTimeThinkingAndStepsTaken;

    private float dynamicOutputOverallIndex;

    private string selectedDifficulty;

    void Start()
    { // If the selected difficulty was not Adaptive, then no point of this code doing anything
        selectedDifficulty = PlayerPrefs.GetString("modeDifficulty", "???");

        dynamicInputIndexDamageReceivedAndDealt = 0.5f;
        dynamicInputIndexHealthLeft = 1.0f;
        dynamicInputIndexPowerupUsage = 0.25f;
        dynamicInputIndexTimeThinkingAndStepsTaken = 0.5f;
        ResetThisRoundInitialDynamicInputIndices();
        //dynamicOutputOverallIndex = (dynamicInputIndexDamageReceivedAndDealt + dynamicInputIndexHealthLeft + dynamicInputIndexPowerupUsage + dynamicInputIndexTimeThinkingAndStepsTaken) / 4.0f;
        dynamicOutputOverallIndex = (dynamicInputIndexDamageReceivedAndDealt + dynamicInputIndexTimeThinkingAndStepsTaken + dynamicInputIndexTimeThinkingAndStepsTaken) / 3.0f;
    }

    public void ResetThisRoundInitialDynamicInputIndices()
    {
        ThisRoundInitialdynamicInputIndexDamageReceived = dynamicInputIndexDamageReceivedAndDealt;
        ThisRoundInitialdynamicInputIndexHealthLeft = dynamicInputIndexHealthLeft;
        ThisRoundInitialdynamicInputIndexPowerupUsage = dynamicInputIndexPowerupUsage;
        ThisRoundInitialdynamicInputIndexTimeThinkingAndStepsTaken = dynamicInputIndexTimeThinkingAndStepsTaken;
    }

    public void SetDynamicInputChange(string dynamicInputType, float change, bool overwriteExisitingValue) // overwriteExisitingValue = just change the value. not overwriteExisitingValue = add the variable to the current amount.
    {
        if (selectedDifficulty != "Adaptive") { return; }
        else if (selectedDifficulty == "Adaptive")
        {
            if      (dynamicInputType == "damageReceivedAndDealt")
            {
                dynamicInputIndexDamageReceivedAndDealt = overwriteExisitingValue ? change : Mathf.Clamp(dynamicInputIndexDamageReceivedAndDealt + change, 0.0f, 1.0f);
                dynamicInputIndexDamageReceivedAndDealt = Mathf.Min(dynamicInputIndexDamageReceivedAndDealt, ThisRoundInitialdynamicInputIndexDamageReceived + 0.3f);
                dynamicInputIndexDamageReceivedAndDealt = Mathf.Max(dynamicInputIndexDamageReceivedAndDealt, ThisRoundInitialdynamicInputIndexDamageReceived - 0.3f);
            }
            else if (dynamicInputType == "healthLeft")
            {
                dynamicInputIndexHealthLeft = overwriteExisitingValue ? change : Mathf.Clamp(dynamicInputIndexHealthLeft + change, 0.0f, 1.0f);
                dynamicInputIndexHealthLeft = Mathf.Min(dynamicInputIndexHealthLeft, ThisRoundInitialdynamicInputIndexHealthLeft + 0.3f);
                dynamicInputIndexHealthLeft = Mathf.Max(dynamicInputIndexHealthLeft, ThisRoundInitialdynamicInputIndexHealthLeft - 0.3f);
            }
            else if (dynamicInputType == "powerupUsage")
            {
                dynamicInputIndexPowerupUsage = overwriteExisitingValue ? change : Mathf.Clamp(dynamicInputIndexPowerupUsage + change, 0.0f, 1.0f);
                dynamicInputIndexPowerupUsage = Mathf.Min(dynamicInputIndexPowerupUsage, ThisRoundInitialdynamicInputIndexPowerupUsage + 0.3f);
                dynamicInputIndexPowerupUsage = Mathf.Max(dynamicInputIndexPowerupUsage, ThisRoundInitialdynamicInputIndexPowerupUsage - 0.3f);
            }
            else if (dynamicInputType == "TimeThinkingAndStepsTaken")
            {
                dynamicInputIndexTimeThinkingAndStepsTaken = overwriteExisitingValue ? change : Mathf.Clamp(dynamicInputIndexTimeThinkingAndStepsTaken + change, 0.0f, 1.0f);
                dynamicInputIndexTimeThinkingAndStepsTaken = Mathf.Min(dynamicInputIndexTimeThinkingAndStepsTaken, ThisRoundInitialdynamicInputIndexTimeThinkingAndStepsTaken + 0.3f);
                dynamicInputIndexTimeThinkingAndStepsTaken = Mathf.Max(dynamicInputIndexTimeThinkingAndStepsTaken, ThisRoundInitialdynamicInputIndexTimeThinkingAndStepsTaken - 0.15f);
            }
            else { Debug.LogWarning("unknown dynamic input type:" + dynamicInputType); }
        }
        else { Debug.LogWarning("unknown difficulty setting:" + selectedDifficulty);}
        //dynamicOutputOverallIndex = (dynamicInputIndexDamageReceivedAndDealt + dynamicInputIndexHealthLeft + dynamicInputIndexPowerupUsage + dynamicInputIndexTimeThinkingAndStepsTaken) / 4.0f;
        dynamicOutputOverallIndex = (dynamicInputIndexDamageReceivedAndDealt + dynamicInputIndexTimeThinkingAndStepsTaken + dynamicInputIndexTimeThinkingAndStepsTaken) / 3.0f;
    }

    public float GetDynamicOutput(string dynamicOutputType)
    {
        if      (selectedDifficulty == "Easy")     { return 0.0f; }
        else if (selectedDifficulty == "Medium")   { return 0.5f; }
        else if (selectedDifficulty == "Hard")     { return 1.0f; }
        else if (selectedDifficulty == "Adaptive")
        {
            if      (dynamicOutputType == "enemyStats")     { return PlayerPrefs.GetInt("isStatsChecked", 0)   == 1 ? dynamicOutputOverallIndex : 0.5f; }
            else if (dynamicOutputType == "visualHint")     { return PlayerPrefs.GetInt("isHintsChecked", 0)   == 1 ? dynamicOutputOverallIndex : 0.5f; }
            else if (dynamicOutputType == "powerupQuality") { return PlayerPrefs.GetInt("isPowerupChecked", 0) == 1 ? dynamicOutputOverallIndex : 0.5f; }
            else if (dynamicOutputType == "enemyAI")        { return PlayerPrefs.GetInt("isAIChecked", 0)      == 1 ? dynamicOutputOverallIndex : 0.5f; }
            else if (dynamicOutputType == "timeLimit")      { return PlayerPrefs.GetInt("isLimitChecked", 0)   == 1 ? dynamicOutputOverallIndex : 0.5f; }
            else { Debug.LogWarning("unknown dynamic output type: " + dynamicOutputType); return 0.5f; }
        }
        else { Debug.LogWarning("unknown difficulty setting: " + selectedDifficulty); return 0.5f; }
    }

    public void PrintAllDGBInputAndOutputIndex()
    {
        Debug.Log("OUTPUT DGB INDEX: " + dynamicOutputOverallIndex + " --- Damage Received: " + dynamicInputIndexDamageReceivedAndDealt + ", Health Left: " + dynamicInputIndexHealthLeft + ", Powerup Usage: " + dynamicInputIndexPowerupUsage + ", Time + Steps Taken: " + dynamicInputIndexTimeThinkingAndStepsTaken);
    }
}
