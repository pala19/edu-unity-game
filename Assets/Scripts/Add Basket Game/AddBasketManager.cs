using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBasketManager : GameManager
{
    private Tuple<int, int, bool> CountableNumber;
    public GameObject TutorialApple;
    public GameObject SkipTutorialBtn;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        TutorialComponents = new GameObject[] { Tap, TutorialApple, SkipTutorialBtn };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckResultAndPrepareRound()
    {
        MainGameData.PressedButton = true;
        int result = Utils.DoMyMath(CountableNumber.Item1, CountableNumber.Item2, CountableNumber.Item3);
        var delay = 1;
        if (result == GameObject.Find("Basket").GetComponent<BasketBehaviour>().getAppleContained())
        {
            ButtonController.GetComponent<CanvasBehaviour>().GoodAnswer();
            Character.GetComponent<CharacterBehaviour>().GoodAnswer();
            AddBasketData.Success += 1;
        }
        else
        {
            ButtonController.GetComponent<CanvasBehaviour>().ShowCorrectAnswer();
            ShowCorrectAnswer();
            Handheld.Vibrate();
            delay = 2;
        }
        StartCoroutine(PrepareWithDelay(delay));
    }

    protected override void VoiceCurrentRound()
    {
        MainGameData.PressedButton = true;
        StartCoroutine(VoiceFirstNumberWithDelay(CountableNumber.Item1));
    }

    IEnumerator VoiceOtherWithDelay(int i, bool IsSign)
    {
        yield return new WaitForSeconds(1f);
        GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayOtherVoice(i);
        if (IsSign)
            StartCoroutine(VoiceNumberWithDelay(CountableNumber.Item2));
    }
    IEnumerator VoiceNumberWithDelay(int i)
    {
        yield return new WaitForSeconds(1f);
        GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayVoice(i - 1);
        StartCoroutine(VoiceOtherWithDelay(0, false));
    }

    IEnumerator VoiceFirstNumberWithDelay(int i)
    {
        yield return new WaitForSeconds(1.5f);
        GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayVoice(i - 1);
        if (CountableNumber.Item3)
            StartCoroutine(VoiceOtherWithDelay(1, true));
        else
            StartCoroutine(VoiceOtherWithDelay(2, true));
    }


    protected override void ChangeGameOverData()
    {  
        AddBasketData.GameOver = true;
    }
    protected override bool CheckIfGameOver()
    {
        return Tuple.Equals(CountableNumber, Tuple.Create(-1, -1));
    }
    protected override void MakeCountablesForRound() { }

    protected override void AssignCountableNumber()
    {
        CountableNumber = AddBasketData.NextRoundSettings;
    }

    protected override void AssignGamesWon() 
    {
        AddBasketData.Round += 1;
        GamesWon = AddBasketData.Success;
    }

    protected override void PrepareButtons() 
    {
        ButtonController.GetComponent<CanvasBehaviour>().PrepareButtons();
    }

    protected override void AdditionalActionsBeforeDestroy(int i) 
    {
    }

    protected override void AdditionalActionsAfterDestroy() 
    {
        if (!CheckIfGameOver())
        GameObject.Find("Basket").GetComponent<BasketBehaviour>().setAppleContained(CountableNumber.Item1, false);
        else
        {
            GameObject.Find("Basket").GetComponent<Animator>().SetTrigger("End");
            GameObject.Find("Pile of Apples").GetComponent<Animator>().SetTrigger("End");
        }
    }

    protected override void ActivateEndScreen() 
    {
        ButtonController.GetComponent<AddBasketCanvasBehaviour>().ActivateEndScreen();
    }

    public void ChangeResult(int number)
    {
        ButtonController.GetComponent<AddBasketCanvasBehaviour>().ChangeNumber(number);
    }
    protected override void ShowCorrectAnswer() 
    {
        var result = Utils.DoMyMath(CountableNumber.Item1, CountableNumber.Item2, CountableNumber.Item3);
        StartCoroutine(ShowCorrectAnswerWithDelay(result));
    }
    IEnumerator ShowCorrectAnswerWithDelay(int result)
    {
        yield return new WaitForSeconds(1.5f);
        ChangeResult(result);
        GameObject.Find("Basket").GetComponent<BasketBehaviour>().setAppleContained(result, false);
        VoiceResult();
    }

    void VoiceResult()
    {
        var result = Utils.DoMyMath(CountableNumber.Item1, CountableNumber.Item2, CountableNumber.Item3);
        GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayVoice(result - 1);
    }

    protected override void SetLastGame()
    {
        MainGameData.LastFinishedGame = 3;
    }


}
