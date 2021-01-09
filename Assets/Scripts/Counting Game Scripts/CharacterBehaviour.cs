using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    AudioSource GoodAnswerAudio;
    AudioSource WinnerAudio;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] audios = GetComponents<AudioSource>();
        GoodAnswerAudio = audios[0];
        WinnerAudio = audios[1];
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoodAnswer()
    {
        GoodAnswerAudio.Play();
        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(DisactivateWithDelay(0));
    }
    public void Winner()
    {
        WinnerAudio.Play();
        transform.GetChild(1).gameObject.SetActive(true);
        StartCoroutine(DisactivateWithDelay(1));
    }
    IEnumerator DisactivateWithDelay(int i)
    {
        float delay = 3.0f;
        yield return new WaitForSeconds(delay);
        transform.GetChild(i).gameObject.SetActive(false);
        
    }

}
