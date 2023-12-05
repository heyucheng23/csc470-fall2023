using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float range = 20;
    public string enemyTag = "Enemy";
    public Transform target;
    public Transform partRotate;
    public Transform bulletPoint;
    public float rotSpeed = 10;

    [Header("useBullet")]
    public GameObject bulletPrefab;
    public float bulletRate =  2f; 
    private float countDown = 0;

    [Header("useLaser")]
    public bool useLaser = false;
    public float overTimeHp = 30;
    public float slowPct = 0.3f;
    private EnemyAI enemyMove;
    private EnemyHealth enemyHp;
    public ParticleSystem impactEffect;
    public Light pointLight;

    public LineRenderer lineRender;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget",0,0.5f);
        countDown = 1/bulletRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            if(useLaser)
            {
                lineRender.enabled = false;
                impactEffect.Stop();
                pointLight.enabled = false;
            }
            return;
        }
        LockTarget();

        if(useLaser)
        {
            Laser();
        }
        else
        { 
            countDown -= Time.deltaTime;
            if(countDown<=0)
            {   
                Shoot();
                countDown = 1/bulletRate;
            }
        }
    }

    private void Laser()
    {   
        enemyHp.Damage(overTimeHp * Time.deltaTime);
        enemyMove.Slow(slowPct);
        if(!lineRender.enabled)
        {
            lineRender.enabled = true;
            impactEffect.Play();
            pointLight.enabled = true;
        }
        lineRender.SetPosition(0,bulletPoint.position);
        lineRender.SetPosition(1,target.position);

        Vector3 dir = bulletPoint.position - target.position;
        impactEffect.transform.position = target.position + dir.normalized * 1;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir.normalized);
    }

    private void Shoot()
    {
            Debug.Log("Firing");
            GameObject bulletGo = Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);
            Bullet bullet = bulletGo.GetComponent<Bullet>();
            if(bullet == null)
            {
                bullet = bulletGo.AddComponent<Bullet>();
            }
            bullet.SetTarget(target);
    }

    private void OnDrawGizmosSelected()
    {   
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,range);
    }
    private void UpdateTarget()
    {
        GameObject[]enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float minDistance = Mathf.Infinity;
        Transform nearestEnemy = null;
        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }
        if(minDistance < range)
        {
            target = nearestEnemy;
            enemyHp = target.GetComponent<EnemyHealth>();
            enemyMove = target.GetComponent<EnemyAI>();
        }
        else
        {
            target = null;
        }
    }
    private void LockTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir);
        Quaternion lerpRot = Quaternion.Lerp(partRotate.rotation,rotation,Time.deltaTime * rotSpeed);
        partRotate.rotation = Quaternion.Euler(new Vector3(0, lerpRot.eulerAngles.y, 0));
    }
}
