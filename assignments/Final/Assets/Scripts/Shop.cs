using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretDesign standardTurret;
    public TurretDesign missileLauncher;
    public TurretDesign laserBeamer;
    public void OnPurseStandardTurret()
    {
        Debug.Log("buying");
        BuildManager.Instance.SelectedTurret = standardTurret;
    }

    public void OnPurseMissleTurret()
    {
        Debug.Log("buying");
        BuildManager.Instance.SelectedTurret = missileLauncher;
    }

    public void OnPurseLaserBeamer()
    {
        Debug.Log("buying");
        BuildManager.Instance.SelectedTurret = laserBeamer;
    }
}