using TMPro;
using UnityEngine;

public class CowController : MonoBehaviour, IDamageable
{
	[Header("Events")]
	public CowEventSO cowDied;
	public CowEventSO cowHerded;
	public BoolEventSO cowCanHerd;

	[Header("Stats")]
	public float health = 100f;
	public float followDistance = 2f;
	public float speed = 2f;

	[Header("Misc")]
	public GameObject damageTextPrefab;

	private bool isBeingHerded = false;
	private PlayerController player;
	private bool isInPen = false;

	public void TakeDamage(float damage)
	{
		health -= damage;

		if (damageTextPrefab != null)
			DisplayDamageText(damage);

		if (health < 0)
			Die();
	}

	public void StartHerding(PlayerController player, out float herdSpeed)
	{
		herdSpeed = speed;
		this.player = player;
		isBeingHerded = true;
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
		isBeingHerded = false;
	}

	private void DisplayDamageText(float damage)
	{
		var go = Instantiate(damageTextPrefab, transform);
		var textMesh = go.GetComponentInChildren<TextMeshPro>();

		if (textMesh != null)
			textMesh.SetText(((int)damage).ToString());
	}

	void Die()
	{
		cowDied?.RaiseEvent(this.gameObject);
	}

	void Herded()
	{
		cowHerded?.RaiseEvent(this.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			cowCanHerd?.RaiseEvent(true);
			isBeingHerded = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player") && isBeingHerded)
		{
			cowCanHerd?.RaiseEvent(false);
			isBeingHerded = false;
		}
	}

	private void Update()
	{
		if (isBeingHerded && player != null)
		{
			float distance = Vector3.Distance(this.transform.position, player.transform.position);

			if (distance >= followDistance)
			{
				FollowPlayer();
			}
		}
	}

	private void FollowPlayer()
	{
		Vector2 movement = (player.transform.position - this.transform.position).normalized;

		if (movement != Vector2.zero)
		{
			this.transform.Translate(new Vector3(movement.x * speed * Time.deltaTime, movement.y * speed * Time.deltaTime));
		}
	}
}
