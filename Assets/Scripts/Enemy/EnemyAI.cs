using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float speed = 20f;
    public float nextWayPointDistance = 3f;
    public bool canFly;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);

    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(path == null)
        {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;  // force that will move the enemy in the desired direction

        Vector2 velocity;

        if (canFly)
        {
            // If a flyer, apply velocity in all directions
            // rb.AddForce(force);  // the old method
            velocity = force;
            rb.velocity = velocity;
        }
        else
        {
            // If not a flyer, only apply velocity to the x axis
            //force.y = 0;
            //velocity.x = force.x;
            //rb.velocity = velocity;
            force = new Vector2(force.x, 0);
            rb.AddForce(force);
            //rb.velocity = new Vector2(velocity.x, 0);

        }

        float distance = Vector2.Distance(rb.position,path.vectorPath[currentWaypoint]);

        if(distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }
    }
}
