using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData = SubGameData;

public class SubGameManager : MonoBehaviour
{
    public GameObject CountablePrefab;
    public GameObject Character;
    public GameObject ButtonController;
    private GameObject[] Countables;
    private Tuple<int, int> CountableNumber;
    public GameObject Tap;
    public GameObject TutorialCountable1;
    public GameObject TutorialCountable2;
    public GameObject TutorialCountable3;
    private GameObject[] TutorialComponents;
    private int GamesWon;
    private int SelectedCountables;

    // Start is called before the first frame update
    void Start()
    {
        GameData.PressedButton = true;
        GamesWon = 0;

        TutorialComponents = new GameObject[] { Tap, TutorialCountable1, TutorialCountable2, TutorialCountable3 };
    }

    // Update is called once per frame
    void Update()
    {
    }
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
        GameData.PressedButton = true;
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
    IEnumerator PrepareWithDelay(int i)
    {
        yield return new WaitForSeconds(2.0f * i);
        PrepareForNextRound();
    }

    public void PrepareForNextRound()
    {
        CountableNumber = GameData.NextRoundSettings;
        if (Tuple.Equals(CountableNumber, Tuple.Create(-1, -1)))
            GameOver();
        else
        {
            GameData.Round += 1;
            GamesWon = GameData.Success;
            SelectedCountables = CountableNumber.Item1;
            DestroyCountablesAfterRound();
            StartCoroutine(MakeCountablesWithDelay());
            ButtonController.GetComponent<SubCanvasBehaviour>().PrepareButtons(SelectedCountables);
            GameData.PressedButton = true;
        }
    }

    public void DeleteTutorial()
    {
        foreach (GameObject obj in TutorialComponents)
            Destroy(obj);
    }

    IEnumerator MakeCountablesWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        MakeCountablesForRound();
        GameData.PressedButton = false;

    }

    private void MakeCountablesForRound()
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
    private void DestroyCountablesAfterRound()
    {

        if (Countables != null)
        {
            for (int i = 0; i < Countables.Length; i++)
            {
                Countables[i].GetComponent<Animator>().SetTrigger("End");
                Countables[i].GetComponents<AudioSource>()[1].Play();
                Destroy(Countables[i], 2);
            }
        }
        
    }

    private void GameOver()
    {
        Character.GetComponent<CharacterBehaviour>().Winner();
        ButtonController.GetComponent<SubCanvasBehaviour>().ActivateEndScreen();
        DestroyCountablesAfterRound();
        GameData.GameOver = true;
    }
    private void ShowCorrectAnswer()
    {
        StartCoroutine(TellTheAnswerWithDelay());
        for (int i = 0; i < CountableNumber.Item1; i++)
        {
            Countables[i].GetComponent<SubCountableBehaviour>().Unselect();
        }
        for (int i=0; i < CountableNumber.Item1 - CountableNumber.Item2; i++)
        {
            StartCoroutine(ShowWithDelay(i));
        }

    }
    IEnumerator TellTheAnswerWithDelay()
    {
        var number = CountableNumber.Item1 - CountableNumber.Item2;
        yield return new WaitForSeconds(1.0f);
        GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayVoice(number - 1);
        ButtonController.GetComponent<SubCanvasBehaviour>().ChangeNumber(number);
    }
    IEnumerator ShowWithDelay(int i)
    {
        yield return new WaitForSeconds(1.0f * (i + 3));
        Countables[i].GetComponent<SubCountableBehaviour>().Select();
    }

}

