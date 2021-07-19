using System;
using System.Collections;
using UnityEngine;

public class AddGameManager : GameManager
{
    private Tuple<int, int> CountableNumber;
    public GameObject TutorialCountable1;
    public GameObject TutorialCountable2;
    public GameObject SkipTutorialBtn;
    private int SelectedCountables;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        GamesWon = 0;
        SelectedCountables = 0;
        TutorialComponents = new GameObject[] { Tap, TutorialCountable1, TutorialCountable2, SkipTutorialBtn};
    }

    // Update is called once per frame
    void Update() {}

    public void ChangeSelected(int id)
    {
        if (Countables[id].GetComponent<CountableBehaviour>().IsSelected())
            SelectedCountables++;
        else
            SelectedCountables--;
        ButtonController.GetComponent<AddCanvasBehaviour>().ChangeNumber(SelectedCountables);
        if (SelectedCountables > 0)
            GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayVoice(SelectedCountables - 1);
    }
    public void CheckResultAndPrepareRound()
    {
        MainGameData.PressedButton = true;
        int result = Utils.DoMyMath(CountableNumber.Item1,CountableNumber.Item2, true);
        var delay = 1;
        if (result == SelectedCountables)
        {
            ButtonController.GetComponent<CanvasBehaviour>().GoodAnswer();
            Character.GetComponent<CharacterBehaviour>().GoodAnswer();
            AddGameData.Success += 1;
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

    protected override void VoiceCurrentRound()
    {
        StartCoroutine(VoiceFirstNumberWithDelay(CountableNumber.Item1 - 1));
    }

    IEnumerator VoiceOtherWithDelay(int i, bool IsPlusSign)
    {
        yield return new WaitForSeconds(1f);
        GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayOtherVoice(i);
        if (IsPlusSign)
            StartCoroutine(VoiceNumberWithDelay(CountableNumber.Item2-1));
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
        StartCoroutine(VoiceOtherWithDelay(1, true));
    }

    protected override void SetCountablesNumber()
    {
        CountablesNumber = CountableNumber.Item1 + CountableNumber.Item2;
    }

    protected override void SetCountableId(int i)
    {
        Countables[i].GetComponent<CountableBehaviour>().SetId(i);
    }


    protected override void ShowCorrectAnswer()
    {
        StartCoroutine(TellTheAnswerWithDelay());
        for (int i=0; i<Countables.Length; i++)
        {
            Countables[i].GetComponent<CountableBehaviour>().Unselect();
            StartCoroutine(ShowWithDelay(i));
        }

    }
    IEnumerator TellTheAnswerWithDelay()
    {
        var number = CountableNumber.Item1 + CountableNumber.Item2;
        SelectedCountables = 0;
        yield return new WaitForSeconds(1f);
        GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayVoice(number - 1);
        ButtonController.GetComponent<AddCanvasBehaviour>().ChangeNumber(number);
    }
    IEnumerator ShowWithDelay(int i)
    {
        yield return new WaitForSeconds(1.0f * (i + 2));
        Countables[i].GetComponent<CountableBehaviour>().Select();
    }

    protected override void ChangeGameOverData() 
    {
        AddGameData.GameOver = true;
    }

    protected override void AssignCountableNumber()
    {
        CountableNumber = AddGameData.NextRoundSettings;
    }

    protected override bool CheckIfGameOver()
    {
        return Tuple.Equals(CountableNumber, Tuple.Create(-1, -1));
    }

    protected override void AssignGamesWon() 
    {
        AddGameData.Round += 1;
        GamesWon = AddGameData.Success;
    }
    
    protected override void PrepareButtons()
    {
        ButtonController.GetComponent<CanvasBehaviour>().PrepareButtons();
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
        ButtonController.GetComponent<AddCanvasBehaviour>().ActivateEndScreen();
    }

    protected override void SetLastGame()
    {
        MainGameData.LastFinishedGame = 1;
    }
}
