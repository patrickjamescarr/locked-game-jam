using UnityEngine;

public abstract class State
{
	protected StateMachine stateMachine;

	public State(StateMachine stateMachine)
	{
		this.stateMachine = stateMachine;
	}

	public virtual void Enter() { }
	public virtual void HandleInput() { }
	public virtual void LogicUpdate() { }
	public virtual void PhysicsUpdate() { }
	public virtual void PlayerFound(GameObject player) { }
	public virtual void PlayerLost() { }
	public virtual void Exit() { }
}
