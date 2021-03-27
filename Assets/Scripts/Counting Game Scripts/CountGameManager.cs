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
    private int GamesWon;
    // Start is called before the first frame update
    void Start()
    {
        GamesWon = 0;
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

    protected override void MakeCountablesForRound()
    {
        Countables = new GameObject[CountableNumber];
        for (int i = 0; i< CountableNumber; i++)
        {
            Vector3 pos = new Vector3(0f,1f,90f);
            float height = Screen.currentResolution.height / 4;
            float width;
            if (CountableNumber > 5)
                width = Screen.currentResolution.width / 6;
            else
                width = Screen.currentResolution.width / (CountableNumber+1);
            if (i > 4)
            {
                width = Screen.currentResolution.width / (CountableNumber - 4);
                float pom = i - 5;
                Countables[i] = Instantiate(CountablePrefab, pos, Quaternion.identity);
                Countables[i].transform.SetParent(GameObject.Find("Background").transform);
                Countables[i].transform.localPosition = new Vector3((-Screen.currentResolution.width / 2) + ((pom + 1) * width), -height*1.6f, 0);
            }
            else
            {
                Countables[i] = Instantiate(CountablePrefab, pos, Quaternion.identity);
                Countables[i].transform.SetParent(GameObject.Find("Background").transform);
                Countables[i].transform.localPosition = new Vector3((-Screen.currentResolution.width / 2) + ((i + 1) * width), -height/1.1f, 0);
            }
        }
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

    protected override void ChangePressedButton()
    {
        CountGameData.PressedButton = !CountGameData.PressedButton;
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

    protected override void TurnOffTutorial()
    {
        CountGameData.TutorialShown = true;
    }

    protected override bool CheckIfTutorialPlayed()
    {
        return CountGameData.TutorialShown;
    }
}
