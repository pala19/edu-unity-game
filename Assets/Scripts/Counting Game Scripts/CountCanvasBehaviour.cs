using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountCanvasBehaviour : CanvasBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        EndScreen.SetActive(false);
        DeactivateFireworks();
        AddAudioSourcesToArray();
        PlayedNumber = 0;
        ActiveButtons = new GameObject[3];        
    }

    // Update is called once per frame
    void Update() {}

    protected override void InstantiateButtons()
    {
        int number = 0;
        if (PlayedNumber > 0 && PlayedNumber < 4)
            number = 1;
        if (PlayedNumber > 3 && PlayedNumber < 7)
            number = 4;
        if (PlayedNumber > 6 && PlayedNumber < 10)
            number = 7;
        for (int i=0; i< 3; i++)
        {
            string name = "Button" + number + "Prefab";
            var button = this.GetType().GetField(name).GetValue(this) as GameObject;
            ActiveButtons[i] = Instantiate(button, gameObject.transform.position, Quaternion.identity);
            ActiveButtons[i].transform.SetParent(gameObject.transform);
            number++;
        }
    }

    public override void ShowCorrectAnswer()
    {
        PlayFailureMusic();
        int number= 0;
        if (PlayedNumber == 2 || PlayedNumber == 5 || PlayedNumber == 8)
            number = 1;
        else if (PlayedNumber == 3 || PlayedNumber == 6 || PlayedNumber == 9)
            number = 2;
        ActiveButtons[number].GetComponent<Animator>().SetTrigger("Pressed");
    }    

    public override void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    public override void Exit()
    {
        SceneManager.LoadScene(2);
    }
    protected override void AssignPlayedNumber() 
    {
        PlayedNumber = CountGameData.CurrentRoundSettings;
    }
    protected override void ChangePressedButton()
    {
        AddGameData.PressedButton = !AddGameData.PressedButton;
    }
}
