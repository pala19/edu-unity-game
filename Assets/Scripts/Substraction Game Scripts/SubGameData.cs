using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SubGameData
{
    private static int[] first = { 2, 3, 4, 6, 7, 7, 9, 6, 8 };
    private static int[] second = { 1, 2, 2, 3, 1, 5, 2, 3, 4 };
    private static int CurrentRound = -1;
    private static int successRate = 0;
    private static bool ButtonPressedFlag = false;

    public static Tuple<int, int> CurrentRoundSettings
    {
        get
        {
            if (CurrentRound < first.Length)
            {
                return Tuple.Create(first[CurrentRound], second[CurrentRound]);
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

            if (CurrentRound + 1 < first.Length)
            {
                return Tuple.Create(first[CurrentRound + 1], second[CurrentRound + 1]);
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
            return successRate;
        }
        set
        {
            successRate = value;
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