using System;
static class AddGameData
{
    private static readonly int[,] First = { 
        { 1, 1, 2, 1, 3, 1, 3, 2, 2 },
        { 2, 3, 4, 1, 1, 2, 3, 4, 5 },
        { 3, 4, 5, 6, 7, 8, 1, 2, 2 },
        { 1, 3, 5, 6, 5, 3, 1, 2, 3 },
        { 1, 2, 3, 4, 5, 6, 7, 8, 2 },
        { 2, 3, 4, 5, 6, 7, 8, 1, 1 }
    };
    private static readonly int[,] Second = {
        { 1, 2, 2, 3, 1, 3, 1, 3, 2 },
        { 1, 3, 4, 2, 1, 2, 3, 1, 2 },
        { 1, 3, 3, 2, 1, 1, 5, 4, 3 },
        { 8, 6, 2, 3, 4, 2, 6, 4, 2 },
        { 1, 2, 3, 4, 3, 2, 1, 1, 2 },
        { 1, 1, 1, 1, 1, 1, 1, 1, 1 }

    };
    private static bool[] FinishedRounds = { false, false, false, false, false, false };
    private static int[] SuccessRate = { 0, 0, 0, 0, 0, 0 };
    private static int CurrentSuccessRate;
    private static int CurrentRound = -1;
    private static bool ButtonPressedFlag = false;
    private static int CurrentGame;
    private static bool Completed = false;

    public static int SetCurrentGame
    {
        set
        {
            CurrentGame = value;
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
            return CountGameData.IsCompleted;
        }
        return FinishedRounds[i - 1];

    }

    public static Tuple<int,int> CurrentRoundSettings
    {
        get
        {
            if (CurrentRound < 9)
            {
                return Tuple.Create(First[CurrentGame, CurrentRound], Second[CurrentGame, CurrentRound]);
            }
            else
            {
                return Tuple.Create (-1, -1);
            }

        }
    }
    public static Tuple<int, int> NextRoundSettings
    {
        get
        {


            if (CurrentRound + 1 < 9)
            {
                return Tuple.Create(First[CurrentGame, CurrentRound + 1], Second[CurrentGame, CurrentRound + 1]);
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

