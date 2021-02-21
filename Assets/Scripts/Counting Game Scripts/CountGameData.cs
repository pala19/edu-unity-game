using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CountGameData
{
    private static int[,] Rounds = { { 1, 2, 3, 4, 5, 6, 7, 8, 9 } };
    private static bool[] FinishedRounds = { false };
    private static int[] successRate = { 0 };
    private static int currentSuccessRate;
    private static int CurrentRound = -1;
    private static bool ButtonPressedFlag = true;
    private static System.Random random = new System.Random();
    private static int RandomizedGame = -1;

    public static int CurrentRoundSettings
    {
        get
        {
            if (CurrentRound < 9)
            {
                return Rounds[RandomizedGame, CurrentRound];
            }
            else
            {
                return -1;
            }

        }
    }
    public static int NextRoundSettings
    {
        get
        {
            if (RandomizedGame == -1)
            {
                RandomizedGame = random.Next(0, FinishedRounds.Length - 1);
                while (!FinishedRounds[RandomizedGame] && FinishedRounds.Any(round => round.Equals(true)))
                    RandomizedGame = random.Next(0, FinishedRounds.Length - 1);
            }

            if (CurrentRound + 1 < 9)
            {
                return Rounds[RandomizedGame, CurrentRound+1];
            }
            else
            {
                return -1;
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
    public static bool GameOver
    {
        set
        {
            if (currentSuccessRate > successRate[RandomizedGame])
                successRate[RandomizedGame] = currentSuccessRate;
            if (successRate[RandomizedGame] == 9)
                FinishedRounds[RandomizedGame] = true;
            RandomizedGame = -1;
            currentSuccessRate = 0;
            CurrentRound = -1;
        }
    }

}