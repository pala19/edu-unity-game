using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public class BasketBehaviour : MonoBehaviour
{
    public GameObject[] BasketPrefabs;
    public GameObject ApplePrefab;
    private int appleContained;
    private bool AppleIn;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setAppleContained(int number, bool playSound = true)
    {
        appleContained = number;
        ChangeImage(playSound);
    }
    public int getAppleContained()
    {
        return appleContained;
    }
    public void AddApple()
    {
        if (AppleIn && appleContained < 9)
        {
            appleContained++;
            ChangeImage(true);
            GameObject.Find("Game").GetComponent<AddBasketManager>().ChangeResult(appleContained);
        }
        AppleIn = false;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Apple"))
            AppleIn = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Apple"))
            AppleIn = false;
    }

    public void TakeApple()
    {
        if (appleContained > 0)
        {
            appleContained--;
            var Apple = Instantiate(ApplePrefab, transform.position, Quaternion.identity);
            Apple.transform.SetParent(GameObject.Find("Background").transform, false);
            ChangeImage(true);
            GameObject.Find("Game").GetComponent<AddBasketManager>().ChangeResult(appleContained);
        }
    }

    private void ChangeImage(bool playSound)
    {
        GetComponent<SVGImage>().sprite = BasketPrefabs[appleContained].GetComponent<SVGImage>().sprite;
        if (playSound)
            GameObject.Find("SoundObject").GetComponent<SoundBehaviour>().PlayVoice(appleContained-1);
    }

}
