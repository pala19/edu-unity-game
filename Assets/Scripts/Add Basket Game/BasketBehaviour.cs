using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public class BasketBehaviour : MonoBehaviour
{
    public GameObject[] BasketPrefabs;
    private int appleContained;
    private bool AppleIn;
    // Start is called before the first frame update
    void Start()
    {
        appleContained = 1;
        ChangeImage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setAppleContained(int number)
    {
        appleContained = number;
    }
    public int getAppleContained()
    {
        return appleContained;
    }
    public void AddApple()
    {
        print("used");
        if (AppleIn && appleContained < 9)
        {
            appleContained++;
            ChangeImage();
        }
        AppleIn = false;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name.Contains("Apple"));
        if (collision.gameObject.name.Contains("Apple"))
            AppleIn = true;
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Apple"))
            AppleIn = false;
    }
    private void ChangeImage()
    {
        GetComponent<SVGImage>().sprite = BasketPrefabs[appleContained - 1].GetComponent<SVGImage>().sprite;
    }
}
