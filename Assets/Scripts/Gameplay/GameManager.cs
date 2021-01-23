using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	private CowManager cowManager;
	private bool isMuted = true;
	private float initialVolume = 30f;
	private List<Image> healthHearts;

	[Header("UI")]
	public GameObject pauseUI;
	public GameObject herdInstructionUI;
	public GameObject gameOverUI;
	public TMP_Text gameOverText;
	public TMP_Text cowsSavedText;
	public GameObject padlockWarningUI;
	public TMP_Text ammoInfoText;
	public TMP_Text savedCowsText;
	public GameObject startGameUI;
	public GameObject healthUI;
	public GameObject heart;

	[Header("Events")]
	[SerializeField] private VoidEventSO quitGameEvent = default;
	[SerializeField] private BoolEventSO cowCanHerd = default;
	[SerializeField] private VoidEventSO playerDiedEvent = default;
	[SerializeField] private HerdingEventSO cowHerdingComplete = default;
	[SerializeField] private HerdingEventSO cowHerdingChanged = default;
	[SerializeField] private VoidEventSO startGameEvent = default;
	[SerializeField] private VoidEventSO restartGameEvent = default;
	[SerializeField] private BoolEventSO penOpenEvent = default;
	[SerializeField] private AmmoEventSO ammoChangedEventChannel = default;
	[SerializeField] private BoolEventSO displayHudEventChannel = default;
	[SerializeField] private PlayerTakeDamageSO playerTakeDamage = default;

	private void Start()
	{
		cowManager = GetComponent<CowManager>();

		initialVolume = AudioListener.volume;
		AudioListener.volume = 0;

		startGameUI.SetActive(true);
	}

	public void StartTheGame()
	{
		StartGame();
	}

	private void StartGame()
	{
		ClearAllUI();
		cowManager.StartGame();
		Time.timeScale = 1;
		startGameEvent?.RaiseEvent();

		ShowHud(true);

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

		if (cowHerdingChanged != null)
			cowHerdingChanged.OnEventRaised += CowHerdingChanged;

		if (restartGameEvent != null)
			restartGameEvent.OnEventRaised += RestartGame;

		if (penOpenEvent != null)
			penOpenEvent.OnEventRaised += DisplayPadlockWarning;

		if (ammoChangedEventChannel != null)
			ammoChangedEventChannel.OnEventRaised += AmmoChanged;

		if (playerTakeDamage != null)
			playerTakeDamage.OnEventRaised += HealthChanged;
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

		if (cowHerdingChanged != null)
			cowHerdingChanged.OnEventRaised -= CowHerdingChanged;

		if (restartGameEvent != null)
			restartGameEvent.OnEventRaised -= RestartGame;

		if (penOpenEvent != null)
			penOpenEvent.OnEventRaised -= DisplayPadlockWarning;

		if (ammoChangedEventChannel != null)
			ammoChangedEventChannel.OnEventRaised -= AmmoChanged;

		if (playerTakeDamage != null)
			playerTakeDamage.OnEventRaised -= HealthChanged;
	}

	private bool gamePaused = false;

	private void Update()
	{
		if ((GameSettings.inGame || gamePaused) && Input.GetKeyDown(KeyCode.Escape) && pauseUI != null)
		{
			var displayMenu = !pauseUI.activeSelf;
			gamePaused = displayMenu;
			DisplayCanvas(pauseUI, displayMenu);
			ShowHud(!displayMenu);
		}
	}

	public void MuteGame()
	{
		isMuted = !isMuted;

		if (isMuted)
		{
			AudioListener.volume = 0;
		}
		else
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
		TryTweenUI(pauseUI);
		TryTweenUI(gameOverUI);
	}

	private void TryTweenUI(GameObject ui)
	{
		var scaleTween = ui.GetComponent<ScaleTween>();

		if (scaleTween != null)
		{
			scaleTween.CloseMe();
		}
		else
		{
			ui.SetActive(false);
		}
	}

	private void DisplayCanvas(GameObject ui, bool display)
	{
		if (!display)
		{
			TryTweenUI(ui);
		}
		else
		{
			ui.SetActive(display);
		}


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
		gameOverText.text = "You died. The zombies ate all the cows. It was awful!";
		DisplayCanvas(gameOverUI, true);
		ShowHud(false);
		GameSettings.inGame = false;
	}

	private void CowHerdingComplete(HerdingState herding)
	{
		Time.timeScale = 0;
		GameSettings.inGame = false;
		cowsSavedText.text = herding.cowsSaved.ToString();
		gameOverText.text = GetHerdingCompleteText(herding);
		DisplayCanvas(gameOverUI, true);
		ShowHud(false);
	}

	private List<string> cowNames = new List<string>() { "Buttercup", "Daisy", "Fat Sam", "The Moo-minator", "Hamburger", "Cowlick", "Ineda Bunn", "Big Mac", "Moscow", "Waffles", "Mickey D.", "Brown Cow" };

	private string GetHerdingCompleteText(HerdingState herding)
	{
		if (herding.cowsDied <= 0)
		{
			return "You saved all the cows. It's a miracle, congratulations.\nYour herd mooing your name through the night in an hommage to you, their saviour.";
		}
		else if (herding.cowsSaved <= 0)
		{
			return "All the cows died.\nDid you even try?";

		}

		return $"You saved {herding.cowsSaved} cows, but {herding.cowsDied} died.\nYou probably look at that as a good result. I'm not sure {cowNames[Random.Range(0, cowNames.Count - 1)]} and her friends would agree.";
	}

	private void CowHerdingChanged(HerdingState herding)
	{
		if (savedCowsText != null)
		{
			savedCowsText.text = $"{herding.cowsSaved}";
		}
	}

	private void ShowHud(bool show)
	{
		displayHudEventChannel.RaiseEvent(show);
	}

	private void AmmoChanged(AmmoInfo ammo)
	{
		if (ammoInfoText != null)
			ammoInfoText.text = $"{ammo.currentBulletsInClip} / {ammo.clipSize}  [{ammo.heldBullets}]";
	}
	private void HealthChanged(float health)
	{
		var heartCount = Mathf.CeilToInt(health / 10);

		if (healthHearts != null)
		{
			foreach (var healthHeart in healthHearts)
			{
				Destroy(healthHeart.gameObject);
			}
		}

		healthHearts = new List<Image>();

		for (int i = 0; i < heartCount; i++)
		{
			GameObject heartInstance = Instantiate(heart, healthUI.transform);
			healthHearts.Add(heartInstance.GetComponent<Image>());
		}
	}
}
