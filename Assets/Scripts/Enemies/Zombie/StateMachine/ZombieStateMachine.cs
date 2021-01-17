public class ZombieStateMachine : StateMachine
{
    private ZombieController controller;

	public ZombieStateMachine(ZombieController controller)
	{
        this.controller = controller;
	}
}
