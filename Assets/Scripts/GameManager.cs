using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject CountablePrefab;
    public GameObject Character;
    public GameObject ButtonController;
    public GameObject Tap;
    protected GameObject[] TutorialComponents;
    protected GameObject[] Countables;
    protected int CountablesNumber;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}


    public void DeleteTutorial()
    {
        foreach (GameObject obj in TutorialComponents)
            Destroy(obj);
    }

    public void PrepareForNextRound()
    {
        AssignCountableNumber();
        if (CheckIfGameOver())
            GameOver();
        else
        {
            AssignGamesWon();
            DestroyCountablesAfterRound();
            StartCoroutine(MakeCountablesWithDelay());
            PrepareButtons();
            VoiceCurrentRound();
        }
    }

    IEnumerator MakeCountablesWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        MakeCountablesForRound();
        MainGameData.PressedButton = !MainGameData.PressedButton;
    }

    protected virtual void VoiceCurrentRound(){ }

    protected void MakeCountablesForRound() 
    {
       SetCountablesNumber();
        Countables = new GameObject[CountablesNumber];
        for (int i = 0; i < CountablesNumber; i++)
        {
            Vector3 pos = new Vector3(0f, 1f, 90f);
            float height = Screen.currentResolution.height / 4;
            float width;
            Countables[i] = Instantiate(CountablePrefab, pos, Quaternion.identity);
            Countables[i].transform.SetParent(GameObject.Find("Background").transform);

            if (CountablesNumber > 4 && CountablesNumber < 7)
            {
                width = Screen.currentResolution.width / 4;

                if (i > 2)
                {
                    width = Screen.currentResolution.width / (CountablesNumber - 2);
                    float pom = i - 3;
                    Countables[i].transform.localPosition = new Vector3((-Screen.currentResolution.width / 2) + ((pom + 1) * width), -height * 1.6f, 0);
                }
                else
                {
                    Countables[i].transform.localPosition = new Vector3((-Screen.currentResolution.width / 2) + ((i + 1) * width), -height / 1.1f, 0);
                }
            }
            else if (CountablesNumber > 6 && CountablesNumber < 9)
            {
                width = Screen.currentResolution.width / 5;
                if (i > 3)
                {
                    width = Screen.currentResolution.width / (CountablesNumber - 3);
                    float pom = i - 4;
                    Countables[i].transform.localPosition = new Vector3((-Screen.currentResolution.width / 2) + ((pom + 1) * width), -height * 1.6f, 0);
                }
                else
                {
                    Countables[i].transform.localPosition = new Vector3((-Screen.currentResolution.width / 2) + ((i + 1) * width), -height / 1.1f, 0);
                }
            }
            else
            {
                if (CountablesNumber > 5)
                    width = Screen.currentResolution.width / 6;
                else
                    width = Screen.currentResolution.width / (CountablesNumber + 1);
                if (i > 4)
                {
                    width = Screen.currentResolution.width / (CountablesNumber - 4);
                    float pom = i - 5;
                    Countables[i].transform.localPosition = new Vector3((-Screen.currentResolution.width / 2) + ((pom + 1) * width), -height * 1.6f, 0);
                }
                else
                {
                    Countables[i].transform.localPosition = new Vector3((-Screen.currentResolution.width / 2) + ((i + 1) * width), -height / 1.1f, 0);
                }
            }
            SetCountableId(i);
        }
    }
    protected virtual void SetCountablesNumber() { }

    protected virtual void SetCountableId(int i) { }

    protected IEnumerator PrepareWithDelay(int i)
    {
        yield return new WaitForSeconds(2.0f * i);
        PrepareForNextRound();
    }

    private void DestroyCountablesAfterRound()
    {
        if (Countables != null)
        {
            for (int i = 0; i < Countables.Length; i++)
            {
                AdditionalActionsBeforeDestroy(i);               
                Destroy(Countables[i], 2.0f);
            }
        }
        AdditionalActionsAfterDestroy();
    }
    private void GameOver()
    {
        Character.GetComponent<CharacterBehaviour>().Winner();
        ActivateEndScreen(); 
        DestroyCountablesAfterRound();
        ChangeGameOverData();

    }

    protected virtual void ShowCorrectAnswer()
    {
        for (int i = 0; i < Countables.Length; i++)
        {
            StartCoroutine(ShowWithDelay(i));
        }
    }

    IEnumerator ShowWithDelay(int i)
    {
        yield return new WaitForSeconds((i + 1) * 1.0f);
        Countables[i].GetComponent<Animator>().SetTrigger("Show");
        GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayVoice(i);
    }
    protected virtual void ChangeGameOverData() {}

    protected virtual bool CheckIfGameOver()
    {
        return false;
    }

    protected virtual void AssignCountableNumber() {}

    protected virtual void AssignGamesWon() {}

    protected virtual void PrepareButtons() {}

    protected virtual void AdditionalActionsBeforeDestroy(int i) { }

    protected virtual void AdditionalActionsAfterDestroy() { }

    protected virtual void ActivateEndScreen() { }


    public void SkipTutorial() 
    {
        GetComponent<UnityEngine.Playables.PlayableDirector>().enabled = false;  
        DeleteTutorial();
        PrepareForNextRound();
    }

}
