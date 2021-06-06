using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Awake()
    {
        var languageValue = 0;
        if (!PlayerPrefs.HasKey("language"))
        {
            if (Application.systemLanguage == SystemLanguage.Polish)
            {
                MainGameData.changeLanguage = SystemLanguage.Polish;;
            }
            else
            {
                MainGameData.changeLanguage = SystemLanguage.English;
                languageValue = 1;
            }
            PlayerPrefs.SetInt("language", languageValue);
            PlayerPrefs.Save();                
        }
        else
        {
            if (PlayerPrefs.GetInt("language") == 0)
                MainGameData.changeLanguage = SystemLanguage.Polish;
            else
                MainGameData.changeLanguage = SystemLanguage.English;
        }       

    }
}
