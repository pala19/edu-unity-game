using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SubCanvasBehaviour : CanvasBehaviour
{

    public GameObject First;
    public GameObject Second;
    public GameObject MinusSign;
    public GameObject EqualSign;
    public GameObject Result;
    private GameObject[] Buttons;
    // Start is called before the first frame update
    void Start()
    {

        EndScreen.SetActive(false);
        DeactivateFireworks();
        AddAudioSourcesToArray();
        Buttons = new GameObject[] { First, Second, MinusSign, EqualSign, Result };
    }

    // Update is called once per frame
    void Update() {}

    protected override void InstantiateButtons(int SelectedCountables)
    {
        ActivateButtons();
        Result.SetActive(false);
        var current = SubGameData.CurrentRoundSettings;
        First.GetComponent<SVGImage>().sprite = ButtonPrefabs[current.Item1-1].GetComponent<SVGImage>().sprite;
        Second.GetComponent<SVGImage>().sprite = ButtonPrefabs[current.Item2-1].GetComponent<SVGImage>().sprite;
        ChangeNumber(SelectedCountables);
    }

    public void ChangeNumber(int number)
    {
        if (number != 0)
        {
            Result.SetActive(true);
            Result.GetComponent<SVGImage>().sprite = ButtonPrefabs[number-1].GetComponent<SVGImage>().sprite;
        }
        else
            Result.SetActive(false);
    }

    public override void ShowCorrectAnswer()
    {
        PlayFailureMusic();
        Result.GetComponent<Animator>().SetBool("Pressed", true);

    }

    public override void GoodAnswer()
    {
        PlaySuccessMusic();
        ActivateFireworks();
        Result.GetComponent<Animator>().SetBool("Pressed", true);
    }
    
    public override void PlayAgain()
    {
        SceneManager.LoadScene(3);
    }

    public override void Exit()
    {
        SubGameData.SetCurrentGame = SubGameData.GetCurrentGame + 1;
        SceneManager.LoadScene(3);
    }

    public void ResultPressed()
    {
        GameObject.Find("Game").GetComponent<SubGameManager>().CheckResultAndPrepareRound();
    }

    protected override void HideButtons()
    { 
        if (Buttons != null && Buttons[0].activeSelf)
        {
            foreach (GameObject button in Buttons)
            {
                button.GetComponent<Animator>().SetTrigger("End");
                StartCoroutine(DeactivateButtonWithDelay(button));
            }
        }
    }
    IEnumerator DeactivateButtonWithDelay(GameObject button)
    {
        yield return new WaitForSeconds(1f);
        button.SetActive(false);
    }
    private void ActivateButtons()
    {
        for (int i = 0; i < Buttons.Length - 1; i++)
            Buttons[i].SetActive(true);
    }

    protected override void CheckIfNextGameEnabled()
    {
        if (!SubGameData.IsActive(SubGameData.GetCurrentGame + 1) || SubGameData.IsCompleted)
        {
            EndScreen.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    protected override int GetRoundSuccessRate()
    {
        return SubGameData.Success;
    }
}
