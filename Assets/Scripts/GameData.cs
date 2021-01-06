using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    private static int[] Rounds = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private static int CurrentRound = -1;
    private static bool ButtonPressedFlag = false;


    public static int CurrentRoundSettings
    {
        get
        {
            if (CurrentRound < Rounds.Length)
            {
                return Rounds[CurrentRound];
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
            if (CurrentRound + 1 < Rounds.Length)
            {
                return Rounds[CurrentRound+1];
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
  
}