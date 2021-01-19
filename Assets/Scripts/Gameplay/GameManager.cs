﻿using TMPro;
using UnityEngine;

public static class GameSettings
{
	public static bool inGame = false;
}

public class GameManager : MonoBehaviour
{
	private CowManager cowManager;
	private bool isMuted = true;
	private float initialVolume = 30f;

	[Header("UI")]
	public GameObject pauseUI;
	public GameObject herdInstructionUI;
	public GameObject successUI;
	public TMP_Text cowsSavedText;
	public GameObject padlockWarningUI;

	[Header("Events")]
    [SerializeField] private VoidEventSO quitGameEvent = default;
	[SerializeField] private BoolEventSO cowCanHerd = default;
	[SerializeField] private VoidEventSO playerDiedEvent = default;
	[SerializeField] private HerdingEventSO cowHerdingComplete = default;
	[SerializeField] private VoidEventSO startGameEvent = default;
	[SerializeField] private VoidEventSO restartGameEvent = default;
	[SerializeField] private BoolEventSO penOpenEvent = default;

	private void Start()
	{
		cowManager = GetComponent<CowManager>();

		initialVolume = AudioListener.volume;
		AudioListener.volume = 0;

		StartGame();
	}

	private void StartGame()
	{
		ClearAllUI();
		cowManager.StartGame();
		Time.timeScale = 1;
		startGameEvent?.RaiseEvent();

		GameSettings.inGame = true;
	}

	void OnEnable()
	{
		if (quitGameEvent != null)
			quitGameEvent.OnEventRaised += QuitGame;

		if (cowCanHerd != null)
			cowCanHerd.OnEventRaised += DisplayHerdInstructions;
	
		if (playerDiedEvent != null)
			playerDiedEvent.OnEventRaised += PlayerDied;

		if (cowHerdingComplete != null)
			cowHerdingComplete.OnEventRaised += CowHerdingComplete;

		if (restartGameEvent != null)
			restartGameEvent.OnEventRaised += RestartGame;

		if (penOpenEvent != null)
			penOpenEvent.OnEventRaised += DisplayPadlockWarning;
    }

	private void OnDisable()
	{
		if (quitGameEvent != null)
            quitGameEvent.OnEventRaised -= QuitGame;

		if (cowCanHerd != null)
			cowCanHerd.OnEventRaised -= DisplayHerdInstructions;

		if (playerDiedEvent != null)
			playerDiedEvent.OnEventRaised -= PlayerDied;

		if (cowHerdingComplete != null)
			cowHerdingComplete.OnEventRaised -= CowHerdingComplete;

		if (restartGameEvent != null)
			restartGameEvent.OnEventRaised -= RestartGame;

		if (penOpenEvent != null)
			penOpenEvent.OnEventRaised -= DisplayPadlockWarning;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && pauseUI != null)
		{
			bool showPause = !pauseUI.activeSelf;
			DisplayPauseMenu(showPause);
		}
	}

	public void MuteGame()
	{
		isMuted = !isMuted;
		
		if (isMuted)
		{
			AudioListener.volume = 0;
		} else
		{
			AudioListener.volume = initialVolume;
		}
	}

	private void RestartGame()
	{
		StartGame();
	}

	private void DisplayPadlockWarning(bool isOpen)
	{
		if (padlockWarningUI != null)
			padlockWarningUI.SetActive(isOpen);
	}

	private void ClearAllUI()
	{
		pauseUI.SetActive(false);
		successUI.SetActive(false);
	}

	private void DisplayPauseMenu(bool display)
	{
		pauseUI.SetActive(display);

		if (display)
		{
			Time.timeScale = 0;
			GameSettings.inGame = false;
		}
		else
		{
			Time.timeScale = 1;
			GameSettings.inGame = true;
		}
	}

	private void QuitGame()
	{
        Application.Quit();
	}

	private void DisplayHerdInstructions(bool val)
	{
		herdInstructionUI.SetActive(val);
	}

	private void PlayerDied()
	{
		DisplayPauseMenu(true);
		GameSettings.inGame = false;
	}

	private void CowHerdingComplete(HerdingState herding)
	{
		Time.timeScale = 0;
		GameSettings.inGame = false;
		cowsSavedText.text = herding.cowsSaved.ToString();
		successUI.SetActive(true);
	}
}
