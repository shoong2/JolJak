using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private float speed = 3f;

    void Update()
    {
        transform.Rotate(0f, -Input.GetAxis("Mouse X") * speed, 0f, Space.World);
       // transform.Rotate(-Input.GetAxis("Mouse Y") * speed, 0f, 0f);
    }
}
