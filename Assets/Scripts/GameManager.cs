using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject CountablePrefab;
    public GameObject Character;
    public GameObject ButtonController;
    private GameObject[] Countables;
    private int CountableNumber;
    // Start is called before the first frame update
    void Start()
    {
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
            if (GameData.Round > -1)
                GameObject.Find("Kitty").GetComponent<CharacterBehaviour>().GoodAnswer();
            GameData.Round += 1;
            DestroyCountablesAfterRound();
            StartCoroutine(MakeCountablesWithDelay());
            ButtonController.GetComponent<ButtonsController>().PrepareButtons();
        }
        
    }
    private void GameOver()
    {
        //bravo!
        GameObject.Find("Kitty").GetComponent<CharacterBehaviour>().Winner();
        ButtonController.GetComponent<ButtonsController>().ActivateEndScreen();
        ButtonController.GetComponent<ButtonsController>().DestroyOldButtons();
        DestroyCountablesAfterRound();

    }


    private void MakeCountablesForRound()
    {
        Countables = new GameObject[CountableNumber];
        for (int i =0; i< CountableNumber; i++)
        {
            Vector3 pos = new Vector3(0f,1f,90f);
            var height = Screen.currentResolution.height / 4;
            var width = Screen.currentResolution.width / (CountableNumber+1);
            Countables[i] = Instantiate(CountablePrefab, pos, Quaternion.identity);
            Countables[i].transform.SetParent(GameObject.Find("Background").transform);
            Countables[i].transform.localPosition = new Vector3((-Screen.currentResolution.width / 2) + ((i + 1) * width), -height, 0);

        }

    }
    IEnumerator MakeCountablesWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        MakeCountablesForRound();
    }
    private void DestroyCountablesAfterRound()
    {

        if (Countables != null)
        {
            for (int i=0; i< Countables.Length; i++)
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
}
