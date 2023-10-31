using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager SharedInstance;

    public TMP_Text scoreText;
    int score = 0;

    void Awake()
    {
        if (SharedInstance != null)
        {
            Debug.Log("There should only be one GameManager!");
        }
        SharedInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int amount)
    {
        score += amount;
        scoreText.text = "There are five rings in the map, you have found" + score.ToString();
    }
}
    

