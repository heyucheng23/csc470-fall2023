using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{   
    public Color hoverColor = Color.gray;
    private Color initColor;
    private Renderer render;
    public GameObject turrentPrefab;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<MeshRenderer>();
        initColor = render.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseEnter()
    {
        if (BuildManager.Instance.SelectedTurrent == null) return;
        render.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        render.material.color = initColor;
    }

    private void OnMouseDown()
    {
        if (BuildManager.Instance.SelectedTurrent == null) return;
        Debug.Log("Setting Turrent");
        Instantiate(BuildManager.Instance.SelectedTurrent, transform.position, Quaternion.identity);
    }


}
