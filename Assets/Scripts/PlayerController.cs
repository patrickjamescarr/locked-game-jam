using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

	private void Update()
	{
		Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

		if (movement != Vector2.zero)
		{
			this.transform.Translate(new Vector3(movement.x * speed * Time.deltaTime, movement.y * speed * Time.deltaTime));
		}
	}
}
