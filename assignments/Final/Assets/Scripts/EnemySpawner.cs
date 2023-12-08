using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public static int EnemyAlive;
    public Wave[] waveEnemy;
    public GameObject enemyPrefab;
    public float spawnInterval = 1f;
    private float countDown;
    public Transform spawnPoint;
    private int waveIndex;

    public Text timerText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(GameManager.GameIsOver)
        {
            EnemyAlive = 0;
            return;
        }
        if(EnemyAlive>0)
        {
            return;
        }
        if(waveIndex == waveEnemy.Length)
        {
            Debug.Log("Win");
            GameManager.Instance.GameWin();
            this.enabled = false;
        }
        countDown -= Time.deltaTime;
        countDown = Mathf.Clamp(countDown,0,Mathf.Infinity);
        string time = string.Format("{0:00.00}", countDown);
        timerText.text = time;
        if(countDown <=0)
        {
            countDown = spawnInterval;
            SpawnEnemy();
        }
        
    }
    private void SpawnEnemy()
    {
        StartCoroutine(WaveEnemy());
       
    }
    IEnumerator WaveEnemy()
    {   
        if(waveIndex >= waveEnemy.Length)
        {
            yield break;
        }
        PlayerStatus.Rounds++;
        Wave wave = waveEnemy[waveIndex];
        EnemyAlive = wave.count;
        for(int i =0; i <wave.count; i++)
        {
            Instantiate(wave.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(1/wave.rate);
        }
        waveIndex++;
    }
    
}
