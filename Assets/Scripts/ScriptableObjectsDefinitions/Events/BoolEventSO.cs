using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Bool Event Channel")]
	public class BoolEventSO : ScriptableObject
	{
		public UnityAction<bool> OnEventRaised;

		public void RaiseEvent(bool val)
		{
			OnEventRaised?.Invoke(val);
		}
	}
