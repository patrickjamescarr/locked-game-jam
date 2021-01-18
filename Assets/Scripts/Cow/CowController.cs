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

	[Header("Misc")]
	public GameObject damageTextPrefab;

	private bool isBeingHerded = false;

	public void TakeDamage(float damage)
	{
		health -= damage;

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

	void Die()
	{
		cowDied?.RaiseEvent(this.gameObject);
		Destroy(this.gameObject, 0.5f);
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
}
