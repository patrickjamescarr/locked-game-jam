using UnityEngine;

public class ChaseState : ZombieState
{
	public Transform target { get; set; }

	public ChaseState(ZombieController controller, StateMachine stateMachine) : base(controller, stateMachine) { }

	public override void Enter()
	{
		controller.UpdatePath(target.position);
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (UnityEngine.Random.Range(0f, 50f) <= 1f)
		{
			controller.UpdatePath(target.position);
		}

		if (Random.Range(0f, 50f) <= 5)
			DetectPlayer();
	}

	private void DetectPlayer()
	{
		Collider2D[] result = Physics2D.OverlapCircleAll(controller.transform.position, controller.detectionDistance);

		foreach (var collider in result)
		{
			if (collider.CompareTag("Player"))
			{
				return;
			}
		}

		PlayerLost();
	}

	public override void PlayerLost()
	{
		controller.chase.target = null;
		stateMachine.ChangeState(controller.wander);
	}
}
