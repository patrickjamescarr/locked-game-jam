using Pathfinding;
using System;
using TMPro;
using UnityEngine;

public class ZombieController : MonoBehaviour, IDamageable
{
	private StateMachine stateMachine;
	private Seeker seeker;
	private bool newTargetCalculated = true;

	public ChaseState chase;
	public WanderState wander;
	public AttackState attack;
	public Rigidbody2D rb;
	public Mover mover;

	[Header("Movement")]
	public Vector3 target;
	public float acceleration = 5f;
	public float maxSpeed = 10f;
	public float nextWaypointDistance = 3f;

	[Header("Stats")]
	public int health = 100;

	[Header("Misc")]
	public GameObject damageTextPrefab;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		seeker = GetComponent<Seeker>();

		mover = new Mover(nextWaypointDistance);

		stateMachine = new ZombieStateMachine(this);
		wander = new WanderState(this, stateMachine);
		chase = new ChaseState(this, stateMachine);
		attack = new AttackState(this, stateMachine);

		stateMachine.Initialize(wander);
	}

	public void UpdatePath(Vector3 newTarget)
	{
		newTargetCalculated = false;
		target = newTarget;

		if (seeker.IsDone() && newTarget != null)
			seeker.StartPath(rb.position, newTarget, OnPathComplete);
	}

	private void OnPathComplete(Path p)
	{
		newTargetCalculated = true;

		if (!p.error)
		{
			mover.SetPath(p);
		}
	}

	void Update()
	{
		stateMachine.CurrentState.HandleInput();
		stateMachine.CurrentState.LogicUpdate();

		if (!newTargetCalculated)
		{
			UpdatePath(target);
		}
	}

	private void FixedUpdate()
	{
		stateMachine.CurrentState.PhysicsUpdate();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			stateMachine.CurrentState.PlayerFound(other.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			stateMachine.CurrentState.PlayerLost();
		}
	}

	public void TakeDamage(float damage)
	{
		Debug.Log($"Taking damage {damage}");

		health -= (int)damage;

		if (damageTextPrefab != null)
			DisplayDamageText(damage);

		if (health < 0)
			Die();
	}

	private void DisplayDamageText(float damage)
	{
		var go = Instantiate(damageTextPrefab, transform);
		var textMesh = go.GetComponentInChildren<TextMeshPro>();

		if (textMesh != null)
			textMesh.SetText(((int)damage).ToString());
	}

	private void Die()
	{
		// TODO: Notify Points Scored

		Destroy(this.gameObject);
	}
}
