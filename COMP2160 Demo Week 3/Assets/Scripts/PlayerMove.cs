using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5; // m/s
    public float turnSpeed = 60; // deg/s

    void Start()
    {
        
    }

    void Update()
    {
        float dx = Input.GetAxis(InputAxes.Horizontal);        
        float dy = Input.GetAxis(InputAxes.Vertical);

        Vector2 move = Vector2.up * dy * speed * Time.deltaTime;
        transform.Translate(move, Space.Self);

        float rotate = -turnSpeed * dx * Time.deltaTime;
        transform.Rotate(Vector3.forward * rotate); // rotate around the z axis
    }
}
