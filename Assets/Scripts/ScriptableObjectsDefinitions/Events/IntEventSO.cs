using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Int Event Channel")]
public class IntEventSO : ScriptableObject
{
	public UnityAction<int> OnEventRaised;

	public void RaiseEvent(int val)
	{
		OnEventRaised?.Invoke(val);
	}
}
