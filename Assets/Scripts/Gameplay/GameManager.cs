using TMPro;
using UnityEngine;

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
	public GameObject playerDiedUI;
	public TMP_Text ammoInfoText;
	public TMP_Text savedCowsText;
	public GameObject startGameUI;

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

		if (cowHerdingChanged!= null)
			cowHerdingChanged.OnEventRaised += CowHerdingChanged;

		if (restartGameEvent != null)
			restartGameEvent.OnEventRaised += RestartGame;

		if (penOpenEvent != null)
			penOpenEvent.OnEventRaised += DisplayPadlockWarning;

		if (ammoChangedEventChannel != null)
			ammoChangedEventChannel.OnEventRaised += AmmoChanged;
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
	}

	private void Update()
	{
		if (GameSettings.inGame && Input.GetKeyDown(KeyCode.Escape) && pauseUI != null)
		{
			var displayMenu = !pauseUI.activeSelf;
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
		playerDiedUI.SetActive(false);
	}

	private void DisplayCanvas(GameObject ui, bool display)
	{
		ui.SetActive(display);

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
		DisplayCanvas(playerDiedUI, true);
		ShowHud(false);
		GameSettings.inGame = false;
	}

	private void CowHerdingComplete(HerdingState herding)
	{
		Time.timeScale = 0;
		GameSettings.inGame = false;
		cowsSavedText.text = herding.cowsSaved.ToString();
		successUI.SetActive(true);
		ShowHud(false);
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
}
