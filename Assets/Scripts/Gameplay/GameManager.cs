using UnityEngine;

public class GameManager : MonoBehaviour
{
	private CowManager cowManager;

	public GameObject pauseUI;

    [SerializeField] private VoidEventSO quitGameEvent = default;

	public void StartGame()
	{
		Debug.Log("Start Game");
	}

    void OnEnable()
    {
        if (quitGameEvent != null)
		{
            quitGameEvent.OnEventRaised += QuitGame;
		}
    }

	private void OnDisable()
	{
		if (quitGameEvent != null)
		{
            quitGameEvent.OnEventRaised -= QuitGame;
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
}
