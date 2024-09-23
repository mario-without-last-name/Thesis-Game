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
    {
        if      (thisEnemyVariant == "pawnGoblin")         { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "pawnSkeleton")       { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "pawnBandit")         { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "pawnSlime")          { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "rookTroll")          { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "rookGiant")          { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "rookExecutioner")    { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "rookBeastMan")       { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1}, new[] {+2,+1}, new[] {+2, 0}, new[] {+2,-1}, new[] {+1,-2}, new[] { 0,-2}, new[] {-2,-2}, new[] {-2,+1}, new[] {-2, 0}, new[] {+2,-1}, new[] {+1,+2}, new[] { 0,+2}, new[] {+2,+2} }; }
        else if (thisEnemyVariant == "bishopDarkElf")      { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "bishopWarlock")      { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "bishopElementalist") { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "bishopBomber")       { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1} }; }
        else if (thisEnemyVariant == "knightDireBeast")    { return new int[][] { new[] {+1,+2}, new[] {+2,+1}, new[] {+2,-1}, new[] {+1,-2}, new[] {-1,-2}, new[] {-2,-1}, new[] {-2,+1}, new[] {-1,+2} }; }
        else if (thisEnemyVariant == "knightCentaur")      { return new int[][] { new[] {+1,+2}, new[] {+2,+1}, new[] {+2,-1}, new[] {+1,-2}, new[] {-1,-2}, new[] {-2,-1}, new[] {-2,+1}, new[] {-1,+2} }; }
        else if (thisEnemyVariant == "knightDullahan")     { return new int[][] { new[] {+1,+3}, new[] {+2,+2}, new[] {+3,+1}, new[] {+1,-3}, new[] {+2,-2}, new[] {+3,-1}, new[] {-1,+3}, new[] {-2,+2}, new[] {-3,+1}, new[] {-1,-3}, new[] {-2,-2}, new[] {-3,-1} }; }
        else if (thisEnemyVariant == "knightGryphon")      { return new int[][] { new[] {+1, 0}, new[] {+2, 0}, new[] {+3, 0}, new[] {-1, 0}, new[] {-2, 0}, new[] {-3, 0}, new[] { 0,+1}, new[] { 0,+2}, new[] { 0,+3}, new[] { 0,-1}, new[] { 0,-2}, new[] { 0,-3}, new[] {+1,+1}, new[] {+2,+2}, new[] {+3,+3}, new[] {+1,-1}, new[] {+2,-2}, new[] {+3,-3}, new[] {-1,+1}, new[] {-2,+2}, new[] {-3,+3}, new[] {-1,-1}, new[] {-2,-2}, new[] {-3,-3} }; }
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
        else if (thisEnemyVariant == "pawnSlime")          { return new int[][] { }; }
        else if (thisEnemyVariant == "rookTroll")          { return new int[][] { new[] {+1, 0}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1, 0}, new[] {+2, 0}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2, 0} }; }
        else if (thisEnemyVariant == "rookGiant")          { return new int[][] { new[] {+1,+1}, new[] {+1,-1}, new[] {-1,+1}, new[] {-1,-1}, new[] {+2,+2}, new[] {+2,-2}, new[] {-2,+2}, new[] {-2,-2} }; }
        else if (thisEnemyVariant == "rookExecutioner")    { return new int[][] { new[] {+1,+1}, new[] {+1,-1}, new[] {-1,+1}, new[] {-1,-1}, new[] {+2, 0}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2, 0} }; }
        else if (thisEnemyVariant == "rookBeastMan")       { return new int[][] { }; }
        else if (thisEnemyVariant == "bishopDarkElf")      { return new int[][] { new[] {+2,+3}, new[] {+3,+2}, new[] {-2,+3}, new[] {-3,+2}, new[] {+2,-3}, new[] {+3,-2}, new[] {-2,-3}, new[] {-3,-2} }; }
        else if (thisEnemyVariant == "bishopWarlock")      { return new int[][] { new[] {+1,+3}, new[] {+3,+1}, new[] {-1,+3}, new[] {-3,+1}, new[] {+1,-3}, new[] {+3,-1}, new[] {-1,-3}, new[] {-3,-1} }; }
        else if (thisEnemyVariant == "bishopElementalist") { return new int[][] { new[] {+2,+2}, new[] {+2,-2}, new[] {-2,+2}, new[] {-2,-2}, new[] {+2, 0}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2, 0} }; }
        else if (thisEnemyVariant == "bishopBomber")       { return new int[][] { new[] {+3,+3}, new[] {+3,-3}, new[] {-3,+3}, new[] {-3,-3}, new[] {+3, 0}, new[] { 0,+3}, new[] { 0,-3}, new[] {-3, 0} }; }
        else if (thisEnemyVariant == "knightDireBeast")    { return new int[][] { }; }
        else if (thisEnemyVariant == "knightCentaur")      { return new int[][] { new[] {+1, 0}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1, 0} }; }
        else if (thisEnemyVariant == "knightDullahan")     { return new int[][] { }; }
        else if (thisEnemyVariant == "knightGryphon")      { return new int[][] { }; }
        else if (thisEnemyVariant == "queenMinotaur")      { return new int[][] { new[] {+1, 0}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1, 0}, new[] {+2, 0}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2, 0}, new[] {+3, 0}, new[] { 0,+3}, new[] { 0,-3}, new[] {-3, 0} }; }
        else if (thisEnemyVariant == "queenWyrm")          { return new int[][] { new[] {+1,+1}, new[] {+1,-1}, new[] {-1,+1}, new[] {-1,-1}, new[] {+2, 0}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2, 0}, new[] {+3,+3}, new[] {+3,-3}, new[] {-3,+3}, new[] {-3,-3} }; }
        else if (thisEnemyVariant == "queenAbomination")   { return new int[][] { new[] {+2, 0}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2, 0}, new[] {+3, 0}, new[] { 0,+3}, new[] { 0,-3}, new[] {-3, 0}, new[] {+1,+3}, new[] {+3,+1}, new[] {-1,+3}, new[] {-3,+1}, new[] {+1,-3}, new[] {+3,-1}, new[] {-1,-3}, new[] {-3,-1} }; }
        else if (thisEnemyVariant == "queenGolem")         { return new int[][] { new[] {+1,+1}, new[] {+1,-1}, new[] {-1,+1}, new[] {-1,-1}, new[] {+2,+2}, new[] {+2,-2}, new[] {-2,+2}, new[] {-2,-2}, new[] {+3,+3}, new[] {+3,-3}, new[] {-3,+3}, new[] {-3,-3} }; }
        else if (thisEnemyVariant == "kingLich")           { return new int[][] { new[] {+2,+2}, new[] {+2,-2}, new[] {-2,+2}, new[] {-2,-2}, new[] {+2, 0}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2, 0}, new[] {+3, 0}, new[] { 0,+3}, new[] { 0,-3}, new[] {-3, 0}, new[] {+3,+2}, new[] {+2,+3}, new[] {-3,+2}, new[] {-2,+3}, new[] {+3,-2}, new[] {+2,-3}, new[] {-3,-2}, new[] {2,-3} }; }
        else if (thisEnemyVariant == "kingTitan")          { return new int[][] { new[] {+1,+1}, new[] {+1, 0}, new[] {+1,-1}, new[] { 0,+1}, new[] { 0,-1}, new[] {-1,+1}, new[] {-1, 0}, new[] {-1,-1}, new[] {+2,+2}, new[] {+2, 0}, new[] {+2,-2}, new[] { 0,+2}, new[] { 0,-2}, new[] {-2,+2}, new[] {-2, 0}, new[] {-2,-2}, new[] {+3,+3}, new[] {+3, 0}, new[] {+3,-3}, new[] { 0,+3}, new[] { 0,-3}, new[] {-3,+3}, new[] {-3, 0}, new[] {-3,-3} }; }
        else if (thisEnemyVariant == "kingDragon")         { return new int[][] { new[] {-3,-3}, new[] {-2,-3}, new[] {-1,-3}, new[] { 0,-3}, new[] {+1,-3}, new[] {+2,-3}, new[] {+3,-3},                new[] {-3,-2}, new[] {-2,-2}, new[] {-1,-2}, new[] { 0,-2}, new[] {+1,-2}, new[] {+2,-2}, new[] {+3,-2},                new[] {-3,-1}, new[] {-2,-1}, new[] {-1,-1}, new[] { 0,-1}, new[] {+1,-1}, new[] {+2,-1}, new[] {+3,-1},                new[] {-3, 0}, new[] {-2, 0}, new[] {-1, 0},                new[] {+1, 0}, new[] {+2, 0}, new[] {+3, 0},                new[] {-3,+1}, new[] {-2,+1}, new[] {-1,+1}, new[] { 0,+1}, new[] {+1,+1}, new[] {+2,+1}, new[] {+3,+1},                new[] {-3,+2}, new[] {-2,+2}, new[] {-1,+2}, new[] { 0,+2}, new[] {+1,+2}, new[] {+2,+2}, new[] {+3,+2},                new[] {-3,+3}, new[] {-2,+3}, new[] {-1,+3}, new[] { 0,+3}, new[] {+1,+3}, new[] {+2,+3}, new[] {+3,+3} }; }
        else if (thisEnemyVariant == "kingVampire")        { return new int[][] { }; }
        else {
            Debug.LogWarning("Unknown enemy variant: " + thisEnemyVariant);
            return new int[][] { };
        }
    }

    public int[] GetHealthAttackDelay()
    {
        return new int[] { 1, 2, 3 };
    }
}
