using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform m_Target;
    public float speed = 80;
    public float damage = 50;

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
        EnemyDamage();
        Destroy(gameObject);
    }
    private void EnemyDamage()
    {
        EnemyHealth enemyHp = m_Target.GetComponent<EnemyHealth>();
        if(enemyHp != null)
        {
            enemyHp.Damage(damage);
        }
    }
}
