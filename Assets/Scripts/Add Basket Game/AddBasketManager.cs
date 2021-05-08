using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBasketManager : GameManager
{
    private Tuple<int, int> CountableNumber;
    //public GameObject TutorialCountable1;
   // public GameObject TutorialCountable2;
   // public GameObject SkipTutorialBtn; later
    private int GamesWon;
    // Start is called before the first frame update
    void Start()
    {
        GamesWon = 0;
        PrepareForNextRound();
       // TutorialComponents = new GameObject[] { Tap, TutorialCountable1, TutorialCountable2, SkipTutorialBtn };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckResultAndPrepareRound()
    {
        MainGameData.PressedButton = true;
        int result = CountableNumber.Item1 + CountableNumber.Item2;
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
            delay = result + 1;
        }
        StartCoroutine(PrepareWithDelay(delay));
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
        GameObject.Find("Basket").GetComponent<BasketBehaviour>().setAppleContained(CountableNumber.Item1);
    }

    protected override void ActivateEndScreen() 
    {
        ButtonController.GetComponent<AddBasketCanvasBehaviour>().ActivateEndScreen();
    }

    public void ChangeResult(int number)
    {
        ButtonController.GetComponent<AddBasketCanvasBehaviour>().ChangeNumber(number);
    }
    protected override void ShowCorrectAnswer() { }

}
