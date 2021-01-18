using UnityEngine;

public class PenController : MonoBehaviour
{
	[Header("Objects")]
	public SpriteRenderer timerSprite;
	public GameObject herdButtonUI;

	[Header("Events")]
	public VoidEventSO gameStartEvent;
	public BoolEventSO penOpenEvent = default;

	[Header("Settings")]
	public float padlockTimer = 5f;
	public float cowReleaseIntervan = 3f;

	private float currentPadlockTime = 0f;
	private bool playerInPen = false;
	private bool penOpen = false;

	private Vector3 initialTimerScale;
	private Vector3 calculatedTimerScale;

	private void Start()
	{
		initialTimerScale = timerSprite.transform.localScale;
		ResetTimerScale();
	}

	private void OnEnable()
	{
		if (gameStartEvent != null)
			gameStartEvent.OnEventRaised += GameStart;
	}

	private void OnDisable()
	{
		if (gameStartEvent != null)
			gameStartEvent.OnEventRaised -= GameStart;
	}

	private void GameStart()
	{
		ResetTimerScale();
	}

	private void ResetTimerScale()
	{
		currentPadlockTime = 0;
		calculatedTimerScale = initialTimerScale;
		timerSprite.transform.localScale = calculatedTimerScale;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (penOpen)
			{
				penOpen = false;
				penOpenEvent?.RaiseEvent(false);
			}

			var player = other.GetComponent<PlayerController>();

			if (player.IsHerdingCow)
			{
				herdButtonUI.SetActive(true);
			}

			playerInPen = true;
			ResetTimerScale();
		}

		if (other.CompareTag("Cow"))
		{
			var cow = other.GetComponent<CowController>();
			cow.IsInPen(true);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			herdButtonUI.SetActive(false);
			playerInPen = false;	
		}

		if (other.CompareTag("Cow"))
		{
			var cow = other.GetComponent<CowController>();
			cow.IsInPen(false);
		}
	}

	private void Update()
	{
		if (!playerInPen && !penOpen)
		{
			if (currentPadlockTime <= padlockTimer)
			{
				UpdatePadlock();
			}
			else
			{
				OpenPen();
			}
		}
	}

	private void UpdatePadlock()
	{
		currentPadlockTime += Time.deltaTime;

		if (calculatedTimerScale.x > 0.0f)
		{
			calculatedTimerScale.x -= (Time.deltaTime / padlockTimer) * initialTimerScale.x;
			timerSprite.transform.localScale = calculatedTimerScale;
		}
	}

	private void OpenPen()
	{
		penOpen = true;
		penOpenEvent?.RaiseEvent(true);
	}
}
