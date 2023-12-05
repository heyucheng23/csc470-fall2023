using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretDesign
{
    public GameObject prefab;
    public GameObject upgradedPrefab;
    public int cost;
    public int upgradeCost;

    public int SellAmount
    {
        get
        {
            return cost/2;
        }
    }
}