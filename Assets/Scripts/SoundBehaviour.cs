using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBehaviour : MonoBehaviour
{
    private AudioSource[] PolishNumbers;
    private AudioSource[] EnglishNumbers;
    private AudioSource[] PolishOthers;
    private AudioSource[] EnglishOthers;
    // Start is called before the first frame update
    void Start()
    {
        PolishNumbers = new AudioSource[9];
        EnglishNumbers = new AudioSource[9];
        PolishOthers = new AudioSource[3];
        EnglishOthers = new AudioSource[3];

        AudioSource[] audios = GetComponents<AudioSource>();
 
        for (int i=0; i< audios.Length; i++)
        {            
            if (i < 9)
                PolishNumbers[i] = audios[i];
            else if (i < 18)
                EnglishNumbers[i % 9] = audios[i];
            else            
            {
                if (i % 2 != 0)
                    PolishOthers[(i-1) % 3] = audios[i];
                else
                    EnglishOthers[i % 3] = audios[i];
            }      
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayVoice(int i)
    {
        if (MainGameData.changeLanguage == SystemLanguage.Polish)
            PolishNumbers[i].Play();
        else
            EnglishNumbers[i].Play();
    }
    public void PlayOtherVoice(int i)
    {
        if (MainGameData.changeLanguage == SystemLanguage.Polish)
            PolishOthers[i].Play();
        else
            EnglishOthers[i].Play();
    }

}
