using UnityEngine;

public class GameManager : MonoBehaviour
{
	private CowManager cowManager;

	[Header("UI")]
	public GameObject pauseUI;
	public GameObject herdInstructionUI;

	[Header("Events")]
    [SerializeField] private VoidEventSO quitGameEvent = default;
	[SerializeField] private BoolEventSO cowCanHerd = default;

	public void StartGame()
	{
		cowManager = GetComponent<CowManager>();
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
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && pauseUI != null)
		{
			bool newPause = !pauseUI.activeSelf;
			pauseUI.SetActive(newPause);

			if (newPause)
			{
				Time.timeScale = 0;
			} else
			{
				Time.timeScale = 1;
			}
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
}
