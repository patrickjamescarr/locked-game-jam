using UnityEngine;

public class WanderState : ZombieState {
    public WanderState(ZombieController controller, StateMachine stateMachine) : base(controller, stateMachine) { }

	public override void Enter()
	{
		CalculateNewTarget();
	}

	private void CalculateNewTarget()
	{
		Vector3 newTarget = new Vector3(controller.rb.position.x + Random.Range(-10, 10), controller.rb.position.y + Random.Range(-10, 10));

		controller.UpdatePath(newTarget);
	}

	public override void LogicUpdate()
	{
		if (controller.mover.hasReachedEndOfPath)
			CalculateNewTarget();

		if (Random.Range(0f, 50f) <= 1)
			DetectPlayer();
	}

	private void DetectPlayer()
	{
		Collider2D[] result = Physics2D.OverlapCircleAll(controller.transform.position, controller.detectionDistance);

		foreach(var collider in result)
		{
			if (collider.CompareTag("Player"))
			{
				PlayerFound(collider.gameObject);
			}
		}
	}

	public override void PlayerFound(GameObject player)
	{
		controller.chase.target = player.transform;
		stateMachine.ChangeState(controller.chase);
	}
}
