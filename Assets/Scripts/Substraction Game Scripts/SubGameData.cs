using System;

public static class SubGameData
{
    private static readonly int[,] First = { 
        { 2, 3, 4, 5, 3, 4, 5, 4, 5 },
        { 5, 5, 5, 6, 6, 6, 5, 6, 6 },
        { 7, 7, 7, 7, 7, 8, 8, 8, 8 },
        { 7, 8, 8, 8, 9, 9, 9, 9, 9 },
        { 9, 9, 9, 5, 6, 5, 4, 3, 2 },
        { 3, 4, 5, 6, 7, 8, 9, 2, 5 }
    };
    private static readonly int[,] Second = {
        { 1, 1, 1, 1, 2, 2, 2, 3, 3 },
        { 1, 2, 3, 1, 2, 3, 4, 4, 5},
        { 1, 2, 3, 4, 5, 1, 2, 3, 4},
        { 6, 5, 6, 7, 1, 2, 3, 4, 5 },
        { 6, 7, 8, 3, 2, 2, 3, 1, 1 },
        { 1, 1, 2, 3, 2, 1, 1, 1, 4 }
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
            return AddGameData.IsCompleted;
        }
        return FinishedRounds[i - 1];

    }

    public static Tuple<int, int> CurrentRoundSettings
    {
        get
        {
            if (CurrentRound < 9)
            {
                return Tuple.Create(First[CurrentGame, PermutatedRound[CurrentRound]], Second[CurrentGame, PermutatedRound[CurrentRound]]);
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
            if (CurrentRound + 1 < 9)
            {
                if (CurrentRound == -1)
                {
                    PermutatedRound = Utils.GenerateRandomPermutation(First.GetUpperBound(1));
                }
                return Tuple.Create(First[CurrentGame, PermutatedRound[CurrentRound + 1]], Second[CurrentGame, PermutatedRound[CurrentRound + 1]]);
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