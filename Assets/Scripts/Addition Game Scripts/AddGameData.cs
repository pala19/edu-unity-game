using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class AddGameData
{
    private static int[] first = { 1, 1, 2, 1, 3, 2, 3, 3, 3 };
    private static int[] second = { 1, 2, 2, 3, 1, 3, 2, 3, 4 };
    private static int CurrentRound = -1;
    private static int successRate = 0;
    private static bool ButtonPressedFlag = false;

    public static Tuple<int,int> CurrentRoundSettings
    {
        get
        {
            if (CurrentRound < first.Length)
            {
                return Tuple.Create(first[CurrentRound], second[CurrentRound]);
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

            if (CurrentRound + 1 < first.Length)
            {
                return Tuple.Create(first[CurrentRound+1], second[CurrentRound+1]);
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

