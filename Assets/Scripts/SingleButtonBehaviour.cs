using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                GameObject.Find("Canvas").GetComponent<ButtonsController>().PlaySuccessMusic();
                StartCoroutine(PrepareWithDelay());
            }
            else
            {
                GameObject.Find("Canvas").GetComponent<ButtonsController>().PlayFailureMusic();
                GameData.PressedButton = false;
            }

        }
    }
        IEnumerator PrepareWithDelay()
        {
            float delay = 3.0f;
            yield return new WaitForSeconds(delay);
            GameObject.Find("Game").GetComponent<GameManager>().PrepareForNextRound();

        }
    public void SetEndTrigger()
    {
        animator.SetTrigger("End");
    }


}
