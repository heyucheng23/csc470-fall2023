using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval;
    private float countDown;
    public Transform spawnPoint;
    private int waveNum = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
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
        for(int i =0; i <waveNum; i++)
        {
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(0.5f);
        }
        waveNum++;
    }
    
}
