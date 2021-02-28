using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SubGameData
{
    private static readonly int[,] first = { 
        { 2, 3, 4, 6, 7, 7, 9, 6, 8 },
        { 9, 4, 5, 8, 2, 8, 3, 4, 9 } 
    };
    private static readonly int[,] second = {
        { 1, 2, 2, 3, 1, 5, 2, 3, 4 },
        { 5, 1, 1, 6, 1, 4, 2, 1, 1} 
    };
    private static bool[] FinishedRounds = { false };
    private static int[] successRate = { 0 };
    private static int currentSuccessRate;
    private static int CurrentRound = -1;
    private static bool ButtonPressedFlag = false;
    private static System.Random random = new System.Random();
    private static int RandomizedGame = -1;


    public static Tuple<int, int> CurrentRoundSettings
    {
        get
        {
            
            if (CurrentRound < 9)
            {
                return Tuple.Create(first[RandomizedGame,CurrentRound], second[RandomizedGame,CurrentRound]);
            }
            else
            {
                return Tuple.Create(-1, -1);
            }

        }
    }
    public static Tuple<int, int> NextRoundSettings
    {
        get
        {
            if (RandomizedGame == -1)
            {
                RandomizedGame = random.Next(0, FinishedRounds.Length-1);
                while (!FinishedRounds[RandomizedGame] && FinishedRounds.Any(round => round.Equals(true)))
                    RandomizedGame = random.Next(0, FinishedRounds.Length - 1); 
            }

            if (CurrentRound + 1 < 9)
            {
                return Tuple.Create(first[RandomizedGame, CurrentRound + 1], second[RandomizedGame, CurrentRound + 1]);
            }
            else
            {
                return Tuple.Create(-1, -1);
            }
        }
    }
    public static int Round
    {
        get
        {
            return CurrentRound;
        }
        set
        {
            CurrentRound = value;
        }
    }
    public static int Success
    {
        get
        {
            return currentSuccessRate;
        }
        set
        {

            currentSuccessRate = value;
        }
    }
    public static bool PressedButton
    {
        get
        {
            return ButtonPressedFlag;
        }
        set
        {
            ButtonPressedFlag = value;
        }
    }
    public static bool GameOver
    {
        set
        {
            if (currentSuccessRate > successRate[RandomizedGame])
                successRate[RandomizedGame] = currentSuccessRate;
            if (successRate[RandomizedGame] == 9)
                FinishedRounds[RandomizedGame] = true; 
            currentSuccessRate = 0;
            CurrentRound = -1;
        }
    }

}