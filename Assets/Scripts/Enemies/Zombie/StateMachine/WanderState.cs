using UnityEngine;

public class WanderState : ZombieState {
    public WanderState(ZombieController controller)
    {
        this.controller = controller;
    }

	public override void Enter()
	{
		//Debug.Log("Entering WanderState");
	}

	public override void HandleInput()
	{
		//Debug.Log("WanderState.HandleInput");
	}

	public override void LogicUpdate()
	{
		//Debug.Log("WanderState.LogicUpdate");
	}

	public override void PhysicsUpdate()
	{
		//Debug.Log("WanderState.PhysicsUpdate");
	}

	public override void Exit()
	{
		//Debug.Log("WanderState.Exit");
	}
}
