using UnityEngine;

public class CowFollowState : CowState
{
	public CowFollowState(CowController controller, StateMachine stateMachine) : base(controller, stateMachine) { }

	public override void LogicUpdate()
	{
		if (controller.player != null)
		{
			float distance = Vector3.Distance(controller.transform.position, controller.player.transform.position);

			if (distance <= controller.followDistance)
			{
				FollowPlayer();
			} else
			{
				controller.player.CancelHerd();
				stateMachine.ChangeState(controller.wander);
			}
		}
	}

	private void FollowPlayer()
	{
		Vector2 movement = (controller.player.transform.position - controller.transform.position).normalized;

		if (movement != Vector2.zero)
		{
			var x = movement.x * controller.speed * Time.deltaTime;
			var y = movement.y* controller.speed* Time.deltaTime;

			controller.transform.Translate(new Vector3(x, y));
			controller.UpdateGraphics(movement);
		}
	}
}