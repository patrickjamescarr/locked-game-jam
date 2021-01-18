public abstract class CowState : State
{
	protected CowController controller;

	protected CowState(CowController controller, StateMachine stateMachine) : base(stateMachine)
	{
		this.controller = controller;
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		controller.mover.PhysicsUpdate(controller.rb, controller.acceleration, controller.maxSpeed);
	}
}
