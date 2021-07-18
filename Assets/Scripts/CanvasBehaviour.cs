using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasBehaviour : MonoBehaviour
{
    public GameObject[] ButtonPrefabs;
    public GameObject EndScreen;
    public GameObject FireworksEffect1;
    public GameObject FireworksEffect2;
    protected GameObject[] ActiveButtons;
    protected AudioSource correctAudio;
    protected AudioSource errorAudio;
    protected int PlayedNumber;
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    protected void AddAudioSourcesToArray()
    {
        AudioSource[] audios = GetComponents<AudioSource>();
        correctAudio = audios[0];
        errorAudio = audios[1];
    }

    public void PrepareButtons()
    {
        DeactivateFireworks();
        DestroyOldButtons();
        HideButtons();
        AssignPlayedNumber();
        StartCoroutine(PrepareButtonsWithDelay());
    }    
    public void PrepareButtons(int SelectedCountables)
    {
        DeactivateFireworks();
        HideButtons();
        StartCoroutine(PrepareButtonsWithDelay(SelectedCountables));
    }
    IEnumerator PrepareButtonsWithDelay(int SelectedCountables)
    {

        yield return new WaitForSeconds(1.0f);
        InstantiateButtons(SelectedCountables);
    }

    IEnumerator PrepareButtonsWithDelay()
    {
        yield return new WaitForSeconds(1.0f);
        InstantiateButtons();
        DeactivateFireworks();
        PositionNumbers();
    
    }

    protected virtual void InstantiateButtons() { }

    protected virtual void InstantiateButtons(int SelectedCountables) { }

    private void PositionNumbers()
    {
        if (ActiveButtons != null)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3((Screen.currentResolution.width / 4)*(i+1), Screen.currentResolution.height * 7/ 8, GameObject.Find("Background").transform.position.z));

                pos.z = GameObject.Find("Background").transform.position.z;               

                ActiveButtons[i].transform.localPosition = GameObject.Find("Background").transform.InverseTransformPoint(pos);
            }

        }
    }

    public virtual void DestroyOldButtons()
    {
        if (ActiveButtons != null && ActiveButtons[0] != null)
        {
            for (int i = 0; i < 3; i++)
            {
                ActiveButtons[i].GetComponent<SingleButtonBehaviour>().SetEndTrigger();
                ActiveButtons[i].GetComponent<SingleButtonBehaviour>().PlayDissapearSound();
                Destroy(ActiveButtons[i], 2.0f);
            }
        }
    }
    protected void DeactivateFireworks()
    {
        FireworksEffect1.SetActive(false);
        FireworksEffect2.SetActive(false);
    }
    protected void ActivateFireworks()
    {
        FireworksEffect1.SetActive(true);
        FireworksEffect2.SetActive(true);
    }

    public virtual void ShowCorrectAnswer() { }

    public virtual void GoodAnswer()
    {
        PlaySuccessMusic();
        ActivateFireworks();
        StartCoroutine(PlaySoundWithDelay());
    }
    public void ActivateEndScreen()
    {
        StartCoroutine(ActivateEndScreenWithDelay());
    }
    IEnumerator ActivateEndScreenWithDelay()
    {
        HideButtons();
        yield return new WaitForSeconds(2.0f);
        EndScreen.SetActive(true);
        CheckIfNextGameEnabled();
    }

    protected virtual void HideButtons() { }

    IEnumerator PlaySoundWithDelay()
    {
        yield return new WaitForSeconds(1.0f);
        GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayVoice(PlayedNumber - 1);
    }
    protected virtual void PlaySuccessMusic()
    {
        correctAudio.Play();
    }

    protected virtual void PlayFailureMusic()
    {
        errorAudio.Play();
    }

    public virtual void PlayAgain() {}

    public void Home() 
    {
        SceneManager.LoadScene(0);
    }

    public virtual void Exit() { }

    protected virtual void AssignPlayedNumber() { }

    protected virtual void CheckIfNextGameEnabled() { }


}
