using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SingleButtonBehaviour : MonoBehaviour
{
    Animator animator;
    AudioSource dissapearSound;

    // Start is called before the first frame update
    void Start()
    {
        dissapearSound = GetComponents<AudioSource>()[1];
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {}

    public void ButtonPress()
    {
        if (!CountGameData.PressedButton)
        {
            CountGameData.PressedButton = true;
            string ButtonName = name;
            if (Char.GetNumericValue(ButtonName[6]) == CountGameData.CurrentRoundSettings)
            {
                animator.SetTrigger("Pressed");
                GameObject.Find("Game").GetComponent<CountGameManager>().PrepareRound(false);
            }
            else
            {
                GameObject.Find("Game").GetComponent<CountGameManager>().PrepareRound(true);
            }
        }
    }

    public void SetEndTrigger()
    {
        animator.SetTrigger("End");
    }

    public void PlayDissapearSound()
    {
        dissapearSound.Play();
    }
}
