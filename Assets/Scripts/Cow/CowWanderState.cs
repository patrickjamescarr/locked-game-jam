using UnityEngine;

public class CowWanderState : CowState
{
	public CowWanderState(CowController controller, StateMachine stateMachine) : base(controller, stateMachine) { }

	public override void Enter()
	{
		CalculateNewTarget();
	}

	public override void Exit()
	{
		controller.rb.velocity = Vector2.zero;
		controller.rb.angularVelocity = 0f;
		controller.mover.SetPath(null);
	}

	private void CalculateNewTarget()
	{
		Vector3 newTarget = new Vector3(controller.rb.position.x + Random.Range(-controller.wanderRange, controller.wanderRange), controller.rb.position.y + Random.Range(-controller.wanderRange, controller.wanderRange));

		controller.UpdatePath(newTarget);
	}

	public override void LogicUpdate()
	{
		if (controller.mover.hasReachedEndOfPath)
			CalculateNewTarget();
	}
}
