using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonsController : MonoBehaviour
{
    public GameObject Button1Prefab;
    public GameObject Button2Prefab;
    public GameObject Button3Prefab;
    public GameObject Button4Prefab;
    public GameObject Button5Prefab;
    public GameObject Button6Prefab;
    public GameObject Button7Prefab;
    public GameObject Button8Prefab;
    public GameObject Button9Prefab;
    public GameObject EndScreen;
    private GameObject[] ActiveButtons;
    private int PlayedNumber;
    AudioSource correctAudio;
    AudioSource errorAudio;
    // Start is called before the first frame update
    void Start()
    {
        EndScreen.SetActive(false);
        AudioSource[] audios = GetComponents<AudioSource>();
        correctAudio = audios[0];
        errorAudio = audios[1];
        PlayedNumber = 0;
        ActiveButtons = new GameObject[3];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PrepareButtons()
    {
        DestroyOldButtons();
        PlayedNumber = GameData.CurrentRoundSettings;
        StartCoroutine(PrepareButtonsWithDelay());
    }

    private void InstantiateButtons()
    {
        int number = 0;
        if (PlayedNumber > 0 && PlayedNumber < 4)
            number = 1;
        if (PlayedNumber > 3 && PlayedNumber < 7)
            number = 4;
        if (PlayedNumber > 6 && PlayedNumber < 10)
            number = 7;
        for (int i=0; i< 3; i++)
        {
            string name = "Button" + number + "Prefab";
            var button = this.GetType().GetField(name).GetValue(this) as GameObject;
            ActiveButtons[i] = Instantiate(button, gameObject.transform.position, Quaternion.identity);
            ActiveButtons[i].transform.SetParent(gameObject.transform);
            number++;
        }
    }
    private void PositionNumbers()
    {
        for (int i=0; i<3; i++)
        {
            Vector3 pos =new Vector3(-Screen.currentResolution.width / 4 + Screen.currentResolution.width / 4 * i, Screen.currentResolution.height / 8, 0);
            ActiveButtons[i].transform.localPosition = pos;
        }
    }
    IEnumerator PrepareButtonsWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        InstantiateButtons();
        PositionNumbers();
        GameData.PressedButton = false;
    }
    public void ActivateEndScreen()
    {
        StartCoroutine(ActivateEndScreenWithDelay());
    }
    IEnumerator ActivateEndScreenWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        EndScreen.SetActive(true);
    }

    public void DestroyOldButtons()
    {
        if (ActiveButtons[0] != null)
        {
            for (int i=0; i<3;i++)
            {
                ActiveButtons[i].GetComponent<SingleButtonBehaviour>().SetEndTrigger();
                StartCoroutine(DestroyWithDelay(i));
            }
        }
    }
    IEnumerator DestroyWithDelay(int i)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(ActiveButtons[i]);
    }
    public void PlaySuccessMusic()
    {
        correctAudio.Play();
    }
    public void PlayFailureMusic()
    {
        errorAudio.Play();
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
