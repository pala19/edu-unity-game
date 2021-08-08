using UnityEngine;
using UnityEngine.Audio;


public class PlayerSettings : MonoBehaviour
{
    public AudioMixer audioMix;
    public GameObject AudioMix;
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
        LoadLanguageSettings();
        LoadDefaultMusicSettings();
        LoadDefaultSfxSettings();
        LoadDefaultVoiceSettings();
    }

    void LoadLanguageSettings()
    {
        if (!PlayerPrefs.HasKey("language"))
        {
            int languageValue = 0;
            if (Application.systemLanguage == SystemLanguage.Polish)
            {
                MainGameData.changeLanguage = SystemLanguage.Polish;
            }
            else
            {
                MainGameData.changeLanguage = SystemLanguage.English;
                languageValue = 1;
            }
            SaveLanguageSettings(languageValue);
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
    void LoadDefaultMusicSettings()
    {
        if (!PlayerPrefs.HasKey("musicVol"))
        {
            PlayerPrefs.SetFloat("musicVol", 0.75f);
        }
    }
    public void SaveMusicSettings(float musicVol)
    {
        PlayerPrefs.SetFloat("musicVol", musicVol);
    }
    void LoadDefaultSfxSettings()
    {
        if (!PlayerPrefs.HasKey("sfxVol"))
        {
            PlayerPrefs.SetFloat("sfxVol", 0.75f);
        }
    }
    public void SaveSfxSettings(float sfxVol)
    {
        PlayerPrefs.SetFloat("sfxVol", sfxVol);
    }
    void LoadDefaultVoiceSettings()
    {
        if (!PlayerPrefs.HasKey("voiceVol"))
        {
            PlayerPrefs.SetFloat("voiceVol", 0.75f);
        }
    }
    public void SaveVoiceSettings(float voiceVol)
    {
        PlayerPrefs.SetFloat("voiceVol", voiceVol);
    }

    public void SaveLanguageSettings(int languageValue)
    {
        PlayerPrefs.SetInt("language", languageValue);
    }

    public void SavePrefs()
    {
        PlayerPrefs.Save();
    }


}
