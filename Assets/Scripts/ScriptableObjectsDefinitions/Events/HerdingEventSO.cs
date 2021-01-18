using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Herding Event Channel")]
public class HerdingEventSO : ScriptableObject
{
	public UnityAction<HerdingState> OnEventRaised;

	public void RaiseEvent(HerdingState herding)
	{
		OnEventRaised?.Invoke(herding);
	}
}
