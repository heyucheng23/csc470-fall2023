using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject GameOverUI;
    public static bool GameIsOver;
    // Start is called before the first frame update
    void Start()
    {
        GameIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameIsOver)
        {
            return;
        }

        if(PlayerStatus.Lives <=0)
        {
            GameEnd();
        }
    }

    private void GameEnd()
    {   
        GameIsOver = true;
        Debug.Log("Game over");
        GameOverUI.SetActive(true);
    }
}
