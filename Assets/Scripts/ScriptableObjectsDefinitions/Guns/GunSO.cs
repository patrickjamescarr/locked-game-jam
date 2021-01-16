using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 1)]
public class GunSO : ScriptableObject
{
	public string gunName;
	private List<Projectile> availableProjectiles;

	[Range(0, 100f)]
	public float damage;

	[Range(0, 5f)]
	public float damageRange;

	[Range(0, 500f)]
	public float speed;

	public GameObject bulletPrefab;

	[Range(0, 10f)]
	public float cooldownTime;

	[Range(0, 10f)]
	public float reloadTime;

	[Range(0, 100)]
	public int bulletCount;

	private void Awake()
	{
		availableProjectiles = new List<Projectile>();
		availableProjectiles.Clear();
	}

	public void Shoot(Transform transform, Vector2 direction)
	{
		var bullet = GetProjectile(transform);
		bullet.Shoot(direction, speed, damageRange, damage, DeactivateProjectile);	
	}

	private Projectile GetProjectile(Transform transform)
	{
		Projectile proj = availableProjectiles.FirstOrDefault(x => x != null);

		if (proj == null)
		{
			var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
			return bullet.GetComponent<Projectile>();
		} else
		{
			proj.transform.position = transform.position;
			proj.transform.rotation = transform.rotation;
			proj.gameObject.SetActive(true);
			return proj;
		}
	}

	private void DeactivateProjectile(Projectile projectile)
	{
		availableProjectiles.Add(projectile);
	}
}
