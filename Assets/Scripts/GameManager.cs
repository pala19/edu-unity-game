using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
    protected virtual void Start() {
        PlaceCharacter();
    }

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
    protected void PlaceCharacter()
    {
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.currentResolution.width / 10, Screen.currentResolution.height* 3/4, GameObject.Find("Background").transform.position.z));
        pos.z = GameObject.Find("Background").transform.position.z;
        Character.transform.localPosition  = GameObject.Find("Background").transform.InverseTransformPoint(pos);
    }

    IEnumerator MakeCountablesWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        MakeCountablesForRound();
        MainGameData.PressedButton = !MainGameData.PressedButton;
    }

    protected virtual void VoiceCurrentRound(){ }

    protected virtual void MakeCountablesForRound() 
    {
       SetCountablesNumber();
        Countables = new GameObject[CountablesNumber];
        for (int i = 0; i < CountablesNumber; i++)
        {
            float height;
            float width;
            Countables[i] = Instantiate(CountablePrefab, new Vector3(0f, 1f, 0f), Quaternion.identity);
            Countables[i].transform.SetParent(GameObject.Find("Background").transform);
            Vector3 position;

            if (CountablesNumber > 4 && CountablesNumber < 7) //5, 6
            {
                width = Screen.currentResolution.width / 4;

                if (i > 2)
                {
                   position = Camera.main.ScreenToWorldPoint(new Vector3(width*(i+1 - (CountablesNumber - 2.5f)), Screen.currentResolution.height / (2 + 0.5f), GameObject.Find("Background").transform.position.z));
                }
                else
                {
                    position = Camera.main.ScreenToWorldPoint(new Vector3(width * (i + 1), Screen.currentResolution.height / 5, GameObject.Find("Background").transform.position.z));
                }
            }
            else if (CountablesNumber > 6 && CountablesNumber < 10) //7, 8, 9
            {
                float[] pom = { 4.5f, 5f, 5.5f };
                width = Screen.currentResolution.width / 5;
                if (i > 3)
                {
                    position = Camera.main.ScreenToWorldPoint(new Vector3(width * (i - (CountablesNumber - pom[CountablesNumber%7])), Screen.currentResolution.height / (2 + 0.5f), GameObject.Find("Background").transform.position.z));

                }
                else
                {
                    position = Camera.main.ScreenToWorldPoint(new Vector3(width*(i + 1), Screen.currentResolution.height / 5, GameObject.Find("Background").transform.position.z));
                }
            }
            else //1, 2 , 3, 4
            {
                width = Screen.currentResolution.width / (CountablesNumber + 1);
                position = Camera.main.ScreenToWorldPoint(new Vector3(width * (i + 1), Screen.currentResolution.height / 5, GameObject.Find("Background").transform.position.z));
            }

            position.z = GameObject.Find("Background").transform.position.z;
            Countables[i].transform.localPosition = GameObject.Find("Background").transform.InverseTransformPoint(position);

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
        SetLastGame();
        SaveGame();
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

    protected virtual void SetLastGame() {}

    private void SaveGame()
    {
        Save save = CreateSaveGameObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    private Save CreateSaveGameObject()
    {
        Save save = new Save();

        var CountGameSuccessRate = CountGameData.GetSuccessRate;
        var AddGameSuccessRate = AddGameData.GetSuccessRate;
        var SubGameSuccessRate = SubGameData.GetSuccessRate;
        var BasketGameSuccessRate = AddBasketData.GetSuccessRate;

        foreach (var elem in CountGameSuccessRate)
            save.CountingGameClearedLevels.Add(elem);
        foreach (var elem in AddGameSuccessRate)
            save.AddGameClearedLevels.Add(elem);
        foreach (var elem in SubGameSuccessRate)
            save.SubGameClearedLevels.Add(elem);
        foreach (var elem in BasketGameSuccessRate)
            save.BasketGameClearedLevels.Add(elem);

        return save;
    }
}
