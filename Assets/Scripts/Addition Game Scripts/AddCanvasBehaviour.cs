﻿using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AddCanvasBehaviour : CanvasBehaviour
{
    public GameObject First;
    public GameObject Second;
    public GameObject PlusSign;
    public GameObject EqualSign;
    public GameObject Result;
    GameObject[] Buttons;
    // Start is called before the first frame update
    void Start()
    {
        EndScreen.SetActive(false);
        DeactivateFireworks();
        DeactivateFireworks();
        AddAudioSourcesToArray();
        Buttons = new GameObject[] { First, Second, PlusSign, EqualSign, Result };
    }

    // Update is called once per frame
    void Update() {}

    public override void DestroyOldButtons() {}

    protected override void InstantiateButtons()
    {
        ActivateButtons();
        Result.SetActive(false);
        var current = AddGameData.CurrentRoundSettings;
        First.GetComponent<SVGImage>().sprite = ButtonPrefabs[current.Item1-1].GetComponent<SVGImage>().sprite;
        Second.GetComponent<SVGImage>().sprite = ButtonPrefabs[current.Item2-1].GetComponent<SVGImage>().sprite;
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
        SceneManager.LoadScene(2);
    }
    public override void Exit()
    {
        AddGameData.SetCurrentGame = AddGameData.GetCurrentGame + 1;
        SceneManager.LoadScene(2);
    }
    public void ResultPressed()
    {
        GameObject.Find("Game").GetComponent<AddGameManager>().CheckResultAndPrepareRound();
    }

    protected override void HideButtons()
    {
        if (Buttons[0].activeSelf)
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
        if (!AddGameData.IsActive(AddGameData.GetCurrentGame + 1) || AddGameData.IsCompleted)
        {
            EndScreen.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
