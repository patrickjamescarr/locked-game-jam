using UnityEngine;

public class PenController : MonoBehaviour
{
	public GameObject herdButtonUI;
	public PlayerController player;

	private bool isHerding = false;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Player herding");

			herdButtonUI.SetActive(true);
			isHerding = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			herdButtonUI.SetActive(false);
			isHerding = false;
		}
	}

	private void Update()
	{
		if (isHerding && player != null)
		{
			if (Input.GetKeyDown(KeyCode.H))
			{
				Debug.Log("Trying to herd cow");
				// player.AttemptHerd();
			}
		}
	}
}
