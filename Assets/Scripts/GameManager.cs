using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject CountablePrefab;
    public GameObject Character;
    public GameObject ButtonController;
    public GameObject Tap;
    protected GameObject[] TutorialComponents;
    protected GameObject[] Countables;

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
        }
    }
    IEnumerator MakeCountablesWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        MakeCountablesForRound();
        ChangePressedButton();
    }
    protected virtual void MakeCountablesForRound() {}

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
    protected virtual void ChangePressedButton() {}

    protected virtual void PrepareButtons() {}

    protected virtual void AdditionalActionsBeforeDestroy(int i) { }

    protected virtual void AdditionalActionsAfterDestroy() { }

    protected virtual void ActivateEndScreen() { }
}
