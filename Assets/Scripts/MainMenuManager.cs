using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FirstGameOnClick()
    {
        SceneManager.LoadScene(1);

    }
    public void SecondGameOnClick()
    {
        SceneManager.LoadScene(2);

    }
    public void ThirdGameOnClick()
    {
        SceneManager.LoadScene(3);

    }
}
