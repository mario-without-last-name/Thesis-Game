using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DynamicDifficultyController : MonoBehaviour
{

    [Header("Controllers")]
    [SerializeField] private GenerateStatisticsController generateStatisticsController;

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
        selectedDifficulty = PlayerPrefs.GetString("modeDifficulty", "Adaptive");

        dynamicInputIndexDamageReceivedAndDealt = 1f / 3f;
        dynamicInputIndexHealthLeft = 1.0f;
        dynamicInputIndexPowerupUsage = 1f / 3f;
        dynamicInputIndexTimeThinkingAndStepsTaken = 1f / 3f;
        ResetThisRoundInitialDynamicInputIndices();
        dynamicOutputOverallIndex = (dynamicInputIndexDamageReceivedAndDealt + dynamicInputIndexHealthLeft + dynamicInputIndexPowerupUsage + dynamicInputIndexTimeThinkingAndStepsTaken) / 4.0f;
    }

    public void ResetThisRoundInitialDynamicInputIndices() // Ensuring that in each round, every input does not go beyond +/- 0.3 (or whatever thershold). Health left is the only one without this.
    {
        ThisRoundInitialdynamicInputIndexDamageReceived = dynamicInputIndexDamageReceivedAndDealt;
        ThisRoundInitialdynamicInputIndexHealthLeft = dynamicInputIndexHealthLeft;
        ThisRoundInitialdynamicInputIndexPowerupUsage = dynamicInputIndexPowerupUsage;
        ThisRoundInitialdynamicInputIndexTimeThinkingAndStepsTaken = dynamicInputIndexTimeThinkingAndStepsTaken;
    }

//DGB for powerup usage increases per:
//purchase of powerup
//selling of poweup
//good use of active powerup

//decreases per:
//new round
//overhealed on healling stat buffs
//wrong use of active powerup

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
                //dynamicInputIndexHealthLeft = Mathf.Min(dynamicInputIndexHealthLeft, ThisRoundInitialdynamicInputIndexHealthLeft + 0.3f);
                //dynamicInputIndexHealthLeft = Mathf.Max(dynamicInputIndexHealthLeft, ThisRoundInitialdynamicInputIndexHealthLeft - 0.3f);
                // This is commented because unlike the other DGB Input, no need to worry too much about making the health DGB input increase or decrease fast
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
        dynamicOutputOverallIndex = (dynamicInputIndexDamageReceivedAndDealt + dynamicInputIndexHealthLeft + dynamicInputIndexPowerupUsage + dynamicInputIndexTimeThinkingAndStepsTaken) / 4.0f;
        //dynamicOutputOverallIndex = (dynamicInputIndexDamageReceivedAndDealt + dynamicInputIndexTimeThinkingAndStepsTaken + dynamicInputIndexTimeThinkingAndStepsTaken) / 3.0f;
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

    public void PrintAndLogPerTurnAllDGBInputAndOutputIndex()
    {
        //Debug.Log("[PER TURN] OUTPUT DGB INDEX: " + dynamicOutputOverallIndex + " --- Damage Received: " + dynamicInputIndexDamageReceivedAndDealt + ", Health Left: " + dynamicInputIndexHealthLeft + ", Powerup Usage: " + dynamicInputIndexPowerupUsage + ", Time + Steps Taken: " + dynamicInputIndexTimeThinkingAndStepsTaken);
        generateStatisticsController.LogPerTurnDGBInputsAndOutputs(Mathf.RoundToInt(dynamicInputIndexDamageReceivedAndDealt * 1000).ToString(), Mathf.RoundToInt(dynamicInputIndexHealthLeft * 1000).ToString(), Mathf.RoundToInt(dynamicInputIndexPowerupUsage * 1000).ToString(), Mathf.RoundToInt(dynamicInputIndexTimeThinkingAndStepsTaken * 1000).ToString(), Mathf.RoundToInt(dynamicOutputOverallIndex * 1000).ToString());
    }

    public void PrintAndLogPerRoundAllDGBInputAndOutputIndex()
    {
        //Debug.Log("[===PER ROUND===] OUTPUT DGB INDEX: " + dynamicOutputOverallIndex + " --- Damage Received: " + dynamicInputIndexDamageReceivedAndDealt + ", Health Left: " + dynamicInputIndexHealthLeft + ", Powerup Usage: " + dynamicInputIndexPowerupUsage + ", Time + Steps Taken: " + dynamicInputIndexTimeThinkingAndStepsTaken);
        generateStatisticsController.LogPerRoundDGBInputsAndOutputs(Mathf.RoundToInt(dynamicInputIndexDamageReceivedAndDealt * 1000).ToString(), Mathf.RoundToInt(dynamicInputIndexHealthLeft * 1000).ToString(), Mathf.RoundToInt(dynamicInputIndexPowerupUsage * 1000).ToString(), Mathf.RoundToInt(dynamicInputIndexTimeThinkingAndStepsTaken * 1000).ToString(), Mathf.RoundToInt(dynamicOutputOverallIndex * 1000).ToString());
    }
}