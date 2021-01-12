using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData = AddGameData;
public class AddGameManager : MonoBehaviour
{
    public GameObject CountablePrefab;
    public GameObject Character;
    public GameObject ButtonController;
    private GameObject[] Countables;
    private Tuple<int, int> CountableNumber;
    private int GamesWon;
    private int SelectedCountables;

    // Start is called before the first frame update
    void Start()
    {
        GamesWon = 0;
        SelectedCountables = 0;
        PrepareForNextRound();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeSelected(int id)
    {
        if (Countables[id].GetComponent<CountableBehaviour>().IsSelected())
            SelectedCountables++;
        else
            SelectedCountables--;
        ButtonController.GetComponent<CanvasBehaviour>().ChangeNumber(SelectedCountables);
    }
    public void CheckResultAndPrepareRound()
    {
        int result = CountableNumber.Item1 + CountableNumber.Item2;
        if (result == SelectedCountables)
        {
            ButtonController.GetComponent<CanvasBehaviour>().GoodAnswer();
            Character.GetComponent<CharacterBehaviour>().GoodAnswer();
        }
        else
            ButtonController.GetComponent<CanvasBehaviour>().ShowCorrectAnswer();
        SelectedCountables = 0;
        StartCoroutine(PrepareWithDelay());
    }
    IEnumerator PrepareWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
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
            DestroyCountablesAfterRound();
            StartCoroutine(MakeCountablesWithDelay());
            ButtonController.GetComponent<CanvasBehaviour>().PrepareButtons();
        }


    }
    IEnumerator MakeCountablesWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        MakeCountablesForRound();
        GameData.PressedButton = false;

    }

    private void MakeCountablesForRound()
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
    private void DestroyCountablesAfterRound()
    {

        if (Countables != null)
        {
            for (int i = 0; i < Countables.Length; i++)
            {
                Countables[i].GetComponent<Animator>().SetTrigger("End");
                StartCoroutine(DestroyWithDelay(i));
            }
        }
    }
    IEnumerator DestroyWithDelay(int i)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(Countables[i]);
    }

    private void GameOver()
    {
        ButtonController.GetComponent<CanvasBehaviour>().ActivateEndScreen();
        Character.GetComponent<CharacterBehaviour>().Winner();
    }
}
