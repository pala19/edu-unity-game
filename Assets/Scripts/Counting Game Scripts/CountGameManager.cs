using System.Collections;
using UnityEngine;

public class CountGameManager : GameManager
{

    public GameObject TutorialCountable1;
    public GameObject TutorialCountable2;
    public GameObject TutorialButton1;
    public GameObject TutorialButton2;
    public GameObject TutorialButton3;
    public GameObject SkipTutorialBtn;
    private int CountableNumber;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        TutorialComponents = new GameObject[] { Tap, TutorialCountable1, TutorialCountable2, TutorialButton1, TutorialButton2, TutorialButton3, SkipTutorialBtn };
    }


    // Update is called once per frame
    void Update() {}

    public void PrepareRound(bool failure)
    {
        if (failure)
        {
            ButtonController.GetComponent<CountCanvasBehaviour>().ShowCorrectAnswer();
            ShowCorrectAnswer();
            StartCoroutine(PrepareWithDelay(CountableNumber));
            Handheld.Vibrate();
        }
        else
        {
            CountGameData.Success += 1;
            Character.GetComponent<CharacterBehaviour>().GoodAnswer();
            ButtonController.GetComponent<CountCanvasBehaviour>().GoodAnswer();
            StartCoroutine(PrepareWithDelay(1));
        }
    }

    protected override void SetCountablesNumber()
    {
        CountablesNumber = CountableNumber;
    }

    protected override void ChangeGameOverData()
    {
        CountGameData.GameOver = true;
    }

    protected override void AssignCountableNumber() 
    {
        CountableNumber = CountGameData.NextRoundSettings;
    }

    protected override bool CheckIfGameOver()
    {
        return CountableNumber == -1;
    }

    protected override void AssignGamesWon()
    {
        CountGameData.Round += 1;
        GamesWon = CountGameData.Success;
    }

    protected override void PrepareButtons()
    {
        ButtonController.GetComponent<CountCanvasBehaviour>().PrepareButtons();
    }

    protected override void AdditionalActionsBeforeDestroy(int i)
    {
        Countables[i].GetComponent<Animator>().SetTrigger("End");
    }
    protected override void ActivateEndScreen()
    {
    ButtonController.GetComponent<CountCanvasBehaviour>().ActivateEndScreen();
    ButtonController.GetComponent<CountCanvasBehaviour>().DestroyOldButtons();
    }

    protected override void SetLastGame()
    {
        MainGameData.LastFinishedGame = 0;
    }
}
