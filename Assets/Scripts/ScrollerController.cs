using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollerController : MonoBehaviour
{
    public RectTransform Background;
    public Button[] btns;
    public RectTransform Center;

    private float[] distance;
    private bool dragging;
    private int btnDistance;
    private int minButtonNum;

     void Start()
    {
        int btnLength = btns.Length;
        distance = new float[btnLength];

        btnDistance = (int)Mathf.Abs(btns[1].GetComponent<RectTransform>().anchoredPosition.x - btns[0].GetComponent<RectTransform>().anchoredPosition.x);
    }
     void Update()
    {
        for (int i=0; i < btns.Length; i++)
        {
            distance[i] = Mathf.Abs(Center.transform.position.x - btns[i].transform.position.x);
        }

        float minDistance = Mathf.Min(distance);

        for (int a = 0; a < btns.Length; a++)
        {
            if (minDistance == distance[a])
                minButtonNum = a;
        }

        if (!dragging)
        {
            LerpToBtn(minButtonNum * -btnDistance);
        }
    }
    void LerpToBtn(int position)
    {
        float newX = Mathf.Lerp(Background.anchoredPosition.x, position, Time.deltaTime * 10f);
        Vector2 newPosition = new Vector2(newX, Background.anchoredPosition.y);
        Background.anchoredPosition = newPosition;
    }

    public void StartDrag()
    {
        dragging = true;
    }
    public void EndDrag()
    {
        dragging = false;
    }


}
