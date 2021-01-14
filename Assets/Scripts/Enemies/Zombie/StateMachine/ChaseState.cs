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
	}
}
