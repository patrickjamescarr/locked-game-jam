using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 1)]
public class GunSO : ScriptableObject
{
	private string gunName;

	[Range(0, 100f)]
	public float damage;

	[Range(0, 5f)]
	public float damageRange;

	[Range(0, 10f)]
	public float speed;

	public GameObject bulletPrefab;

	[Range(0, 10f)]
	public float cooldownTime;

	[Range(0, 10f)]
	public float reloadTime;

	[Range(0, 100)]
	public int bulletCount;
}
