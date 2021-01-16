using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    public float speed = 5f;
	public Camera cam;
	public Transform gunSprite;
	public GunSO gun;

	private void Start()
	{
		if (cam == null)
		{
			cam = Camera.main;
		}
	}

	private void Update()
	{
		MovePlayer();
		RotateGun();
		Shoot();
	}

	private void MovePlayer()
	{
		Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

		if (movement != Vector2.zero)
		{
			this.transform.Translate(new Vector3(movement.x * speed * Time.deltaTime, movement.y * speed * Time.deltaTime));
		}
	}

	private void RotateGun()
	{
		Vector2 mousePosition = Input.mousePosition;
		Vector2 gunPosition = cam.WorldToScreenPoint(gunSprite.position);

		direction = (mousePosition - gunPosition).normalized;

		float angle = AngleBetweenTwoPoints(mousePosition, gunPosition);

		gunSprite.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
	}

	Vector2 direction;

	private float AngleBetweenTwoPoints(Vector2 mousePosition, Vector2 gunPosition)
	{
		return Mathf.Atan2(mousePosition.y - gunPosition.y, mousePosition.x - gunPosition.x) * Mathf.Rad2Deg;
	}

	private void Shoot()
	{
		if (Input.GetMouseButtonDown(0))
		{
			gun.Shoot(this.transform, direction);
		}
	}

	public void TakeDamage(float damage)
	{
		Debug.Log($"Player being damaged: {damage}");
	}
}
