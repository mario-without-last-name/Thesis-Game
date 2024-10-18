using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Reflection;

public class BestiaryController : MonoBehaviour
{
    [SerializeField] private Sprite spritePawnGoblin, spritePawnSkeleton, spritePawnBandit, spritePawnSlime,
                                    spriteRookTroll, spriteRookGiant, spriteRookExecutioner, spriteRookBeastMan,
                                    spriteBishopDarkElf, spriteBishopWarlock, spriteBishopElementalist, spriteBishopBomber,
                                    spriteKnightDireBeast, spriteKnightCentaur, spriteKnightDullahan, spriteKnightGryphon,
                                    spriteQueenMinotaur, spriteQueenWyrm, spriteQueenAbomination, spriteQueenGolem,
                                    spriteKingLich, spriteKingTitan, spriteKingDragon, spriteKingVampire,
                                    spriteBlank;
    public Sprite GetEnemySprite(string thisEnemyVariant)
    {
        if      (thisEnemyVariant == "pawnGoblin")         { return spritePawnGoblin; }
        else if (thisEnemyVariant == "pawnSkeleton")       { return spritePawnSkeleton; }
        else if (thisEnemyVariant == "pawnBandit")         { return spritePawnBandit; }
        else if (thisEnemyVariant == "pawnSlime")          { return spritePawnSlime; }
        else if (thisEnemyVariant == "rookTroll")          { return spriteRookTroll; }
        else if (thisEnemyVariant == "rookGiant")          { return spriteRookGiant; }
        else if (thisEnemyVariant == "rookExecutioner")    { return spriteRookExecutioner; }
        else if (thisEnemyVariant == "rookBeastMan")       { return spriteRookBeastMan; }
        else if (thisEnemyVariant == "bishopDarkElf")      { return spriteBishopDarkElf; }
        else if (thisEnemyVariant == "bishopWarlock")      { return spriteBishopWarlock; }
        else if (thisEnemyVariant == "bishopElementalist") { return spriteBishopElementalist; }
        else if (thisEnemyVariant == "bishopBomber")       { return spriteBishopBomber; }
        else if (thisEnemyVariant == "knightDireBeast")    { return spriteKnightDireBeast; }
        else if (thisEnemyVariant == "knightCentaur")      { return spriteKnightCentaur; }
        else if (thisEnemyVariant == "knightDullahan")     { return spriteKnightDullahan; }
        else if (thisEnemyVariant == "knightGryphon")      { return spriteKnightGryphon; }
        else if (thisEnemyVariant == "queenMinotaur")      { return spriteQueenMinotaur; }
        else if (thisEnemyVariant == "queenWyrm")          { return spriteQueenWyrm; }
        else if (thisEnemyVariant == "queenAbomination")   { return spriteQueenAbomination; }
        else if (thisEnemyVariant == "queenGolem")         { return spriteQueenGolem; }
        else if (thisEnemyVariant == "kingLich")           { return spriteKingLich; }
        else if (thisEnemyVariant == "kingTitan")          { return spriteKingTitan; }
        else if (thisEnemyVariant == "kingDragon")         { return spriteKingDragon; }
        else if (thisEnemyVariant == "kingVampire")        { return spriteKingVampire; }
        else {
            Debug.LogWarning("Unknown enemy variant: " + thisEnemyVariant);
            return spriteBlank;
        }
    }

