using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float moveSpeed = 50;
    public float scrollSpeed = 60;
    public float space = 50;
    public float Min_X = -100;
    public float Max_X = 35;
    public float Min_Y = 30;
    public float Max_Y = 120;
    public float Min_Z = -50;
    public float Max_Z = 50;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || (IsInScreenSpace() && Input.mousePosition.x < space && Input.mousePosition.x >=0))
        {
            transform.position += Vector3.left * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || (IsInScreenSpace() && Input.mousePosition.x > Screen.width - space && Input.mousePosition.x <= Screen.width))
        {
            transform.position += Vector3.right * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W) || (IsInScreenSpace() && Input.mousePosition.y > Screen.height - space && Input.mousePosition.y <= Screen.height))
        {
            transform.position += Vector3.forward * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || (IsInScreenSpace() && Input.mousePosition.y < space && Input.mousePosition.y >=0))
        {
            transform.position += Vector3.back * Time.deltaTime;
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            transform.position += Vector3.up * scroll * scrollSpeed * Time.deltaTime;
        }
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, Min_X, Max_X);
        pos.y = Mathf.Clamp(pos.y, Min_Y, Max_Y);
        pos.z = Mathf.Clamp(pos.z, Min_Z, Max_Z);
        transform.position = pos;
    }

    bool IsInScreenSpace()
    {
        return (Input.mousePosition.x >=0 && Input.mousePosition.x <=Screen.width && Input.mousePosition.y>=0 && Input.mousePosition.y <= Screen.height);
    }
}