using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject pauseUI;

    [SerializeField] private VoidEventSO quitGameEvent = default;

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
