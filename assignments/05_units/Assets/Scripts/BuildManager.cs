using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static BuildManager Instance;
    private GameObject selectedTurret;
    public GameObject SelectedTurrent
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
