using Pathfinding;
using UnityEngine;

public class Mover
{
    private Path path;
    private int currentWaypoint = 0;
    private float nextWaypointDistance = 3f;

    public bool hasReachedEndOfPath { get; private set; } = true;

    public Mover(float nextWaypointDistance)
	{
        this.nextWaypointDistance = nextWaypointDistance;
	}

    public void SetPath(Path p)
	{
        path = p;
        currentWaypoint = 0;
        hasReachedEndOfPath = false;
	}

    public void PhysicsUpdate(Rigidbody2D rb, float acceleration, float maxSpeed)
	{
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
		{
            hasReachedEndOfPath = true;
            return;
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
