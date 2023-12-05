using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static BuildManager Instance;
    private Node selectedNode;
    public NodeUI nodeUI;
    private TurretDesign selectedTurret;


    public TurretDesign SelectedTurret
    {
        get
        {
            return selectedTurret;
        }
        set
        {
            selectedTurret = value;
        }
    }

    public bool HasEnoughMoney
    {
        get
        {
            return PlayerStatus.Money >= SelectedTurret.cost;
        }
    }
    public bool CanBuild
    {
        get
        {
            return selectedTurret != null && selectedTurret.prefab != null;
        }
    }
    private void Awake()
    {
        Instance = this;
    }

    public void SelectNode(Node node)
    {
        if(selectedNode == node)
        {
            DeSelect();
            return;
        }
        selectedTurret = null;
        selectedNode = node;
        nodeUI.SetTarget(selectedNode);
        nodeUI.ShowUI();
    }

    public void DeSelect()
    {
        selectedNode = null;
        nodeUI.HideUI();
    }
}
