using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private CowManager cowManager;

	[Header("UI")]
	public GameObject pauseUI;
	public GameObject herdInstructionUI;
	public GameObject successUI;
	public TMP_Text cowsSavedText;

	[Header("Events")]
    [SerializeField] private VoidEventSO quitGameEvent = default;
	[SerializeField] private BoolEventSO cowCanHerd = default;
	[SerializeField] private VoidEventSO playerDiedEvent = default;
	[SerializeField] private HerdingEventSO cowHerdingComplete = default;
	[SerializeField] private VoidEventSO startGameEvent = default;

	private void Start()
	{
		cowManager = GetComponent<CowManager>();

		StartGame();
	}

	private void StartGame()
	{
		cowManager.StartGame();

		startGameEvent?.RaiseEvent();
	}

	void OnEnable()
	{
		if (quitGameEvent != null)
		{
			quitGameEvent.OnEventRaised += QuitGame;
		}

		if (cowCanHerd != null)
		{
			cowCanHerd.OnEventRaised += DisplayHerdInstructions;
		}

		if (playerDiedEvent != null)
			playerDiedEvent.OnEventRaised += PlayerDied;

		if (cowHerdingComplete != null)
			cowHerdingComplete.OnEventRaised += CowHerdingComplete;
    }

	private void OnDisable()
	{
		if (quitGameEvent != null)
		{
            quitGameEvent.OnEventRaised -= QuitGame;
		}

		if (cowCanHerd != null)
		{
			cowCanHerd.OnEventRaised -= DisplayHerdInstructions;
		}

		if (playerDiedEvent != null)
			playerDiedEvent.OnEventRaised -= PlayerDied;

		if (cowHerdingComplete != null)
			cowHerdingComplete.OnEventRaised -= CowHerdingComplete;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && pauseUI != null)
		{
			bool showPause = !pauseUI.activeSelf;
			DisplayPauseMenu(showPause);
		}
	}

	private void DisplayPauseMenu(bool display)
	{
		pauseUI.SetActive(display);

		if (display)
		{
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = 1;
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
	}

	private void CowHerdingComplete(HerdingState herding)
	{
		Time.timeScale = 0;
		cowsSavedText.text = herding.cowsSaved.ToString();
		successUI.SetActive(true);
	}
}
