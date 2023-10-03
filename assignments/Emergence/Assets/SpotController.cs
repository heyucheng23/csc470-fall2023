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
    public GameObject spotPrefab;
    Renderer rend;
    //public Sprite[] sprites;
    // Start is called before the first frame update
    private bool isClicked = false;
    private float initialY;
    private float moveUpAmount = 0.5f;
    void Start()
    {
        rend = gameObject.GetComponentInChildren<Renderer>();
        dead = rend.material.color;
        initialY = transform.position.y;
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
        UpdateColor();
    }


    public void UpdateColor()
    {
        if (state == 1)
        {
            rend.material.color = alive;
            transform.position = new Vector3(transform.position.x, initialY + 0.5f, transform.position.z);
        }
        else
        {
            rend.material.color = dead;
            transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
        }
    }
    private void OnMouseDown()
    {
        isClicked = !isClicked;

        ChangeState();
        UpdateColor();
    }
}
    