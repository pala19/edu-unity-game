using System;
using System.Collections;
using UnityEngine;

public class AddGameManager : GameManager
{
    private Tuple<int, int> CountableNumber;
    public GameObject TutorialCountable1;
    public GameObject TutorialCountable2;
    public GameObject SkipTutorialBtn;
    private int GamesWon;
    private int SelectedCountables;

    // Start is called before the first frame update
    void Start()
    {
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
        int result = CountableNumber.Item1 + CountableNumber.Item2;
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

    protected override void MakeCountablesForRound()
    {
        Countables = new GameObject[CountableNumber.Item1 + CountableNumber.Item2];
        for (int i = 0; i < CountableNumber.Item1 + CountableNumber.Item2; i++)
        {
            Vector3 pos = new Vector3(0f, 1f, 90f);
            float height = Screen.currentResolution.height / 4;
            float width;
            if (CountableNumber.Item1 + CountableNumber.Item2 > 5)
                width = Screen.currentResolution.width / 6;
            else
                width = Screen.currentResolution.width / (CountableNumber.Item1 + CountableNumber.Item2 + 1);
            if (i > 4)
            {
                width = Screen.currentResolution.width / (CountableNumber.Item1 + CountableNumber.Item2 - 4);
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
            Countables[i].GetComponent<CountableBehaviour>().SetId(i);

        }

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
        var number = CountableNumber.Item1 - CountableNumber.Item2;
        SelectedCountables = 0;
        yield return new WaitForSeconds(1.0f);
        GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayVoice(number - 1);
        ButtonController.GetComponent<AddCanvasBehaviour>().ChangeNumber(number);
    }
    IEnumerator ShowWithDelay(int i)
    {
        yield return new WaitForSeconds(1.0f * (i + 3));
        Countables[i].GetComponent<CountableBehaviour>().Select();
    }

    protected override void ChangeGameOverData() 
    {
        Debug.Log("ChangeGameOverData");
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
}
