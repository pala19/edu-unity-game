using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddBasketCanvasBehaviour : CanvasBehaviour
{
    public GameObject First;
    public GameObject Second;
    public GameObject PlusSign;
    public GameObject MinusSign;
    public GameObject EqualSign;
    public GameObject Result;
    GameObject[] Buttons;
    GameObject[] Signs;
    // Start is called before the first frame update
    void Start()
    {
        EndScreen.SetActive(false);
        DeactivateFireworks();
        DeactivateFireworks();
        AddAudioSourcesToArray();
        Buttons = new GameObject[] { First, Second, EqualSign, Result };
        Signs = new GameObject[] { PlusSign, MinusSign };

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void DestroyOldButtons() { }

    protected override void InstantiateButtons()
    {
        ActivateButtons();
        Result.SetActive(false);
        var current = AddBasketData.CurrentRoundSettings;
        First.GetComponent<SVGImage>().sprite = ButtonPrefabs[current.Item1-1].GetComponent<SVGImage>().sprite;
        Second.GetComponent<SVGImage>().sprite = ButtonPrefabs[current.Item2-1].GetComponent<SVGImage>().sprite;
    }

    private void ActivateButtons()
    {
        for (int i = 0; i < Buttons.Length - 1; i++)
            Buttons[i].SetActive(true);
        if (AddBasketData.CurrentRoundSettings.Item3)
            Signs[0].SetActive(true);
        else
            Signs[1].SetActive(true);
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
    public void ResultPressed()
    {
        GameObject.Find("Game").GetComponent<AddBasketManager>().CheckResultAndPrepareRound();
    }

    public override void GoodAnswer()
    {
        PlaySuccessMusic();
        ActivateFireworks();
        Result.GetComponent<Animator>().SetBool("Pressed", true);
    }

    public override void ShowCorrectAnswer()
    {
        PlayFailureMusic();
        Result.GetComponent<Animator>().SetBool("Pressed", true);
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
            foreach (GameObject button in Signs)
            {
                if (button.activeSelf)
                {
                    button.GetComponent<Animator>().SetTrigger("End");
                    StartCoroutine(DeactivateButtonWithDelay(button));
                }
            }
        }
    }
    IEnumerator DeactivateButtonWithDelay(GameObject button)
    {
        yield return new WaitForSeconds(1f);
        button.SetActive(false);
    }

    public override void PlayAgain() 
    {
        SceneManager.LoadScene(4);
    }

    public override void Exit() 
    {
        AddBasketData.SetCurrentGame = AddBasketData.GetCurrentGame + 1;
        SceneManager.LoadScene(4);
    }

    protected override void CheckIfNextGameEnabled()
    {
        if (!AddBasketData.IsActive(AddBasketData.GetCurrentGame + 1) || AddBasketData.IsCompleted)
        {
            EndScreen.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
