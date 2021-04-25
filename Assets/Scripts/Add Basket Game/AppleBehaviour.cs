using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

          if (Application.platform  !=  RuntimePlatform.Android)
        {
            if (Input.GetMouseButton(0))
            {
                var inp = Input.mousePosition;
                var pos = Camera.main.ScreenToWorldPoint(inp);
                print(pos.x + " " + pos.y);
                transform.localPosition = GameObject.Find("Background").transform.InverseTransformPoint(inp);
            }
            else
            {
                GameObject.Find("Basket").GetComponent<BasketBehaviour>().AddApple();
                Destroy(gameObject, 0.5f);
            }
        }
          
    }

}
