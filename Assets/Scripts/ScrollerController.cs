using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollerController : MonoBehaviour
{
    public RectTransform Background;
    public Button[] btns;
    public RectTransform Center;

    private int FocusLevel;
    private int btnDistance;
    private int minButtonNum;
    private float[] distance;

     void Start()
    {
        distance = new float[btns.Length];
        if (gameObject.name != "MenuScroller")
        {
            for (int i = 0; i < btns.Length; i++)
            {
                AssessIfUsed(gameObject.name, i);
            }
        }
        else
            FocusLevel = MainGameData.LastFinishedGame;
        btnDistance = 400;
        LerpToBtn(FocusLevel);
    }
     void Update()
    {        

    }
    void LerpToBtn(int num)
    {
        float newX = - num * btnDistance;
        Vector2 newPosition = new Vector2(newX, Background.anchoredPosition.y);
        Background.anchoredPosition = newPosition;
    }
    public void Drag()
    {
        for (int i = 0; i < btns.Length; i++)
        {
            distance[i] = Mathf.Abs(Center.transform.position.x - btns[i].transform.position.x);
        }

        float minDistance = Mathf.Min(distance);

        for (int a = 0; a < btns.Length; a++)
        {
            if (minDistance == distance[a])
            {
                minButtonNum = a;
                break;
            }
        }
    }

    public void EndDrag()
    {
        print("enddrag " + minButtonNum);
        LerpToBtn(minButtonNum);
    }

    private void AssessIfUsed(string name, int i)
    {
        if (name == "CountingScroller")
        {
            if (CountGameData.IsActive(i))
                FocusLevel = i;
        }
        else if (name == "AddScroller")
        {
            if (AddGameData.IsActive(i))
                FocusLevel = i;
        }
        else if (name == "SubScroller")
        {
            if (SubGameData.IsActive(i))
                FocusLevel = i;
        }
        else if (name == "BasketScroller")
        {
            if (AddBasketData.IsActive(i))
                FocusLevel = i;
        }
        else
        {
            Debug.Log("wrong scroller used");
        }
    }


}
