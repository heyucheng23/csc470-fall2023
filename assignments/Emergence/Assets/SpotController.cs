using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpotController : MonoBehaviour
{
    public int state;
    public int count;
    public Color alive;
    public Color dead;
    Renderer rend;
    //public Sprite[] sprites;
    // Start is called before the first frame update
    private bool isClicked = false;
    void Start()
    {
        rend = gameObject.GetComponentInChildren<Renderer>();
        dead = rend.material.color;
        UpdateColor();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateColor();
        // this.GetComponent<Image>().sprite = sprites[state];
    }


    public void ChangeState()
    {
        if (state == 0)
        {
            state = 1;
        }
        else
        {
            state = 0;
        }
    }


    public void UpdateColor()
    {
        if (state == 1)
        {
            rend.material.color = alive;
        }
        else
        {
            rend.material.color = dead;
        }
    }
    private void OnMouseDown()
    {
        isClicked = !isClicked;

        ChangeState();
        UpdateColor();
    }
}
    