using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileBehaviour : MonoBehaviour
{
    public GameObject ApplePrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeApple()
    {

        var Apple = Instantiate(ApplePrefab, transform.position, Quaternion.identity);
        Apple.transform.SetParent(GameObject.Find("Background").transform, false);

    }
}
