using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCountableBehaviour : MonoBehaviour
{
    private bool selected = true;
    private int id;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(1, 1, 1);
        GetComponent<Animator>().SetBool("Selected", selected);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClick()
    {
        if (!MainGameData.PressedButton)
        {
            Select();
        }
    }
    public bool IsSelected()
    {
        return selected;
    }
    public void SetId(int id)
    {
        this.id = id;
    }
    public void SelectWithoutTelling()
    {
        selected = true;
        GetComponent<Animator>().SetBool("Selected", selected);
         }
    public void Select()
    {
        selected = !selected;
        GetComponent<Animator>().SetBool("Selected", selected);
        GameObject.Find("Game").GetComponent<SubGameManager>().ChangeSelected(id);
    }
}
