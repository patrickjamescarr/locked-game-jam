using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    private StateMachine stateMachine;

    public State chase;
    public State wander;
    public State attack;
    private Rigidbody2D rb;


    [Header("Movement")]
    public Transform target;
    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public float nextWaypointDistance = 3f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    Seeker seeker;

    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        chase = new ChaseState(this);
        wander = new WanderState(this);
        attack = new AttackState(this);

        stateMachine = new ZombieStateMachine(this);

        stateMachine.Initialize(wander);

        //InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
	{
        if (seeker.IsDone() && target != null)
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

	private void OnPathComplete(Path p)
	{
        if (!p.error)
        {
            this.path = p;
            currentWaypoint = 0;
        }
	}

	void Update()
    {
        stateMachine.CurrentState.HandleInput();
        stateMachine.CurrentState.LogicUpdate();

        if (UnityEngine.Random.Range(0f, 50f) <= 1f)
		{
            UpdatePath();
		}
    }

	private void FixedUpdate()
	{
        stateMachine.CurrentState.PhysicsUpdate();

        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * acceleration * Time.deltaTime;

        if (rb.velocity.magnitude < maxSpeed)
		{
			rb.AddForce(force);
		}

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance <= nextWaypointDistance)
            currentWaypoint++;
    }
}
