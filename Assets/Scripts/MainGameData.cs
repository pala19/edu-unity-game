using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MainGameData
{
    private static SystemLanguage gameLanguage;
    private static bool ButtonPressedFlag = true;
    private static int lastGame = 0;
    private static bool FirstLaunched = true;

    public static SystemLanguage changeLanguage
    {
        set
        {
            gameLanguage = value;
        }
        get
        {
            return gameLanguage;
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
    public static int LastFinishedGame
    {
        get
        {
            return lastGame;
        }
        set
        {
            lastGame = value;
        }
    }

    public static bool FirstOpen
    {
        get
        {
            return FirstLaunched;
        }
        set
        {
            FirstLaunched = value;
        }
    }
}
