﻿using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable, ICanPickUp
{
	private bool canHerdCow = false;
	private bool flipped = false;
	private float speed = 5f;
	private bool isCurrentlyHerding = false;
	private CowController cowBeingHerded;
	private Vector3 initialPosition;
	private float initialHealth;

	public Camera cam;
	public Transform gunSprite;
	public GunSO gun;
	public GameObject damageTextPrefab;
	public Animator animator;

	public SpriteRenderer spriteRenderer;
	public AudioSource reloadSound;

	[Header("Events")]
	public BoolEventSO cowCanHerd;
	public VoidEventSO playerDied;
	public VoidEventSO restartGameEvent;
	public PlayerTakeDamageSO playerTakeDamage;

	[Header("Stats")]
	public float health = 100f;
	public float originalSpeed = 5f;
	public float herdRange = 2f;
	public AmmoInfo startingAmmo;

	private int clipSize = 0;
	private int currentBulletsInClip = 0;
	private int heldBullets = 0;

	[Header("Misc")]
	public GameObject blood;

	public bool IsHerdingCow
	{
		get
		{
			return (cowBeingHerded != null);
		}
	}

	private void Start()
	{
		if (cam == null)
		{
			cam = Camera.main;
		}

		speed = originalSpeed;
		initialPosition = transform.position;
		initialHealth = health;
		playerTakeDamage.RaiseEvent(initialHealth);
		gun.SetAmmo(startingAmmo);

		StoreInitialAmmo();
	}

	private void StoreInitialAmmo()
	{
		clipSize = startingAmmo.clipSize;
		currentBulletsInClip = startingAmmo.currentBulletsInClip;
		heldBullets = startingAmmo.heldBullets;
	}

	private void ResetInitialAmmo()
	{
		startingAmmo.clipSize = clipSize;
		startingAmmo.currentBulletsInClip = currentBulletsInClip;
		startingAmmo.heldBullets = heldBullets;
	}

	private void OnEnable()
	{
		if (restartGameEvent != null)
			restartGameEvent.OnEventRaised += RestartGame;
	}

	private void OnDisable()
	{
		if (restartGameEvent != null)
			restartGameEvent.OnEventRaised -= RestartGame;
	}

	private void RestartGame()
	{
		this.transform.position = initialPosition;
		speed = originalSpeed;
		health = initialHealth;
		playerTakeDamage.RaiseEvent(initialHealth);
		canHerdCow = false;
		ResetInitialAmmo();
		gun.SetAmmo(startingAmmo);
	}

	private void Update()
	{
		if (GameSettings.inGame)
		{
			MovePlayer();
			UpdateSprite();
			RotateGun();
			Shoot();
			HerdCow();
		}
	}

	public void CancelHerd()
	{
		TryStopHerdingCow();
	}

	private void HerdCow()
	{
		CheckCanHerdCow();

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (isCurrentlyHerding)
			{
				TryStopHerdingCow();
			}
			else
			{
				TryHerdCow();
			}
		}
	}

	private void CheckCanHerdCow()
	{
		if (Random.Range(0, 100f) < 10f)
		{
			Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, herdRange);

			foreach (var hit in hits)
			{
				if (hit.CompareTag("Cow"))
				{
					canHerdCow = true;
					cowCanHerd.RaiseEvent(true);
					return;
				}
			}

			canHerdCow = false;
			cowCanHerd.RaiseEvent(false);
		}
	}

	private void TryStopHerdingCow()
	{
		cowBeingHerded.StopHerding();
		speed = originalSpeed;
		cowBeingHerded = null;
		isCurrentlyHerding = false;
	}

	private void TryHerdCow()
	{
		if (canHerdCow)
		{
			Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, herdRange);

			foreach (var collider in hits)
			{
				var cow = collider.GetComponent<CowController>();

				if (cow != null)
				{
					cow.StartHerding(this, out speed);
					cowBeingHerded = cow;
					isCurrentlyHerding = true;
					break;
				}
			}
		}
	}

	private void UpdateSprite()
	{
		var mouseHorizontalPosition = cam.ScreenToWorldPoint(Input.mousePosition).x - this.transform.position.x;

		flipped = mouseHorizontalPosition < 0;

		transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));

		animator.SetBool("GunEquiped", !IsHerdingCow);

		gunSprite.gameObject.SetActive(!IsHerdingCow);
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

		Debug.Log(direction.x);

		// Some hacky stuff going on here to prevent pointing the gun at the character
		// and restricting bullet direction based on which way the character is facing
		if (!flipped)
		{
			direction.x = Mathf.Clamp(direction.x, 0.2f, 1.0f);

			angle = Mathf.Clamp(angle, -90f, 70f);
		}
		else
		{
			direction.x = Mathf.Clamp(direction.x, -1.0f, -0.2f);

			if (angle < 0)
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
			direction.y = 0.9f;
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
			if (isCurrentlyHerding)
			{
				CancelHerd();
			}

			gun.Shoot(this.gunSprite, direction, () => { reloadSound.Play(); });
		}
	}

	public void TakeDamage(float damage)
	{
		if (damageTextPrefab != null)
			DisplayDamageText(damage);

		health -= damage;

		Instantiate(blood, transform.position, Quaternion.identity);

		playerTakeDamage.RaiseEvent(health);

		if (health <= 0f)
			Die();
	}

	private void Die()
	{
		playerDied.RaiseEvent();
	}

	private void DisplayDamageText(float damage)
	{
		var go = Instantiate(damageTextPrefab, transform);
		var textMesh = go.GetComponentInChildren<TextMeshPro>();

		if (textMesh != null)
			textMesh.SetText(((int)damage).ToString());
	}

	public void PickUp(PickUpSO item)
	{
		if (item != null)
		{
			gun.AddAmmo(item.count);
		}
	}
}
