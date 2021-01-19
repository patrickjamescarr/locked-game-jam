using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Ammo Event Channel")]
public class AmmoEventSO : ScriptableObject
{
	public UnityAction<AmmoInfo> OnEventRaised;

	public void RaiseEvent(AmmoInfo ammo)
	{
		OnEventRaised?.Invoke(ammo);
	}
}