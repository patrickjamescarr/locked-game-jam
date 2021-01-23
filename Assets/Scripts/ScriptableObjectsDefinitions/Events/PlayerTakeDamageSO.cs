using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Player Take Damage Event Channel")]
public class PlayerTakeDamageSO : ScriptableObject
{
	public UnityAction<float> OnEventRaised;

	public void RaiseEvent(float health)
	{
		OnEventRaised?.Invoke(health);
	}
}
