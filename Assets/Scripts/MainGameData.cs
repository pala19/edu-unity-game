using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MainGameData
{
    private static SystemLanguage gameLanguage = changeLanguage = Application.systemLanguage;
    private static bool ButtonPressedFlag = true;
    private static int lastGame = 0;

    public static SystemLanguage changeLanguage
    {
        set
        {
            if (value == SystemLanguage.English || value == SystemLanguage.Polish)
                gameLanguage = value;
            else
                gameLanguage = SystemLanguage.English;
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
}
