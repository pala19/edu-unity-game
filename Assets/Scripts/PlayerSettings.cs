using UnityEngine;
using UnityEngine.Audio;


public class PlayerSettings : MonoBehaviour
{
    public AudioMixer audioMix;
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
                MainGameData.changeLanguage = SystemLanguage.Polish;
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
        float musicVol;
        if (!PlayerPrefs.HasKey("musicVol"))
        {
            audioMix.GetFloat("musicVol", out musicVol);
            PlayerPrefs.SetFloat("musicVol", musicVol);
            PlayerPrefs.Save();
        }
        else
        {
            musicVol = PlayerPrefs.GetFloat("musicVol");
            audioMix.SetFloat("musicVol", musicVol);
        }
        float sfxVol;
        if (!PlayerPrefs.HasKey("sfxVol"))
        {
            audioMix.GetFloat("sfxVol", out sfxVol);
            PlayerPrefs.SetFloat("sfxVol", sfxVol);
            PlayerPrefs.Save();
        }
        else
        {
            sfxVol = PlayerPrefs.GetFloat("sfxVol");
            audioMix.SetFloat("sfxVol", sfxVol);
        }

    }
}
