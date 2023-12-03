using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretDesign standardTurrent;
    public TurretDesign missileLauncher;
    public TurretDesign laserBeamer;
    public void OnPurseStandardTurrent()
    {
        Debug.Log("buying");
        BuildManager.Instance.SelectedTurrent = standardTurrent;
    }

    public void OnPurseMissleTurret()
    {
        Debug.Log("buying");
        BuildManager.Instance.SelectedTurrent = missileLauncher;
    }

    public void OnPurseLaserBeamer()
    {
        Debug.Log("buying");
        BuildManager.Instance.SelectedTurrent = laserBeamer;
    }
}