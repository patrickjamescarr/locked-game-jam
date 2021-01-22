using UnityEngine;

public class PickUp : MonoBehaviour
{
	public PickUpSO item;
	private SpriteRenderer sr;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		sr.sprite = item.sprite;
	}

	public void SetPickUp(PickUpSO pu)
	{
		item = pu;
		GetComponent<SpriteRenderer>().sprite = pu.sprite;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			var player = other.GetComponent<ICanPickUp>();

			if (player != null)
			{
				player.PickUp(item);

				if (!item.permanent)
				{
					Destroy(this.gameObject);
				}
			}
		}
	}
}
