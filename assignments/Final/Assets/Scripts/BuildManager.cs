using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static BuildManager Instance;
    private TurretDesign selectedTurret;
    public TurretDesign SelectedTurrent
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
            return PlayerStatus.Money >= SelectedTurrent.cost;
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
