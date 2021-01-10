using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameData = AddGameData;

public class CanvasBehaviour : MonoBehaviour
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
    public GameObject First;
    public GameObject Second;
    public GameObject Result;
   public GameObject FireworksEffect1;
    public GameObject FireworksEffect2;
    AudioSource correctAudio;
    AudioSource errorAudio;
    // Start is called before the first frame update
    void Start()
    {
        EndScreen.SetActive(false);
        DeactivateFireworks();
        AudioSource[] audios = GetComponents<AudioSource>();
        correctAudio = audios[0];
        errorAudio = audios[1];
        //PlayedNumber = 0;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PrepareButtons()
    {
        StartCoroutine(PrepareButtonsWithDelay());
    }

    private void InstantiateButtons()
    {
        var current = GameData.CurrentRoundSettings;
        string name1 = "Button" + current.Item1 + "Prefab";
        string name2 = "Button" + current.Item2 + "Prefab";
        var button1 = this.GetType().GetField(name1).GetValue(this) as GameObject;
        var button2 = this.GetType().GetField(name2).GetValue(this) as GameObject;
        First.GetComponent<Image>().sprite = button1.GetComponent<Image>().sprite;
        Second.GetComponent<Image>().sprite = button2.GetComponent<Image>().sprite;
        Result.GetComponent<Image>().enabled = false;
    }
    
    public void ChangeNumber(int number)
    {
        Debug.Log(number);
        if (number != 0)
        {
            Result.GetComponent<Image>().enabled = true;
            string name1 = "Button" + number + "Prefab";
            var button1 = this.GetType().GetField(name1).GetValue(this) as GameObject;
            Result.GetComponent<Image>().sprite = button1.GetComponent<Image>().sprite;
        }
        else
            Result.GetComponent<Image>().enabled = false;

    }

    IEnumerator PrepareButtonsWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        InstantiateButtons();
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
      public void ShowCorrectAnswer()
    {
        PlayFailureMusic();
        int number = GameData.CurrentRoundSettings.Item1 + GameData.CurrentRoundSettings.Item2;
        string name1 = "Button" + number + "Prefab";
        var button1 = this.GetType().GetField(name1).GetValue(this) as GameObject;
        Result.GetComponent<Image>().sprite = button1.GetComponent<Image>().sprite;

    }
    private void DeactivateFireworks()
    {
        FireworksEffect1.SetActive(false);
        FireworksEffect2.SetActive(false);
    }
    public void GoodAnswer()
    {
        PlaySuccessMusic();
        FireworksEffect1.SetActive(true);
        FireworksEffect2.SetActive(true);
    }
    private void PlaySuccessMusic()
    {
        correctAudio.Play();

    }
    private void PlayFailureMusic()
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
    public void ResultPressed()
    {
        GameObject.Find("Game").GetComponent<AddGameManager>().CheckResultAndPrepareRound();
    }
}
