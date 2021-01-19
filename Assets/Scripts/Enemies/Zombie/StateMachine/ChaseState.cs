using UnityEngine;

public class ChaseState : ZombieState
{
	private enum ZombieTargetType { None, Player, Cow };
	private ZombieTargetType targetType = ZombieTargetType.None;

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
		if (targetType != ZombieTargetType.None)
		{
			float distance = Vector2.Distance(controller.transform.position, target.position);

			if (distance > controller.detectionDistance)
				targetType = ZombieTargetType.None;

			// If target lost or targetting cow- check if anything else in range
			if (distance > controller.detectionDistance || targetType == ZombieTargetType.Cow)
			{
				if (!FindNewTarget())
				{
					PlayerLost();
				}
			}
		} else
		{
			if (!FindNewTarget())
			{
				PlayerLost();
			}
		}
	}

	private bool FindNewTarget()
	{
		Collider2D[] result = Physics2D.OverlapCircleAll(controller.transform.position, controller.detectionDistance);

		foreach (var collider in result)
		{
			if (collider.CompareTag("Player"))
			{
				if (targetType != ZombieTargetType.Player)
				{
					PlayerFound(collider.gameObject);
				}

				targetType = ZombieTargetType.Player;
				return true;
			}
			else if (targetType != ZombieTargetType.Cow && collider.CompareTag("Cow"))
			{
				PlayerFound(collider.gameObject);
				targetType = ZombieTargetType.Cow;
				return true;
			}
		}

		targetType = ZombieTargetType.None;
		return false;
	}
}
