using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{   
    public Color hoverColor = Color.gray;
    private Color initColor;
    public Color notEnoughMoneyColor;
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
        if(EventSystem.current.IsPointerOverGameObject()) return;
        if (!BuildManager.Instance.CanBuild) return;
        if(!BuildManager.Instance.HasEnoughMoney)
        {
            render.material.color = notEnoughMoneyColor;
            return;
        }
        render.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        render.material.color = initColor;
    }

    private void OnMouseDown()
    {
        if(EventSystem.current.IsPointerOverGameObject()) return;
        if (!BuildManager.Instance.CanBuild) return;
        if(PlayerStatus.Money >= BuildManager.Instance.SelectedTurrent.cost)
        {
            BuildTurret();
            Debug.Log(PlayerStatus.Money);
        }
        else
        {
            Debug.Log("You do not have enough gold");
        }
        
    }
    public void BuildTurret()
    {
        PlayerStatus.Money -= BuildManager.Instance.SelectedTurrent.cost;
        Instantiate(BuildManager.Instance.SelectedTurrent.prefab, GetPosition(), Quaternion.identity);
    }

    private Vector3 GetPosition()
    {
        return transform.position;
    }
}
