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
    public GameObject turretPrefab;
    private GameObject turret;
    public Vector3 uiOffset = new Vector3(0,0,0);
    public TurretDesign selectedTurretDesign;
    public bool isUpgraded = false;

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
        if(turret !=null)
        {
            BuildManager.Instance.SelectNode(this);
            return;
        }
        BuildManager.Instance.DeSelect();
        if (!BuildManager.Instance.CanBuild) return;
        if(PlayerStatus.Money >= BuildManager.Instance.SelectedTurret.cost)
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
        PlayerStatus.Money -= BuildManager.Instance.SelectedTurret.cost;
        GameObject _turret = Instantiate(BuildManager.Instance.SelectedTurret.prefab, GetPosition(), Quaternion.identity);
        turret = _turret;
        selectedTurretDesign = BuildManager.Instance.SelectedTurret;
    }

    private Vector3 GetPosition()
    {
        return transform.position;
    }
    public Vector3 GetUiOffsetPosition()
    {
        return transform.position + uiOffset;
    }

    public void UpgradeTurret()
       
    {
        if(PlayerStatus.Money < selectedTurretDesign.upgradeCost)
        {
            Debug.Log("Not enough money");
            return;
        }
        PlayerStatus.Money -= selectedTurretDesign.upgradeCost;
        Destroy(turret);
        GameObject _turret = Instantiate(selectedTurretDesign.upgradedPrefab,GetPosition(), Quaternion.identity);
        turret = _turret;
        isUpgraded = true;
    }

    public void SellTurret()
    {
        PlayerStatus.Money += selectedTurretDesign.SellAmount;
        Destroy(turret);
        selectedTurretDesign = null;
        isUpgraded = false;
    }
}
