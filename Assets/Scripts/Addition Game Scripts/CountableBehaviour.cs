using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountableBehaviour : MonoBehaviour
{
    private bool selected = false;
    private int id;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClick()
    {
        selected = !selected;
        GetComponent<Animator>().SetBool("Selected", selected);
        GameObject.Find("Game").GetComponent<AddGameManager>().ChangeSelected(id);
    }
    public bool IsSelected()
    {
        return selected;
    }
    public void SetId(int id)
    {
        this.id = id;
    }
}
