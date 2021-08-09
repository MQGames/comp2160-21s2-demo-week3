using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMove : MonoBehaviour
{
    public Transform[] patrol;
    public int nextCheckpoint = 0;

    public float speed = 5; // m/s
    public float checkpointRadius = 0.1f;

    void Start()
    {
        
    }

    void Update()
    {
        Patrol();
    }

    /**
     * Patrol along a path from one checkpoint to the next
     */
    void Patrol() 
    {
        // check if we've reached the checkpoint & move to the next
        Transform checkpoint = patrol[nextCheckpoint];
        Vector2 vCheckpoint = checkpoint.position - transform.position;

        if (vCheckpoint.magnitude < checkpointRadius)
        {
            nextCheckpoint = (nextCheckpoint + 1) % patrol.Length;
            checkpoint = patrol[nextCheckpoint];
            vCheckpoint = checkpoint.position - transform.position;
        }

        Vector2 move = vCheckpoint.normalized * speed * Time.deltaTime;

        // check if we are going to overshoot the checkpoint
        if (move.magnitude > vCheckpoint.magnitude)
        {
            move = vCheckpoint;
        }
        transform.Translate(move, Space.World);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        for (int i = 0; i < patrol.Length; i++) 
        {
            Vector3 p0 = patrol[i].position;
            Vector3 p1 = patrol[(i+1) % patrol.Length].position;
            Gizmos.DrawLine(p0, p1);
        }
    }
}
