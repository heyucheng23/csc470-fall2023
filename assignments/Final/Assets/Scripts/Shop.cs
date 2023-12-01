using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject standardTurrent;
    public void OnPurseStandardTurrent()
    {
        Debug.Log("buying");
        BuildManager.Instance.SelectedTurrent = standardTurrent;
    }

    public void OnPurseMissleTurret()
    {
        Debug.Log("buying");
        BuildManager.Instance.SelectedTurrent = standardTurrent;
    }

    public void OnPurseLaserBeamer()
    {
        Debug.Log("buying");
        BuildManager.Instance.SelectedTurrent = standardTurrent;
    }
}