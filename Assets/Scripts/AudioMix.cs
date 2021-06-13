using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMix : MonoBehaviour
{
    public AudioMixer audioMix;
    // Start is called before the first frame update

    public void SetMusicLvl(float musicLvl)
    {
        audioMix.SetFloat("musicLvl", Mathf.Log10(musicLvl)*20); 
    }
    public void SetSfxLvl(float sfxLvl)
    {
        audioMix.SetFloat("sfxLvl", sfxLvl);
    }
}
