using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLevel : MonoBehaviour
{
    public SceneFader sceneFader;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NextLevelBtnClicked()
    {
        sceneFader.FadeOut(SceneManager.GetActiveScene().name);
    }

    public void MenuBtnClicked()
    {
        sceneFader.FadeOut("Mainmenu");
    }
}
