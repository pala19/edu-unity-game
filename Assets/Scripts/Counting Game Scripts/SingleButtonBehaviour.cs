using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData = CountGameData;
public class SingleButtonBehaviour : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonPress()
    {
        if (!GameData.PressedButton)
        {
            GameData.PressedButton = true;
            string ButtonName = name;
            if (Char.GetNumericValue(ButtonName[6]) == GameData.CurrentRoundSettings)
            {
                animator.SetTrigger("Pressed");
                GameObject.Find("Game").GetComponent<GameManager>().PrepareRound(false);
            }
            else
            {
                GameObject.Find("Game").GetComponent<GameManager>().PrepareRound(true);
            }

        }
    }
    public void SetEndTrigger()
    {
        animator.SetTrigger("End");
    }


}
