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
                inp.z = GameObject.Find("Background").transform.position.z;
                var pos = Camera.main.ScreenToWorldPoint(inp);
                transform.localPosition = GameObject.Find("Background").transform.InverseTransformPoint(pos);
            }
            else
            {
                GameObject.Find("Basket").GetComponent<BasketBehaviour>().AddApple();
                Destroy(gameObject, 0.1f);
            }
        }
          
    }

}
