using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Camera cam;
	public Transform gunSprite;
	public GunSO gun;
	public GameObject damageTextPrefab;
	public Animator animator;
	public SpriteRenderer spriteRenderer;

	[Header("Stats")]
	public float health = 100f;
	public float speed = 5f;


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
		UpdateSprite();
		RotateGun();
		Shoot();
	}

	private void UpdateSprite()
	{
		var mouseHorizontalPosition = cam.ScreenToWorldPoint(Input.mousePosition).x - this.transform.position.x;

		spriteRenderer.flipX = mouseHorizontalPosition < 0;

		Debug.Log(mouseHorizontalPosition);


	}

	private void MovePlayer()
	{
		Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

		animator.SetFloat("Speed", Mathf.Abs(movement.magnitude * speed));

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
		if (damageTextPrefab != null)
			DisplayDamageText(damage);

		health -= damage;

		if (health <= 0f)
			Die();
	}

	private void Die()
	{
		Debug.Log("Player died!");
	}

	private void DisplayDamageText(float damage)
	{
		var go = Instantiate(damageTextPrefab, transform);
		var textMesh = go.GetComponentInChildren<TextMeshPro>();

		if (textMesh != null)
			textMesh.SetText(((int)damage).ToString());
	}
}
