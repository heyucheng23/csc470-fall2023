using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 10;
    Transform target;
    private int pointIndex;
    // Start is called before the first frame update
    void Start()
    {
        target = PathPoints.pathPoints[pointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);
        if(Vector3.Distance(target.position, transform.position)< 0.2f)
        {
            pointIndex++;
            if(pointIndex >= PathPoints.pathPoints.Length)
            {
                Destroy(gameObject);
                return;
            }
            target = PathPoints.pathPoints[pointIndex];
        }
    }
}
