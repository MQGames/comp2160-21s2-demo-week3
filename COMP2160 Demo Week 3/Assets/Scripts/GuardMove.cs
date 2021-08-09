using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMove : MonoBehaviour
{
    public Transform player;
    public Transform[] patrol;
    public float speed = 5; // m/s
    public float checkpointRadius = 0.1f; // m

    private int nextCheckpoint = 0;

    void Start()
    {
        nextCheckpoint = NearestCheckpoint();
    }

    void Update()
    {
        Patrol();
    }

    /**
     * Patrol along a path from one checkpoint to the next
     */
    private void Patrol() 
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

    private int NearestCheckpoint()
    {
        float distance = float.PositiveInfinity;
        int closest = -1;

        for (int i = 0; i < patrol.Length; i++)
        {
            Vector2 v = patrol[i].position - transform.position;
            if (distance > v.magnitude) 
            {
                distance = v.magnitude;
                closest = i;
            }
        }

        return closest;
    }


    /**
     * Chase the player
     */

    private void Chase()
    {
        Vector2 vPlayer = player.position - transform.position;
        Vector2 move = vPlayer.normalized * speed * Time.deltaTime;
        if (move.magnitude > vPlayer.magnitude)
        {
            move = vPlayer;
        }
        transform.Translate(move, Space.World);
    }

    private bool CanSeePlayer() 
    {
        Vector2 vPlayer = player.position - transform.position;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, vPlayer);

        if (hit.collider == null) 
        {
            return false;
        }

        return hit.collider.gameObject.CompareTag("Player");
    }


    void OnDrawGizmos()
    {
        // draw patrol path
        Gizmos.color = Color.yellow;

        for (int i = 0; i < patrol.Length; i++) 
        {
            Vector3 p0 = patrol[i].position;
            Vector3 p1 = patrol[(i+1) % patrol.Length].position;
            Gizmos.DrawLine(p0, p1);
        }

        // draw player visibility
        Gizmos.color = CanSeePlayer() ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, player.position);

    }
}
