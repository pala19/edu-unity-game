using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData = CountGameData;

public class GameManager : MonoBehaviour
{
    public GameObject CountablePrefab;
    public GameObject Character;
    public GameObject ButtonController;
    private GameObject[] Countables;
    private int CountableNumber;
    private int GamesWon;
    // Start is called before the first frame update
    void Start()
    {
        GamesWon = 0;
        PrepareForNextRound();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void PrepareForNextRound()
    {
        CountableNumber = GameData.NextRoundSettings;
        if (CountableNumber == -1)
            GameOver();
        else
        {
            GameData.Round += 1;
            GamesWon = GameData.Success;
            DestroyCountablesAfterRound();
            StartCoroutine(MakeCountablesWithDelay());
            ButtonController.GetComponent<ButtonsController>().PrepareButtons();
        }
        
    }
    public void PrepareRound(bool failure)
    {
        if (failure)
        {
            ButtonController.GetComponent<ButtonsController>().ShowCorrectAnswer();
            ShowCorrectAnswer();
            StartCoroutine(PrepareWithDelay(CountableNumber));
            Handheld.Vibrate();
        }
        else
        {
            GameData.Success += 1;
            Character.GetComponent<CharacterBehaviour>().GoodAnswer();
            ButtonController.GetComponent<ButtonsController>().GoodAnswer();
            StartCoroutine(PrepareWithDelay(1));

        }
    }
    IEnumerator PrepareWithDelay(int i)
    {
        yield return new WaitForSeconds(2.0f * i);
        PrepareForNextRound();
    }
    private void GameOver()
    {
        Character.GetComponent<CharacterBehaviour>().Winner();
        ButtonController.GetComponent<ButtonsController>().ActivateEndScreen();
        ButtonController.GetComponent<ButtonsController>().DestroyOldButtons();
        DestroyCountablesAfterRound();
        GameData.GameOver = true;

    }


    private void MakeCountablesForRound()
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
    IEnumerator MakeCountablesWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        MakeCountablesForRound();
        GameData.PressedButton = false;

    }
    private void DestroyCountablesAfterRound()
    {

        if (Countables != null)
        {
            for (int i=0; i< Countables.Length; i++)
            {
                Countables[i].GetComponent<Animator>().SetTrigger("End");
                Destroy(Countables[i], 2.0f);
            }
        }
    }


    public void ShowCorrectAnswer()
    {
        for (int i=0; i<Countables.Length; i++)
        {
            StartCoroutine(ShowWithDelay(i));
        }
    }
    IEnumerator ShowWithDelay(int i)
    {
        yield return new WaitForSeconds((i+1) * 1.0f);
        Countables[i].GetComponent<Animator>().SetTrigger("Show");
        GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayVoice(i);
    }
}