    public int[][] GetEnemyMoveTiles(string thisEnemyVariant)
    { // Slime technically doesn't move, but just make its move delay 99 and add som emove tiles to avoid further code reformatting
        if      (thisEnemyVariant == "pawnGoblin")         { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "pawnSkeleton")       { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "pawnBandit")         { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "pawnSlime")          { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "rookTroll")          { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "rookGiant")          { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "rookExecutioner")    { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "rookBeastMan")       { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "bishopDarkElf")      { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "bishopWarlock")      { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "bishopElementalist") { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "bishopBomber")       { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "knightDireBeast")    { return new int[][] { new[] {+1,+2}, new[] {+2,+1}, new[] {+2,-1}, new[] {+1,-2}, new[] {-1,-2}, new[] {-2,-1}, new[] {-2,+1}, new[] {-1,+2} }; }
        else if (thisEnemyVariant == "knightCentaur")      { return new int[][] { new[] {+1, 0}, new[] {+2, 0}, new[] {-1, 0}, new[] {-2, 0}, new[] { 0,+1}, new[] { 0,+2}, new[] { 0,-1}, new[] { 0,-2}, new[] {+1,+1}, new[] {+2,+2}, new[] {+1,-1}, new[] {+2,-2}, new[] {-1,+1}, new[] {-2,+2}, new[] {-1,-1}, new[] {-2,-2} }; }
        else if (thisEnemyVariant == "knightDullahan")     { return new int[][] { new[] {+1, 0}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1, 0} }; }
        else if (thisEnemyVariant == "knightGryphon")      { return new int[][] { new[] {+3,+2}, new[] {+3,+1}, new[] {+3, 0}, new[] {+3,-1}, new[] {+3,-2}, new[] {-3,+2}, new[] {-3,+1}, new[] {-3, 0}, new[] {-3,-1}, new[] {-3,-2}, new[] {+2,+3}, new[] {+1,+3}, new[] { 0,+3}, new[] {-1,+3}, new[] {-2,+3}, new[] {+2,-3}, new[] {+1,-3}, new[] { 0,-3}, new[] {-1,-3}, new[] {-2,-3} }; }
        else if (thisEnemyVariant == "queenMinotaur")      { return new int[][] { new[] {+1, 0}, new[] {+2, 0}, new[] {+3, 0}, new[] {-1, 0}, new[] {-2, 0}, new[] {-3, 0}, new[] { 0,+1}, new[] { 0,+2}, new[] { 0,+3}, new[] { 0,-1}, new[] { 0,-2}, new[] { 0,-3} }; }
        else if (thisEnemyVariant == "queenWyrm")          { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1}, new[] { 0,+2}, new[] {+2, 0}, new[] { 0,-2}, new[] {-2, 0} }; }
        else if (thisEnemyVariant == "queenAbomination")   { return new int[][] { new[] { 0,+2}, new[] {+2,+2}, new[] {+2, 0}, new[] {+2,-2}, new[] { 0,-2}, new[] {-2,-2}, new[] {-2, 0}, new[] {-2,+2} }; }
        else if (thisEnemyVariant == "queenGolem")         { return new int[][] { new[] {+1,+1}, new[] {+2,+2}, new[] {+3,+3}, new[] {+1,-1}, new[] {+2,-2}, new[] {+3,-3}, new[] {-1,+1}, new[] {-2,+2}, new[] {-3,+3}, new[] {-1,-1}, new[] {-2,-2}, new[] {-3,-3} }; }
        else if (thisEnemyVariant == "kingLich")           { return new int[][] { new[] {+3,+3}, new[] {+3, 0}, new[] {+3,-3}, new[] { 0,+3}, new[] { 0,-3}, new[] {-3,+3}, new[] {-3, 0}, new[] {-3,-3} }; }
        else if (thisEnemyVariant == "kingTitan")          { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "kingDragon")         { return new int[][] { new[] {+1, 0}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1, 0} }; }
        else if (thisEnemyVariant == "kingVampire")        { return new int[][] { new[] {-3,-3}, new[] {-2,-3}, new[] {-1,-3}, new[] { 0,-3}, new[] {+1,-3}, new[] {+2,-3}, new[] {+3,-3},                new[] {-3,-2}, new[] {-2,-2}, new[] {-1,-2}, new[] { 0,-2}, new[] {+1,-2}, new[] {+2,-2}, new[] {+3,-2},                new[] {-3,-1}, new[] {-2,-1}, new[] {-1,-1}, new[] { 0,-1}, new[] {+1,-1}, new[] {+2,-1}, new[] {+3,-1},                new[] {-3, 0}, new[] {-2, 0}, new[] {-1, 0},                new[] {+1, 0}, new[] {+2, 0}, new[] {+3, 0},                new[] {-3,+1}, new[] {-2,+1}, new[] {-1,+1}, new[] { 0,+1}, new[] {+1,+1}, new[] {+2,+1}, new[] {+3,+1},                new[] {-3,+2}, new[] {-2,+2}, new[] {-1,+2}, new[] { 0,+2}, new[] {+1,+2}, new[] {+2,+2}, new[] {+3,+2},                new[] {-3,+3}, new[] {-2,+3}, new[] {-1,+3}, new[] { 0,+3}, new[] {+1,+3}, new[] {+2,+3}, new[] {+3,+3} }; }
        else {
            Debug.LogWarning("Unknown enemy variant: " + thisEnemyVariant);
            return new int[][] { };
        }

    }

    public int[][] GetEnemyAttackTiles(string thisEnemyVariant)
    {
        if      (thisEnemyVariant == "pawnGoblin")         { return new int[][] { new[] {+1, 0}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1, 0} }; }
        else if (thisEnemyVariant == "pawnSkeleton")       { return new int[][] { new[] {+1,+1}, new[] {+1,-1}, new[] {-1,+1}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "pawnBandit")         { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "pawnSlime")          { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1}, new[] {+2, 0}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2, 0} }; }
        else if (thisEnemyVariant == "rookTroll")          { return new int[][] { new[] {+1, 0}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1, 0}, new[] {+2, 0}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2, 0} }; }
        else if (thisEnemyVariant == "rookGiant")          { return new int[][] { new[] {+1,+1}, new[] {+1,-1}, new[] {-1,+1}, new[] {-1,-1}, new[] {+2,+2}, new[] {+2,-2}, new[] {-2,+2}, new[] {-2,-2} }; }
        else if (thisEnemyVariant == "rookExecutioner")    { return new int[][] { new[] {+1,+1}, new[] {+1,-1}, new[] {-1,+1}, new[] {-1,-1}, new[] {+2, 0}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2, 0} }; }
        else if (thisEnemyVariant == "rookBeastMan")       { return new int[][] { new[] {+1, 0}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1, 0} }; }
        else if (thisEnemyVariant == "bishopDarkElf")      { return new int[][] { new[] {+2,+3}, new[] {+3,+2}, new[] {-2,+3}, new[] {-3,+2}, new[] {+2,-3}, new[] {+3,-2}, new[] {-2,-3}, new[] {-3,-2} }; }
        else if (thisEnemyVariant == "bishopWarlock")      { return new int[][] { new[] {+1,+3}, new[] {+3,+1}, new[] {-1,+3}, new[] {-3,+1}, new[] {+1,-3}, new[] {+3,-1}, new[] {-1,-3}, new[] {-3,-1} }; }
        else if (thisEnemyVariant == "bishopElementalist") { return new int[][] { new[] {+2,+2}, new[] {+2,-2}, new[] {-2,+2}, new[] {-2,-2}, new[] {+2, 0}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2, 0} }; }
        else if (thisEnemyVariant == "bishopBomber")       { return new int[][] { new[] {+3,+3}, new[] {+3,-3}, new[] {-3,+3}, new[] {-3,-3}, new[] {+3, 0}, new[] { 0,+3}, new[] { 0,-3}, new[] {-3, 0} }; }
        else if (thisEnemyVariant == "knightDireBeast")    { return new int[][] { }; }
        else if (thisEnemyVariant == "knightCentaur")      { return new int[][] { }; }
        else if (thisEnemyVariant == "knightDullahan")     { return new int[][] { }; }
        else if (thisEnemyVariant == "knightGryphon")      { return new int[][] { }; }
        else if (thisEnemyVariant == "queenMinotaur")      { return new int[][] { new[] {+1, 0}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1, 0}, new[] {+2, 0}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2, 0}, new[] {+3, 0}, new[] { 0,+3}, new[] { 0,-3}, new[] {-3, 0} }; }
        else if (thisEnemyVariant == "queenWyrm")          { return new int[][] { new[] {+1,+1}, new[] {+1,-1}, new[] {-1,+1}, new[] {-1,-1}, new[] {+2, 0}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2, 0}, new[] {+2,+2}, new[] {+2,-2}, new[] {-2,+2}, new[] {-2,-2} }; }
        else if (thisEnemyVariant == "queenAbomination")   { return new int[][] { new[] {+2, 0}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2, 0}, new[] {+3, 0}, new[] { 0,+3}, new[] { 0,-3}, new[] {-3, 0}, new[] {+1,+3}, new[] {+3,+1}, new[] {-1,+3}, new[] {-3,+1}, new[] {+1,-3}, new[] {+3,-1}, new[] {-1,-3}, new[] {-3,-1} }; }
        else if (thisEnemyVariant == "queenGolem")         { return new int[][] { new[] {+1,+1}, new[] {+1,-1}, new[] {-1,+1}, new[] {-1,-1}, new[] {+2,+2}, new[] {+2,-2}, new[] {-2,+2}, new[] {-2,-2}, new[] {+3,+3}, new[] {+3,-3}, new[] {-3,+3}, new[] {-3,-3} }; }
        else if (thisEnemyVariant == "kingLich")           { return new int[][] { new[] {+2,+2}, new[] {+2,-2}, new[] {-2,+2}, new[] {-2,-2}, new[] {+2, 0}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2, 0}, new[] {+3, 0}, new[] { 0,+3}, new[] { 0,-3}, new[] {-3, 0}, new[] {+3,+2}, new[] {+2,+3}, new[] {-3,+2}, new[] {-2,+3}, new[] {+3,-2}, new[] {+2,-3}, new[] {-3,-2}, new[] {-2,-3} }; }
        else if (thisEnemyVariant == "kingTitan")          { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1}, new[] {+2,+2}, new[] {+2, 0}, new[] {+2,-2}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2,+2}, new[] {-2, 0}, new[] {-2,-2}, new[] {+3,+3}, new[] {+3, 0}, new[] {+3,-3}, new[] { 0,+3}, new[] { 0,-3}, new[] {-3,+3}, new[] {-3, 0}, new[] {-3,-3} }; }
        else if (thisEnemyVariant == "kingDragon")         { return new int[][] { new[] {-3,-3}, new[] {-2,-3}, new[] {-1,-3}, new[] { 0,-3}, new[] {+1,-3}, new[] {+2,-3}, new[] {+3,-3},                new[] {-3,-2}, new[] {-2,-2}, new[] {-1,-2}, new[] { 0,-2}, new[] {+1,-2}, new[] {+2,-2}, new[] {+3,-2},                new[] {-3,-1}, new[] {-2,-1}, new[] {-1,-1}, new[] { 0,-1}, new[] {+1,-1}, new[] {+2,-1}, new[] {+3,-1},                new[] {-3, 0}, new[] {-2, 0}, new[] {-1, 0},                new[] {+1, 0}, new[] {+2, 0}, new[] {+3, 0},                new[] {-3,+1}, new[] {-2,+1}, new[] {-1,+1}, new[] { 0,+1}, new[] {+1,+1}, new[] {+2,+1}, new[] {+3,+1},                new[] {-3,+2}, new[] {-2,+2}, new[] {-1,+2}, new[] { 0,+2}, new[] {+1,+2}, new[] {+2,+2}, new[] {+3,+2},                new[] {-3,+3}, new[] {-2,+3}, new[] {-1,+3}, new[] { 0,+3}, new[] {+1,+3}, new[] {+2,+3}, new[] {+3,+3} }; }
        else if (thisEnemyVariant == "kingVampire")        { return new int[][] { }; }
        else {
            Debug.LogWarning("Unknown enemy variant: " + thisEnemyVariant);
            return new int[][] { };
        }
    }

    public int[] GetHealthAttackDelayGold(string thisEnemyVariant)
    {
        string selectedDifficulty = PlayerPrefs.GetString("modeDifficulty", "???");
        if (selectedDifficulty != "Easy" && selectedDifficulty != "Medium" && selectedDifficulty != "Hard" && selectedDifficulty != "Adaptive")
        {
            Debug.Log("Unkown Difficuly Selected: " + selectedDifficulty);
            return new int[] {99,99,99,99};
        }

        // MUST DETERMINE THESE STATS IF ADAPTIVE DIFFICULTY IS CHOSEN. MAYBE A NEW CONTROLLER?
        // From left to right, the returned integers in the array is the enemy's health, attack, delay, and gold

        if      (thisEnemyVariant == "pawnGoblin")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] { 4, 1, 5, 1}; }
            else if (selectedDifficulty == "Medium")   { return new int[] { 6, 2, 4, 1}; }
            else if (selectedDifficulty == "Hard")     { return new int[] { 8, 3, 3, 1}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 1}; }
        }
        else if (thisEnemyVariant == "pawnSkeleton")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] { 3, 2, 4, 1}; }
            else if (selectedDifficulty == "Medium")   { return new int[] { 5, 3, 3, 1}; }
            else if (selectedDifficulty == "Hard")     { return new int[] { 7, 4, 2, 1}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 1}; }
        }
        else if (thisEnemyVariant == "pawnBandit")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] { 7, 3, 6, 1}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {10, 4, 5, 1}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {13, 5, 4, 1}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 1}; }
        }
        else if (thisEnemyVariant == "pawnSlime")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] { 1, 3,99, 1}; }
            else if (selectedDifficulty == "Medium")   { return new int[] { 2, 4,99, 1}; }
            else if (selectedDifficulty == "Hard")     { return new int[] { 4, 5,99, 1}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 1}; }
        }
        else if (thisEnemyVariant == "rookTroll")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] { 8, 4, 4, 2}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {10, 5, 3, 2}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {12, 6, 2, 2}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 2}; }
        }
        else if (thisEnemyVariant == "rookGiant")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] { 9, 4, 5, 2}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {12, 5, 4, 2}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {15, 6, 3, 2}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 2}; }
        }
        else if (thisEnemyVariant == "rookExecutioner")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] { 8, 4, 4, 2}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {10, 6, 3, 2}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {12, 8, 2, 2}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 2}; }
        }
        else if (thisEnemyVariant == "rookBeastMan")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] {12,12, 6, 2}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {15,15, 6, 2}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {18,18, 4, 2}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 2}; }
        }
        else if (thisEnemyVariant == "bishopDarkElf")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] { 4, 4, 5, 2}; }
            else if (selectedDifficulty == "Medium")   { return new int[] { 6, 6, 4, 2}; }
            else if (selectedDifficulty == "Hard")     { return new int[] { 8, 8, 3, 2}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 2}; }
        }
        else if (thisEnemyVariant == "bishopWarlock")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] { 6, 4, 5, 2}; }
            else if (selectedDifficulty == "Medium")   { return new int[] { 8, 5, 4, 2}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {10, 6, 3, 2}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 2}; }
        }
        else if (thisEnemyVariant == "bishopElementalist")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] {10, 2, 6, 2}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {12, 3, 5, 2}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {14, 4, 4, 2}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 2}; }
        }
        else if (thisEnemyVariant == "bishopBomber")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] { 8, 3, 5, 2}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {10, 4, 4, 2}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {12, 5, 3, 2}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 2}; }
        }
        else if (thisEnemyVariant == "knightDireBeast")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] { 9, 4, 3, 3}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {12, 5, 2, 3}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {15, 6, 2, 3}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 3}; }
        }
        else if (thisEnemyVariant == "knightCentaur")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] {12, 5, 4, 3}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {15, 6, 3, 3}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {18, 7, 2, 3}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 3}; }
        }
        else if (thisEnemyVariant == "knightDullahan")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] { 2, 2, 2, 3}; }
            else if (selectedDifficulty == "Medium")   { return new int[] { 3, 3, 1, 3}; }
            else if (selectedDifficulty == "Hard")     { return new int[] { 4, 4, 1, 3}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 3}; }
        }
        else if (thisEnemyVariant == "knightGryphon")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] {11, 4, 4, 3}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {14, 5, 3, 3}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {17, 6, 2, 3}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 3}; }
        }
        else if (thisEnemyVariant == "queenMinotaur")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] {16, 6, 5, 4}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {20, 8, 4, 4}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {24,10, 3, 4}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 4}; }
        }
        else if (thisEnemyVariant == "queenWyrm")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] {15, 7, 6, 4}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {18, 9, 5, 4}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {21,11, 4, 4}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 4}; }
        }
        else if (thisEnemyVariant == "queenAbomination")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] {13, 7, 5, 4}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {16, 9, 4, 4}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {19,11, 3, 4}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 4}; }
        }
        else if (thisEnemyVariant == "queenGolem")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] {18, 7, 5, 4}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {22,10, 4, 4}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {26,13, 3, 4}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 4}; }
        }
        else if (thisEnemyVariant == "kingLich")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] {25, 9, 5, 5}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {30,12, 4, 5}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {35,15, 3, 5}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 5}; }
        }
        else if (thisEnemyVariant == "kingTitan")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] {34, 7, 7, 5}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {40,10, 6, 5}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {46,13, 5, 5}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 5}; }
        }
        else if (thisEnemyVariant == "kingDragon")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] {18, 1,12, 5}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {22, 1,10, 5}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {26, 1, 8, 5}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 5}; }
        }
        else if (thisEnemyVariant == "kingVampire")
        {
            if      (selectedDifficulty == "Easy")     { return new int[] {16, 6, 4, 5}; }
            else if (selectedDifficulty == "Medium")   { return new int[] {20, 8, 3, 5}; }
            else if (selectedDifficulty == "Hard")     { return new int[] {24,10, 2, 5}; }
            else    /* Adaptive */                     { return new int[] {99,99,99, 5}; }
        }
        else
        {
            Debug.LogWarning("Unknown enemy variant: " + thisEnemyVariant);
            return new int[] {99,99,99,99};
        }
    }

    public int[] GetHealthAttackDelayGoldMinAndMax(string thisEnemyVariant)
    {
        string selectedDifficulty = PlayerPrefs.GetString("modeDifficulty", "???");
        if (selectedDifficulty != "Easy" && selectedDifficulty != "Medium" && selectedDifficulty != "Hard" && selectedDifficulty != "Adaptive")
        {
            Debug.Log("Unkown Difficuly Selected: " + selectedDifficulty);
            return new int[] { 99, 99, 99, 99, 99, 99, 99 };
        }

        // MUST DETERMINE THESE STATS IF ADAPTIVE DIFFICULTY IS CHOSEN. MAYBE A NEW CONTROLLER?
        // From left to right, the returned integers in the array is the enemy's easy / hard max health, easy / hard attack, easy / hard delay, and gold

        if      (thisEnemyVariant == "pawnGoblin")         { return new int[] { 4, 8,    1, 3,    5, 3,    1}; }
        else if (thisEnemyVariant == "pawnSkeleton")       { return new int[] { 3, 7,    2, 4,    4, 2,    1}; }
        else if (thisEnemyVariant == "pawnBandit")         { return new int[] { 7,13,    3, 5,    6, 4,    1}; }
        else if (thisEnemyVariant == "pawnSlime")          { return new int[] { 1, 4,    3, 5,   99,99,    1}; }

        else if (thisEnemyVariant == "rookTroll")          { return new int[] { 8,12,    4, 6,    4, 2,    2}; }
        else if (thisEnemyVariant == "rookGiant")          { return new int[] { 9,15,    4, 6,    5, 3,    2}; }
        else if (thisEnemyVariant == "rookExecutioner")    { return new int[] { 8,12,    4, 8,    4, 2,    2}; }
        else if (thisEnemyVariant == "rookBeastMan")       { return new int[] {12,18,   12,18,    6, 4,    2}; }

        else if (thisEnemyVariant == "bishopDarkElf")      { return new int[] { 4, 8,    4, 8,    5, 3,    2}; }
        else if (thisEnemyVariant == "bishopWarlock")      { return new int[] { 6,10,    4, 6,    5, 3,    2}; }
        else if (thisEnemyVariant == "bishopElementalist") { return new int[] {10,14,    2, 4,    6, 4,    2}; }
        else if (thisEnemyVariant == "bishopBomber")       { return new int[] { 8,12,    3, 5,    5, 3,    2}; }

        else if (thisEnemyVariant == "knightDireBeast")    { return new int[] { 9,15,    4, 6,    3, 2,    3}; }
        else if (thisEnemyVariant == "knightCentaur")      { return new int[] {12,18,    5, 7,    4, 2,    3}; }
        else if (thisEnemyVariant == "knightDullahan")     { return new int[] { 2, 4,    2, 4,    2, 1,    3}; }
        else if (thisEnemyVariant == "knightGryphon")      { return new int[] {11,17,    4, 6,    4, 2,    3}; }

        else if (thisEnemyVariant == "queenMinotaur")      { return new int[] {16,24,    6,10,    5, 3,    4}; }
        else if (thisEnemyVariant == "queenWyrm")          { return new int[] {15,21,    7,11,    6, 4,    4}; }
        else if (thisEnemyVariant == "queenAbomination")   { return new int[] {13,19,    7,11,    5, 3,    4}; }
        else if (thisEnemyVariant == "queenGolem")         { return new int[] {18,26,    7,13,    5, 3,    3}; }

        else if (thisEnemyVariant == "kingLich")           { return new int[] {25,35,    9,15,    5, 3,    5}; }
        else if (thisEnemyVariant == "kingTitan")          { return new int[] {34,46,    7,13,    7, 5,    5}; }
        else if (thisEnemyVariant == "kingDragon")         { return new int[] {18,26,    1, 1,   12, 8,    5}; }
        else if (thisEnemyVariant == "kingVampire")        { return new int[] {16,24,    6,10,    4, 2,    5}; }
        else
        {
            Debug.LogWarning("Unknown enemy variant: " + thisEnemyVariant);
            return new int[] { 99, 99, 99, 99, 99, 99, 99 };
        }
    }

    // pawn = 1, rook = 2, bishop = 2, knight = 3, queen = 5, king = 10
    public string[] DecideWhatEnemiesToSpawnThisRound(int enemyPointsToAllocate)
    {
        List<string> enemiesToDeploy = new List<string>();

        // Define the possible enemy types for each point value
        string[] pawnEnemies = { "pawnGoblin", "pawnSkeleton", "pawnBandit", "pawnSlime" };
        string[] rookBishopEnemies = { "rookTroll", "rookGiant", "rookExecutioner", "rookBeastMan", "bishopDarkElf", "bishopWarlock", "bishopElementalist", "bishopBomber" };
        string[] knightEnemies = { "knightDireBeast", "knightCentaur", "knightDullahan", "knightGryphon" };
        string[] queenEnemies = { "queenMinotaur", "queenWyrm", "queenAbomination", "queenGolem" };
        string[] kingEnemies = { "kingLich", "kingTitan", "kingDragon", "kingVampire" };

        int[] pointValues = { 1, 1, 2, 2, 3, 5, 8 };

        // Continue until all points are allocated
        while (enemyPointsToAllocate > 0)
        {
            // Filter point values that do not exceed the remaining points to allocate
            List<int> availablePointValues = new List<int>();
            foreach (int value in pointValues)
            {
                if (value <= enemyPointsToAllocate)
                    availablePointValues.Add(value);
            }

            // Pick a random point value from available options
            int selectedPointValue = availablePointValues[UnityEngine.Random.Range(0, availablePointValues.Count)];
            enemyPointsToAllocate -= selectedPointValue;

            // Select the corresponding enemy based on the point value using if-else ladder
            if (selectedPointValue == 1)
            {
                enemiesToDeploy.Add(pawnEnemies[UnityEngine.Random.Range(0, pawnEnemies.Length)]);
            }
            else if (selectedPointValue == 2)
            {
                enemiesToDeploy.Add(rookBishopEnemies[UnityEngine.Random.Range(0, rookBishopEnemies.Length)]);
            }
            else if (selectedPointValue == 3)
            {
                enemiesToDeploy.Add(knightEnemies[UnityEngine.Random.Range(0, knightEnemies.Length)]);
            }
            else if (selectedPointValue == 5)
            {
                enemiesToDeploy.Add(queenEnemies[UnityEngine.Random.Range(0, queenEnemies.Length)]);
            }
            else if (selectedPointValue == 8)
            {
                enemiesToDeploy.Add(kingEnemies[UnityEngine.Random.Range(0, kingEnemies.Length)]);
            }
        }

        return enemiesToDeploy.ToArray(); // Convert the list to a string array and return it
    }

}
