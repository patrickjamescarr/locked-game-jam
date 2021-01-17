using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Cow Event Channel")]
public class CowEventSO : ScriptableObject
{
	public UnityAction<GameObject> OnEventRaised;

	public void RaiseEvent(GameObject go)
	{
		OnEventRaised?.Invoke(go);
	}
}
