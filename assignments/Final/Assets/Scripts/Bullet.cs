using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform m_Target;
    public float speed = 80;
    public float damage = 50;
    public float exploseRadius = 10;

    public void SetTarget(Transform target)
    {
        m_Target = target;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Target == null) 
    {
        Destroy(gameObject);
        return;
    }
        Vector3 dir = m_Target.position - transform.position;
        if(Vector3.Distance(m_Target.position, transform.position)< speed * Time.deltaTime)
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        transform.LookAt(m_Target);
    }
    private void HitTarget()
    {   
        if(exploseRadius > 0)
        {
            Explose();
        }
        else
        {
        EnemyDamage(m_Target);
        Destroy(gameObject);
        }
    }

    private void Explose()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, exploseRadius);
        foreach (var item in colliders)
        {
            if(item.tag == "Enemy")
            {
                Debug.Log("explose");
                EnemyDamage(item.transform);
            }
        }
    }   
    private void EnemyDamage(Transform enemy)
    {
        EnemyHealth enemyHp = m_Target.GetComponent<EnemyHealth>();
        if(enemyHp != null)
        {
            enemyHp.Damage(damage);
        }
    }
}
