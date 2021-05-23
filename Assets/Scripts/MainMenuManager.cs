using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject Scroller1, Scroller2, Scroller3, Scroller4;
    public GameObject AddGameBtn, SubGameBtn, BasketGameBtn;
    public GameObject CBtn1, CBtn2, CBtn3, CBtn4, CBtn5, CBtn6;
    public GameObject ABtn1, ABtn2, ABtn3, ABtn4, ABtn5, ABtn6;
    public GameObject SBtn1, SBtn2, SBtn3, SBtn4, SBtn5, SBtn6;
    public GameObject BBtn1, BBtn2, BBtn3, BBtn4, BBtn5, BBtn6;
    public Material Disabled;
    private GameObject[] CountGameBtns;
    private GameObject[] AddGameBtns;
    private GameObject[] SubGameBtns;
    private GameObject[] BasketGameBtns;
    // Start is called before the first frame update
    void Start()
    {
        if (!AddGameData.IsActive(0))
        {
            AddGameBtn.transform.GetChild(1).gameObject.SetActive(false);
            AddGameBtn.GetComponent<SVGImage>().material = Disabled;
        }
        if (!SubGameData.IsActive(0))
        {
            SubGameBtn.transform.GetChild(1).gameObject.SetActive(false);
            SubGameBtn.GetComponent<SVGImage>().material = Disabled;
        }
        if (!AddBasketData.IsActive(0))
        {
            BasketGameBtn.transform.GetChild(1).gameObject.SetActive(false);
            BasketGameBtn.GetComponent<SVGImage>().material = Disabled;
        }

        CountGameBtns = new GameObject[] { CBtn1, CBtn2, CBtn3, CBtn4, CBtn5, CBtn6 };
        AddGameBtns = new GameObject[] { ABtn1, ABtn2, ABtn3, ABtn4, ABtn5, ABtn6 };
        SubGameBtns = new GameObject[] { SBtn1, SBtn2, SBtn3, SBtn4, SBtn5, SBtn6 };
        BasketGameBtns = new GameObject[] { BBtn1, BBtn2, BBtn3, BBtn4, BBtn5, BBtn6 };

        for (int i=0; i<CountGameBtns.Length; i++)
        {
            if (CountGameData.IsActive(i))
                CountGameBtns[i].transform.GetChild(0).gameObject.SetActive(false);
            else
                CountGameBtns[i].GetComponent<SVGImage>().material = Disabled;
        }
        for (int i = 0; i <AddGameBtns.Length; i++)
        {
            if (AddGameData.IsActive(i))
                AddGameBtns[i].transform.GetChild(0).gameObject.SetActive(false);
            else
                AddGameBtns[i].GetComponent<SVGImage>().material = Disabled;
        }
        for (int i = 0; i < SubGameBtns.Length; i++)
        {
            if (SubGameData.IsActive(i))
                SubGameBtns[i].transform.GetChild(0).gameObject.SetActive(false);
            else
                SubGameBtns[i].GetComponent<SVGImage>().material = Disabled;
        }
       for (int i = 0; i < BasketGameBtns.Length; i++)
        {
            if (AddBasketData.IsActive(i))
                BasketGameBtns[i].transform.GetChild(0).gameObject.SetActive(false);
            else
                BasketGameBtns[i].GetComponent<SVGImage>().material = Disabled;
        }
        MainGameData.PressedButton = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FirstGameOnClick()
    {
        Scroller1.SetActive(!Scroller1.activeSelf);

    }
    public void SecondGameOnClick()
    {
        if (EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.activeSelf)
            Scroller2.SetActive(!Scroller2.activeSelf);
    }
    public void ThirdGameOnClick()
    {
        if (EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.activeSelf)
            Scroller3.SetActive(!Scroller3.activeSelf);
    }
    public void FourthGameOnClick()
    {
        if (EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.activeSelf)
            Scroller4.SetActive(!Scroller3.activeSelf);
    }
    public void PlayCountGameOnClick(int Round)
    {
        if (!EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            CountGameData.SetCurrentGame = Round;
            SceneManager.LoadScene(1);
        }
    }
    public void PlayAddGameOnClick(int Round)
    {
        if (!EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            AddGameData.SetCurrentGame = Round;
            SceneManager.LoadScene(2);
        }
    }
    public void PlaySubGameOnClick(int Round)
    {
        if (!EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            SubGameData.SetCurrentGame = Round;
            SceneManager.LoadScene(3);
        }
    }

    public void PlayBasketGameOnClick(int Round)
    {
        if (!EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            AddBasketData.SetCurrentGame = Round;
            SceneManager.LoadScene(4);
        }
    }
    public void Exit()
    {
        EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
    }
}
