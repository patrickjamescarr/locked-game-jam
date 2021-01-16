using Pathfinding;
using TMPro;
using UnityEngine;

public class ZombieController : MonoBehaviour, IDamageable
{
	private StateMachine stateMachine;
	private Seeker seeker;
	private bool newTargetCalculated = true;
	private float timeSinceDamage = 0f;

	public ChaseState chase;
	public WanderState wander;
	public AttackState attackState;
	public Rigidbody2D rb;
	public Mover mover;

	[Header("Movement")]
	public Vector3 target;
	public float acceleration = 5f;
	public float maxSpeed = 10f;
	public float nextWaypointDistance = 3f;

	[Header("Stats")]
	public int health = 100;
	public float attack = 10f;
	public float waitBetweenAttacks = 2f;
	public float detectionDistance = 10f;

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
		attackState = new AttackState(this, stateMachine);

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

	public void PlayerFound(GameObject player)
	{
		stateMachine.CurrentState.PlayerFound(player);
	}

	public void PlayerLost(GameObject player)
	{
		stateMachine.CurrentState.PlayerLost();
	}

	public void TakeDamage(float damage)
	{
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

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			var playerController = other.gameObject.GetComponent<PlayerController>();
			DealPlayerDamage(playerController);
		}
	}

	private void DealPlayerDamage(PlayerController player)
	{
		player.TakeDamage(attack);
		timeSinceDamage = 0f;
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			timeSinceDamage += Time.deltaTime;

			if (timeSinceDamage >= waitBetweenAttacks)
			{
				var playerController = other.gameObject.GetComponent<PlayerController>();
				DealPlayerDamage(playerController);
			}
		}
	}
}
