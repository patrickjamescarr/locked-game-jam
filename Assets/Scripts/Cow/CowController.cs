using Pathfinding;
using TMPro;
using UnityEngine;

public class CowController : MonoBehaviour, IDamageable
{
	[Header("Events")]
	public CowEventSO cowDied;
	public CowEventSO cowHerded;

	[Header("Movement")]
	public float acceleration = 5f;
	public float maxSpeed = 10f;
	public float nextWaypointDistance = 3f;
	public int wanderRange = 3;

	[Header("Stats")]
	public float health = 100f;
	public float followDistance = 2f;
	public float speed = 2f;

	[Header("Misc")]
	public GameObject damageTextPrefab;
	public GameObject blood;

	private StateMachine stateMachine;
	private Seeker seeker;
	public bool newTargetCalculated = true;
	private float initialHealth;

	public Vector3 target;
	public Rigidbody2D rb;
	public Mover mover;

	public PlayerController player;
	private bool isInPen = false;

	public CowWanderState wander;
	public CowFollowState herding;

	private Animator animator;
	private SpriteRenderer spriteRenderer;


	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		seeker = GetComponent<Seeker>();
		animator = GetComponentInChildren<Animator>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		initialHealth = health;

		mover = new Mover(nextWaypointDistance);

		stateMachine = new CowStateMachine();

		wander = new CowWanderState(this, stateMachine);
		herding = new CowFollowState(this, stateMachine);

		stateMachine.Initialize(wander);
	}

	public void TakeDamage(float damage)
	{
		health -= damage;

		Instantiate(blood, transform.position, Quaternion.identity);

		if (damageTextPrefab != null)
			DisplayDamageText(damage);

		if (health < 0)
			Die();
	}

	public void StartHerding(PlayerController player, out float herdSpeed)
	{
		herdSpeed = speed;
		this.player = player;

		stateMachine.ChangeState(herding);
	}

	public void IsInPen(bool value)
	{
		isInPen = value;
	}

	public void StopHerding()
	{
		if (isInPen)
		{
			Herded();
		}

		this.player = null;
		stateMachine.ChangeState(wander);
	}

	private void DisplayDamageText(float damage)
	{
		var go = Instantiate(damageTextPrefab, transform);
		var textMesh = go.GetComponentInChildren<TextMeshPro>();

		if (textMesh != null)
			textMesh.SetText(((int)damage).ToString());
	}

	public void UpdatePath(Vector3 newTarget)
	{
		newTargetCalculated = false;
		target = newTarget;

		if (seeker.IsDone() && newTarget != null)
			seeker.StartPath(rb.position, newTarget, OnPathComplete);
	}

	void Die()
	{
		cowDied?.RaiseEvent(this.gameObject);
	}

	void Herded()
	{
		cowHerded?.RaiseEvent(this.gameObject);
	}

	private void OnPathComplete(Path p)
	{
		newTargetCalculated = true;

		if (!p.error)
		{
			mover.SetPath(p);
		}
	}

	private void Update()
	{
		stateMachine.CurrentState.LogicUpdate();

		if (!newTargetCalculated)
			UpdatePath(target);
	}

	private void FixedUpdate()
    {
        stateMachine.CurrentState.PhysicsUpdate();

		if (stateMachine.CurrentState == wander)
		{
			UpdateGraphics(rb.velocity);
		}
    }

    public void UpdateGraphics(Vector2 velocity)
    {
        animator.SetFloat("Speed", velocity.magnitude);

        spriteRenderer.flipX = velocity.x < 0;
    }
}
