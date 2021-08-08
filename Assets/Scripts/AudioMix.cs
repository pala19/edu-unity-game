using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMix : MonoBehaviour
{
    public AudioMixer audioMix;
    public GameObject MainMenu;
    public GameObject MusicSlider, SfxSlider, VoiceSlider;
    public GameObject ENG, PL;
    public Image OnMusic, OffMusic, OnSfx, OffSfx, OnVoice, OffVoice;
    public Material Disabled;
    private AudioSource[] audios;
    private bool MusicMutedByButton = false;
    private float previousMusicValue = 0.75f;
    private bool SfxMutedByButton = false;
    private float previousSfxValue = 0.75f;
    private bool VoiceMutedByButton = false;
    private float previousVoiceValue = 0.75f;
    // Start is called before the first frame update

    private void Start()
    {
        audios = GetComponents<AudioSource>();
        SetSliders();
        SetLanguage();
    }

    public void SetMusicLvl(float musicLvl)
    {
        audioMix.SetFloat("musicLvl", Mathf.Log10(musicLvl)*20);

        ChangeSprite(musicLvl, MusicSlider, OnMusic, OffMusic);

        MainMenu.GetComponent<PlayerSettings>().SaveMusicSettings(musicLvl);
    }

    public void SetSfxLvl(float sfxLvl)
    {
        audioMix.SetFloat("sfxLvl", Mathf.Log10(sfxLvl) * 20);
        ChangeSprite(sfxLvl, SfxSlider, OnSfx, OffSfx);

        if (audios != null)
            audios[0].Play();

        MainMenu.GetComponent<PlayerSettings>().SaveSfxSettings(sfxLvl);
    }

    public void SetVoiceLvl(float voiceLvl)
    {
        audioMix.SetFloat("voiceLvl", Mathf.Log10(voiceLvl) * 20);

        ChangeSprite(voiceLvl, VoiceSlider, OnVoice, OffVoice);

        if (audios != null)
        {
            if (MainGameData.changeLanguage == SystemLanguage.Polish)
                audios[1].Play();
            else
                audios[2].Play();
        }

        MainMenu.GetComponent<PlayerSettings>().SaveVoiceSettings(voiceLvl);
    }

    void ChangeSprite(float lvl, GameObject Slider, Image OnSprite, Image OffSprite)
    {
        if (lvl == Slider.GetComponent<Slider>().minValue)
            Slider.transform.GetChild(3).gameObject.GetComponent<Image>().sprite = OffSprite.sprite;
        else
            Slider.transform.GetChild(3).gameObject.GetComponent<Image>().sprite = OnSprite.sprite;
    }

    public void OnMusicBtnClick()
    {
        var value = MusicSlider.GetComponent<Slider>().maxValue;

        if (MusicMutedByButton)
        {
            MusicMutedByButton = false;
            value = previousMusicValue;
        }

        if (MusicSlider.GetComponent<Slider>().value != MusicSlider.GetComponent<Slider>().minValue)
        {
            MusicMutedByButton = true;
            previousMusicValue = MusicSlider.GetComponent<Slider>().value;
            value = MusicSlider.GetComponent<Slider>().minValue;
        }

        SetMusicLvl(value);
        MusicSlider.GetComponent<Slider>().value = value;

    }
    public void OnSfxBtnClick()
    {
        var value = SfxSlider.GetComponent<Slider>().maxValue;

        if (SfxMutedByButton)
        {
            SfxMutedByButton = false;
            value = previousSfxValue;
        }

        if (SfxSlider.GetComponent<Slider>().value != SfxSlider.GetComponent<Slider>().minValue)
        {
            SfxMutedByButton = true;
            previousSfxValue = SfxSlider.GetComponent<Slider>().value;
            value = SfxSlider.GetComponent<Slider>().minValue;            
        }

        SetSfxLvl(value);
        SfxSlider.GetComponent<Slider>().value = value;
    }

    public void OnVoiceBtnClick()
    {
        var value = VoiceSlider.GetComponent<Slider>().maxValue;

        if (VoiceMutedByButton)
        {
            VoiceMutedByButton = false;
            value = previousVoiceValue;
        }

        if (VoiceSlider.GetComponent<Slider>().value != VoiceSlider.GetComponent<Slider>().minValue)
        {
            VoiceMutedByButton = true;
            previousVoiceValue = VoiceSlider.GetComponent<Slider>().value;
            value = VoiceSlider.GetComponent<Slider>().minValue;
        }

        SetVoiceLvl(value);
        VoiceSlider.GetComponent<Slider>().value = value;
    }

    public void OnClickChangeLanguage(int languageValue)
    {
        
        if (languageValue == 0)
        {
            ENG.GetComponent<SVGImage>().material = Disabled;
            PL.GetComponent<SVGImage>().material = null;
            MainGameData.changeLanguage = SystemLanguage.Polish;
            
        }
        else
        {
            ENG.GetComponent<SVGImage>().material = null;
            PL.GetComponent<SVGImage>().material = Disabled;
            MainGameData.changeLanguage = SystemLanguage.English;
        }
        MainMenu.GetComponent<PlayerSettings>().SaveLanguageSettings(languageValue);
    }


    public void SetSliders()
    {
        ChangeAudioEnabled(false);

        float value;
        if (!PlayerPrefs.HasKey("musicVol"))
        {
            value = 0.75f;
        }
        else
        {
            value = PlayerPrefs.GetFloat("musicVol");
        }
        MusicSlider.GetComponent<Slider>().value = value;

        if (!PlayerPrefs.HasKey("sfxVol"))
        {
            value = 0.75f;
        }
        else
        {
            value = PlayerPrefs.GetFloat("sfxVol");
        }
        SfxSlider.GetComponent<Slider>().value = value;

        if (!PlayerPrefs.HasKey("voiceVol"))
        {
           value = 0.75f;
        }
        else
        {
            value = PlayerPrefs.GetFloat("voiceVol");
        }      
        VoiceSlider.GetComponent<Slider>().value = value;

        ChangeAudioEnabled(true);
    }

    public void SetLanguage()
    {
        if (!PlayerPrefs.HasKey("language"))
        {
            if (Application.systemLanguage == SystemLanguage.Polish)
            {
                ENG.GetComponent<SVGImage>().material = Disabled;
            }
            else
            {
                PL.GetComponent<SVGImage>().material = Disabled;
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("language") == 0)
                ENG.GetComponent<SVGImage>().material = Disabled;
            else
                PL.GetComponent<SVGImage>().material = Disabled;
        }
    }

    void ChangeAudioEnabled(bool enabled)
    {
        foreach (AudioSource audio in audios)
            audio.enabled = enabled;
    }
}
