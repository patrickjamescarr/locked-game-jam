using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour
{
    public BoolEventSO displayHudEventChannel = default;

	public List<GameObject> HudItems;

	private void OnEnable()
	{
		if (displayHudEventChannel != null)
			displayHudEventChannel.OnEventRaised += DisplayHud;
	}

	private void OnDisable()
	{
		if (displayHudEventChannel != null)
			displayHudEventChannel.OnEventRaised -= DisplayHud;
	}

	private void DisplayHud(bool display)
	{
		foreach (var item in HudItems)
			item.SetActive(display);
	}
}
