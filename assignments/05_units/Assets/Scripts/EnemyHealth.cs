using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float InitHealth = 100;
    private float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = InitHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damage(float amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            EnemySpawner.EnemyAlive--;
            Destroy(gameObject);
        }
    }
}
