using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public class AddBasketCanvasBehaviour : CanvasBehaviour
{
    public GameObject First;
    public GameObject Second;
    public GameObject PlusSign;
    public GameObject EqualSign;
    public GameObject Result;
    GameObject[] Buttons;
    // Start is called before the first frame update
    void Start()
    {
        EndScreen.SetActive(false);
        DeactivateFireworks();
        DeactivateFireworks();
        AddAudioSourcesToArray();
        Buttons = new GameObject[] { First, Second, PlusSign, EqualSign, Result };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void DestroyOldButtons() { }

    protected override void InstantiateButtons()
    {
        ActivateButtons();
        Result.SetActive(false);
        var current = AddBasketData.CurrentRoundSettings;
        string name1 = "Button" + current.Item1 + "Prefab";
        string name2 = "Button" + current.Item2 + "Prefab";
        var button1 = this.GetType().GetField(name1).GetValue(this) as GameObject;
        var button2 = this.GetType().GetField(name2).GetValue(this) as GameObject;
        First.GetComponent<SVGImage>().sprite = button1.GetComponent<SVGImage>().sprite;
        Second.GetComponent<SVGImage>().sprite = button2.GetComponent<SVGImage>().sprite;
    }

    private void ActivateButtons()
    {
        for (int i = 0; i < Buttons.Length - 1; i++)
            Buttons[i].SetActive(true);
    }

    public void ChangeNumber(int number)
    {
        if (number != 0)
        {
            Result.SetActive(true);
            string name1 = "Button" + number + "Prefab";
            var button1 = this.GetType().GetField(name1).GetValue(this) as GameObject;
            Result.GetComponent<SVGImage>().sprite = button1.GetComponent<SVGImage>().sprite;
        }
        else
            Result.SetActive(false);

    }
    public void ResultPressed()
    {
        GameObject.Find("Game").GetComponent<AddBasketManager>().CheckResultAndPrepareRound();
    }

    public override void GoodAnswer()
    {
        PlaySuccessMusic();
        ActivateFireworks();
        Result.GetComponent<Animator>().SetBool("Pressed", true);
    }
}
