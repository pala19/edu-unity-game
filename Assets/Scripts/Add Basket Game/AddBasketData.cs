using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AddBasketData
{
    private static readonly int[,] First = {
        { 1, 1, 2, 1, 3, 1, 3, 2, 2 },
        { 9, 3, 7, 5, 6, 2, 6, 4, 8 },
        { 3, 4, 5, 6, 7, 8, 1, 8, 2 },
        { 9, 3, 5, 6, 5, 3, 8, 1, 3 },
        { 2, 5, 9, 4, 5, 6, 7, 8, 2 },
        { 2, 3, 4, 5, 6, 7, 8, 1, 9 }
    };
    private static readonly int[,] Second = {
        { 1, 2, 2, 3, 1, 3, 1, 3, 2 },
        { 2, 3, 4, 2, 1, 2, 3, 1, 2 },
        { 1, 3, 3, 2, 1, 1, 5, 4, 3 },
        { 8, 6, 2, 3, 4, 2, 6, 4, 2 },
        { 1, 2, 3, 4, 3, 2, 1, 1, 2 },
        { 1, 1, 1, 1, 1, 1, 1, 1, 8 }

    };
    private static bool[,] IsPlus = { 
        {true, true, true, true, true, true, true, true, true},
        {false, true, false, true, false, true, false, true, false},
        {true, false, true, false, true, false, true, false, true},
        {false, true, false, true, false, true, false, true, false},
        {false, true, false, true, false, true, false, true, false},
        {false, true, false, true, false, true, false, true, false}
    };
    private static bool[] FinishedRounds = { false, false, false, false, false, false };
    private static int[] SuccessRate = { 0, 0, 0, 0, 0, 0 };
    private static int CurrentSuccessRate;
    private static int CurrentRound = -1;
    private static int CurrentGame;
    private static bool Completed = false;
    private static int[] PermutatedRound;

    public static int SetCurrentGame
    {
        set
        {
            CurrentGame = value;
        }
    }

    public static int GetCurrentGame
    {
        get
        {
            return CurrentGame;
        }
    }

    public static bool IsCompleted
    {
        get
        {
            return Completed;
        }
    }

    public static bool IsActive(int i)
    {
        if (i == 0)
        {
            return SubGameData.IsCompleted;
        }
        return FinishedRounds[i - 1];

    }

    public static Tuple<int, int, bool> CurrentRoundSettings
    {
        get
        {
            if (CurrentRound < 9)
            {
                return Tuple.Create(First[CurrentGame, PermutatedRound[CurrentRound]], Second[CurrentGame, PermutatedRound[CurrentRound]], IsPlus[CurrentGame, PermutatedRound[CurrentRound]]);
            }
            else
            {
                return Tuple.Create(-1, -1, false);
            }

        }
    }
    public static Tuple<int, int, bool> NextRoundSettings
    {
        get
        {
            if (CurrentRound + 1 < 9)
            {
                if (CurrentRound == -1)
                {
                    PermutatedRound = Utils.GenerateRandomPermutation(First.GetUpperBound(1));
                }
                return Tuple.Create(First[CurrentGame, PermutatedRound[CurrentRound + 1]], Second[CurrentGame, PermutatedRound[CurrentRound + 1]], IsPlus[CurrentGame, PermutatedRound[CurrentRound+1]]);
            }
            else
            {
                return Tuple.Create(-1, -1, false);
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
            return CurrentSuccessRate;
        }
        set
        {

            CurrentSuccessRate = value;
        }
    }
    public static bool GameOver
    {
        set
        {
            if (CurrentSuccessRate > SuccessRate[CurrentGame])
                SuccessRate[CurrentGame] = CurrentSuccessRate;
            if (SuccessRate[CurrentGame] == 9)
            {
                FinishedRounds[CurrentGame] = true;
                if (CurrentGame == FinishedRounds.Length - 1)
                    Completed = true;
            }
            CurrentSuccessRate = 0;
            CurrentRound = -1;
        }
    }
}