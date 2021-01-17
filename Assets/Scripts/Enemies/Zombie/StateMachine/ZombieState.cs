public abstract class ZombieState : State
{
    protected ZombieController controller;

	protected ZombieState(ZombieController controller, StateMachine stateMachine) : base(stateMachine)
	{
		this.controller = controller;
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		controller.mover.PhysicsUpdate(controller.rb, controller.acceleration, controller.maxSpeed);
	}
}
