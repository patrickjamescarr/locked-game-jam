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
	}

	public override void PlayerFound(GameObject player)
	{
		controller.chase.target = player.transform;
		stateMachine.ChangeState(controller.chase);
	}
}
