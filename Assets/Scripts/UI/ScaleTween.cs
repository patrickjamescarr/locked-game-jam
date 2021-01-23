using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTween : MonoBehaviour
{
	private void OnEnable()
	{
		transform.localScale = new Vector3(0, 0, 0);
		LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.3f).setIgnoreTimeScale(true);
	}

	public void CloseMe()
	{
		LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.5f).setIgnoreTimeScale(true).setOnComplete(DisableMe);
	}

	void DisableMe()
	{
		this.gameObject.SetActive(false);
	}
}
