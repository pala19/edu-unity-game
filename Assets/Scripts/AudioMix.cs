using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMix : MonoBehaviour
{
    public AudioMixer audioMix;
    public GameObject MusicSlider, SfxSlider, VoiceSlider;
    public Image OnMusic, OffMusic, OnSfx, OffSfx, OnVoice, OffVoice;
    private AudioSource[] audios;
    // Start is called before the first frame update

    private void Start()
    {
        audios = GetComponents<AudioSource>();
    }

    public void SetMusicLvl(float musicLvl)
    {
        audioMix.SetFloat("musicLvl", Mathf.Log10(musicLvl)*20);
        if (musicLvl == MusicSlider.GetComponent<Slider>().minValue)
            MusicSlider.transform.GetChild(3).gameObject.GetComponent<Image>().sprite = OffMusic.sprite;
        else
            MusicSlider.transform.GetChild(3).gameObject.GetComponent<Image>().sprite = OnMusic.sprite;
    }

    public void SetSfxLvl(float sfxLvl)
    {
        audioMix.SetFloat("sfxLvl", Mathf.Log10(sfxLvl) * 20);
        if (sfxLvl == SfxSlider.GetComponent<Slider>().minValue)
            SfxSlider.transform.GetChild(3).gameObject.GetComponent<Image>().sprite = OffSfx.sprite;
        else
            SfxSlider.transform.GetChild(3).gameObject.GetComponent<Image>().sprite = OnSfx.sprite;

        audios[0].Play();
    }

    public void SetVoiceLvl(float voiceLvl)
    {
        audioMix.SetFloat("voiceLvl", Mathf.Log10(voiceLvl) * 20);
        if (voiceLvl == VoiceSlider.GetComponent<Slider>().minValue)
            VoiceSlider.transform.GetChild(3).gameObject.GetComponent<Image>().sprite = OffVoice.sprite;
        else
            VoiceSlider.transform.GetChild(3).gameObject.GetComponent<Image>().sprite = OnVoice.sprite;

        if (MainGameData.changeLanguage == SystemLanguage.Polish)
            audios[1].Play();
        else
            audios[2].Play();
    }


    public void OnMusicBtnClick()
    {
        var value = MusicSlider.GetComponent<Slider>().maxValue;

        if (MusicSlider.GetComponent<Slider>().value != MusicSlider.GetComponent<Slider>().minValue)
        {
            value = MusicSlider.GetComponent<Slider>().minValue;
        }

        SetMusicLvl(value);
        MusicSlider.GetComponent<Slider>().value = value;

    }
    public void OnSfxBtnClick()
    {
        var value = SfxSlider.GetComponent<Slider>().maxValue;

        if (SfxSlider.GetComponent<Slider>().value != SfxSlider.GetComponent<Slider>().minValue)
        {
            value = SfxSlider.GetComponent<Slider>().minValue;
        }

        SetSfxLvl(value);
        SfxSlider.GetComponent<Slider>().value = value;
    }

    public void OnVoiceBtnClick()
    {
        var value = VoiceSlider.GetComponent<Slider>().maxValue;

        if (VoiceSlider.GetComponent<Slider>().value != VoiceSlider.GetComponent<Slider>().minValue)
        {
            value = VoiceSlider.GetComponent<Slider>().minValue;
        }

        SetVoiceLvl(value);
        VoiceSlider.GetComponent<Slider>().value = value;
    }
}
