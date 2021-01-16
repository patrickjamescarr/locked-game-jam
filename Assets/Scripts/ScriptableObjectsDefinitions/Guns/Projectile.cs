using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	private float duration;
	private float currentDuration = 0.0f;
	private Action<Projectile> deactivateCallback;
	private Vector3 shootDirection;
	private float shotSpeed;

	private float damageValue;

	public void Shoot(Vector2 direction, float speed, float shotDuration, float damage, Action<Projectile> callback)
	{
		shootDirection = direction;
		deactivateCallback = callback;
		currentDuration = 0.0f;
		duration = shotDuration;
		shotSpeed = speed;
		damageValue = damage;
	}

	public int GetDamage()
	{
		return 10;
	}

	private void Update()
	{
		transform.position += shootDirection * shotSpeed * Time.deltaTime;

		if (currentDuration <= duration)
		{
			currentDuration += Time.deltaTime;
		}
		else
		{
			DeactivateProjectile();
		}
	}

	void DeactivateProjectile()
	{
		deactivateCallback?.Invoke(this);
		this.gameObject.SetActive(false);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		var damageable = other.gameObject.GetComponent<IDamageable>();

		if (damageable != null)
		{
			damageable.TakeDamage(damageValue);
		}

		DeactivateProjectile();
	}
}
