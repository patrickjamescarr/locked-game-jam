using UnityEngine;

public class CowFollowState : CowState
{
	public CowFollowState(CowController controller, StateMachine stateMachine) : base(controller, stateMachine) { }

	public override void LogicUpdate()
	{
		if (controller.player != null)
		{
			float distance = Vector3.Distance(controller.transform.position, controller.player.transform.position);

			if (distance >= controller.followDistance)
			{
				FollowPlayer();
			}
		}
	}

	private void FollowPlayer()
	{
		Vector2 movement = (controller.player.transform.position - controller.transform.position).normalized;

		if (movement != Vector2.zero)
		{
			controller.transform.Translate(new Vector3(movement.x * controller.speed * Time.deltaTime, movement.y * controller.speed * Time.deltaTime));
			controller.UpdateGraphics(movement);
		}
	}
}