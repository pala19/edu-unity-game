using System;
using System.Collections;
using UnityEngine;

public class SubGameManager : GameManager
{
    private Tuple<int, int> CountableNumber;
    public GameObject TutorialCountable1;
    public GameObject TutorialCountable2;
    public GameObject TutorialCountable3;
    public GameObject SkipTutorialBtn;
    private int SelectedCountables;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();      
        TutorialComponents = new GameObject[] { Tap, TutorialCountable1, TutorialCountable2, TutorialCountable3, SkipTutorialBtn };
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
       MainGameData.PressedButton = true;
        int result = Utils.DoMyMath(CountableNumber.Item1, CountableNumber.Item2, false);
        var delay = 1;
        if (result == SelectedCountables)
        {
            ButtonController.GetComponent<SubCanvasBehaviour>().GoodAnswer();
            Character.GetComponent<CharacterBehaviour>().GoodAnswer();
            SubGameData.Success += 1;
        }
        else
        {
            ButtonController.GetComponent<SubCanvasBehaviour>().ShowCorrectAnswer();
            ShowCorrectAnswer();
            Handheld.Vibrate();
            delay = CountableNumber.Item2 + 1;
        }
        StartCoroutine(PrepareWithDelay(delay));
    }

    protected override void VoiceCurrentRound()
    {
        StartCoroutine(VoiceFirstNumberWithDelay(CountableNumber.Item1 - 1));
    }

    IEnumerator VoiceOtherWithDelay(int i, bool IsMinusSign)
    {
        yield return new WaitForSeconds(1f);
        GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayOtherVoice(i);
        if (IsMinusSign)
            StartCoroutine(VoiceNumberWithDelay(CountableNumber.Item2 - 1));
    }
    IEnumerator VoiceNumberWithDelay(int i)
    {
        yield return new WaitForSeconds(1f);
        GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayVoice(i);
        StartCoroutine(VoiceOtherWithDelay(0, false));
    }

    IEnumerator VoiceFirstNumberWithDelay(int i)
    {
        yield return new WaitForSeconds(1.5f);
        GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayVoice(i);
        StartCoroutine(VoiceOtherWithDelay(2, true));
    }



    protected override void SetCountablesNumber()
    {
        CountablesNumber = CountableNumber.Item1;
    }

    protected override void SetCountableId(int i)
    {
        Countables[i].GetComponent<SubCountableBehaviour>().SetId(i);
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

    protected override void SetLastGame()
    {
        MainGameData.LastFinishedGame = 2;
    }
}
