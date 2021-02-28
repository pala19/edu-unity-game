using System;
using System.Collections;
using UnityEngine;

public class SubGameManager : GameManager
{
    private Tuple<int, int> CountableNumber;
    public GameObject TutorialCountable1;
    public GameObject TutorialCountable2;
    public GameObject TutorialCountable3;
    private int GamesWon;
    private int SelectedCountables;

    // Start is called before the first frame update
    void Start()
    {
        SubGameData.PressedButton = true;
        GamesWon = 0;
        TutorialComponents = new GameObject[] { Tap, TutorialCountable1, TutorialCountable2, TutorialCountable3 };
    }

    // Update is called once per frame
    void Update() {}

    public void ChangeSelected(int id)
    {
        if (Countables[id].GetComponent<SubCountableBehaviour>().IsSelected())
            SelectedCountables++;
        else
            SelectedCountables--;
        ButtonController.GetComponent<SubCanvasBehaviour>().ChangeNumber(SelectedCountables);
        if (SelectedCountables > 0)
            GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayVoice(SelectedCountables - 1);
    }
    public void CheckResultAndPrepareRound()
    {
        SubGameData.PressedButton = true;
        int result = CountableNumber.Item1 - CountableNumber.Item2;
        var delay = 1;
        if (result == SelectedCountables)
        {
            ButtonController.GetComponent<SubCanvasBehaviour>().GoodAnswer();
            Character.GetComponent<CharacterBehaviour>().GoodAnswer();
        }
        else
        {
            ButtonController.GetComponent<SubCanvasBehaviour>().ShowCorrectAnswer();
            ShowCorrectAnswer();
            Handheld.Vibrate();
            delay = result + 1;
        }
        StartCoroutine(PrepareWithDelay(delay));
    }

    protected override void MakeCountablesForRound()
    {
        Countables = new GameObject[CountableNumber.Item1];
        for (int i = 0; i < CountableNumber.Item1; i++)
        {
            Vector3 pos = new Vector3(0f, 1f, 90f);
            float height = Screen.currentResolution.height / 4;
            float width;
            if (CountableNumber.Item1 > 5)
                width = Screen.currentResolution.width / 6;
            else
                width = Screen.currentResolution.width / (CountableNumber.Item1 + 1);
            if (i > 4)
            {
                width = Screen.currentResolution.width / (CountableNumber.Item1 - 4);
                float pom = i - 5;
                Countables[i] = Instantiate(CountablePrefab, pos, Quaternion.identity);
                Countables[i].transform.SetParent(GameObject.Find("Background").transform);
                Countables[i].transform.localPosition = new Vector3((-Screen.currentResolution.width / 2) + ((pom + 1) * width), -height * 1.6f, 0);
            }
            else
            {
                Countables[i] = Instantiate(CountablePrefab, pos, Quaternion.identity);
                Countables[i].transform.SetParent(GameObject.Find("Background").transform);
                Countables[i].transform.localPosition = new Vector3((-Screen.currentResolution.width / 2) + ((i + 1) * width), -height / 1.1f, 0);
            }
            Countables[i].GetComponent<SubCountableBehaviour>().SetId(i);
        }
    }
    protected override void ShowCorrectAnswer()
    {
        StartCoroutine(TellTheAnswerWithDelay());
        for (int i = 0; i < CountableNumber.Item1; i++)
        {
            Countables[i].GetComponent<SubCountableBehaviour>().SelectWithoutTelling();
        }
        for (int i=0; i < CountableNumber.Item2; i++)
        {
            StartCoroutine(ShowWithDelay(i));
        }

    }
    IEnumerator TellTheAnswerWithDelay()
    {
        var number = CountableNumber.Item1;
        SelectedCountables = number;
        yield return new WaitForSeconds(1.0f);
        GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayVoice(number - 1);
        ButtonController.GetComponent<SubCanvasBehaviour>().ChangeNumber(number);
    }
    IEnumerator ShowWithDelay(int i)
    {
        yield return new WaitForSeconds(1.0f * (i + 2));
        Countables[i].GetComponent<SubCountableBehaviour>().Select();
    }
    protected override void ChangeGameOverData() 
    {
        SubGameData.GameOver = true; 
    }

    protected override bool CheckIfGameOver()
    {
        return Tuple.Equals(CountableNumber, Tuple.Create(-1, -1));
    }

    protected override void AssignCountableNumber() 
    {
        CountableNumber = SubGameData.NextRoundSettings;
    }

    protected override void AssignGamesWon()
    {
        SubGameData.Round += 1;
        GamesWon = SubGameData.Success;

    }
    protected override void ChangePressedButton() 
    {
        SubGameData.PressedButton = !SubGameData.PressedButton;
    }

    protected override void PrepareButtons() 
    {
        SelectedCountables = CountableNumber.Item1;
        ButtonController.GetComponent<SubCanvasBehaviour>().PrepareButtons(SelectedCountables);
    }

    protected override void AdditionalActionsBeforeDestroy(int i) 
    {
        Countables[i].GetComponent<Animator>().SetTrigger("End");
        Countables[i].GetComponents<AudioSource>()[1].Play();
    }

    protected override void AdditionalActionsAfterDestroy() 
    {
        SelectedCountables = 0;
    }
    protected override void ActivateEndScreen()
    {
        ButtonController.GetComponent<SubCanvasBehaviour>().ActivateEndScreen();
    }
}
