using System;

public static class CountGameData
{
    private static readonly int[,] Rounds = { 
        { 1, 2, 3, 2, 3, 1, 3, 2, 1 },
        { 4, 5, 6, 4, 5, 4, 6, 5, 4 },
        { 1, 4, 6, 2, 3, 6, 5, 3, 4 },
        { 7, 8, 9, 7, 9, 8, 7, 9, 8 },
        { 1, 4, 7, 2, 5, 8, 3, 6, 9 },
        { 2, 6, 9, 1, 4, 7, 8, 3, 5 }
    };
    private static bool[] FinishedRounds = { false, false, false, false, false, false };
    private static int[] SuccessRate = { 0, 0, 0, 0, 0, 0 };
    private static int CurrentSuccessRate;
    private static int CurrentRound = -1;
    private static int CurrentGame;
    private static bool Completed = false;
    private static int[] PermutatedRound;

    public static int[] GetSuccessRate
    {
        get
        {
            return SuccessRate;
        }
    }

    public static void SetSuccessRate(int element, int value)
    {
        SuccessRate[element] = value;
        if (value == 9)
            FinishedRounds[element] = true;
    }

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
            return true;
        return FinishedRounds[i-1];
    }

    public static int CurrentRoundSettings
    {
        get
        {
            if (CurrentRound < 9)
            {
                return Rounds[CurrentGame, PermutatedRound[CurrentRound]];
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
            if (CurrentRound + 1 < 9)
            {
                if (CurrentRound == -1)
                {
                    PermutatedRound = Utils.GenerateRandomPermutation(Rounds.GetUpperBound(1));
                }
                return Rounds[CurrentGame, PermutatedRound[CurrentRound+1]];
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