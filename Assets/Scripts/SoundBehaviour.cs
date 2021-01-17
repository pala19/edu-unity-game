using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBehaviour : MonoBehaviour
{
    private AudioSource[] PolishVoices;
    private AudioSource[] EnglishVoices;
    // Start is called before the first frame update
    void Start()
    {
        PolishVoices = new AudioSource[9];
        EnglishVoices = new AudioSource[9];
        AudioSource[] audios = GetComponents<AudioSource>();
        for (int i=0; i< audios.Length; i++)
        {
            if (i < 9)
                PolishVoices[i] = audios[i];
            else
                EnglishVoices[i % 9] = audios[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayVoice(int i)
    {
        if (MainGameData.changeLanguage == SystemLanguage.Polish)
            PolishVoices[i].Play();
        else
            EnglishVoices[i].Play();
    }

}
