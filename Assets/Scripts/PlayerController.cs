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

	private bool flipped = false;

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

		flipped = mouseHorizontalPosition < 0;

		transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));
	}

	private void MovePlayer()
	{
		Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

		animator.SetFloat("Speed", Mathf.Abs(movement.magnitude * speed));

		if (movement != Vector2.zero)
		{
			var xMovement = movement.x * speed * Time.deltaTime;

			this.transform.Translate(new Vector3(flipped ? -xMovement : xMovement, movement.y * speed * Time.deltaTime));
		}
	}

	private void RotateGun()
	{
		Vector2 mousePosition = Input.mousePosition;
		Vector2 gunPosition = cam.WorldToScreenPoint(gunSprite.position);

		direction = (mousePosition - gunPosition).normalized;

		float angle = AngleBetweenTwoPoints(mousePosition, gunPosition);

		// Some hacky stuff going on here to prevent pointing the gun at the character
		// and restricting bullet direction based on which way the character is facing
		if(!flipped)
        {
			direction.x = Mathf.Clamp(direction.x, 0f, 1.0f);

			angle = Mathf.Clamp(angle, -90f, 70f);
        }
		else
        {
			direction.x = Mathf.Clamp(direction.x, -1.0f, 0f);
			
			if(angle < 0)
            {
				angle = Mathf.Clamp(angle, -180f, -90f);
            }
			else
            {
				angle = Mathf.Clamp(angle, 110f, 180f);
			}
		}

		if (direction.x == 0f && direction.y > 0)
		{
			direction.y = 1.0f;
		}
		if (direction.x == 0f && direction.y < 0)
		{
			direction.y = -1.0f;
		}


		// flip the gun sprite on the y axis when facing left
		gunSprite.GetComponent<SpriteRenderer>().flipY = flipped;

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
			gun.Shoot(this.gunSprite, direction);
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
