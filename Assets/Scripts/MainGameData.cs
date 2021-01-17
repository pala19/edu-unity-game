using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class MainGameData
{
    private static SystemLanguage gameLanguage = changeLanguage = Application.systemLanguage;

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
}
