using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CowManager : MonoBehaviour
{
	private List<GameObject> cows = new List<GameObject>();
	private List<GameObject> deadCows = new List<GameObject>();
	private List<GameObject> herdedCows = new List<GameObject>();
	private bool penOpen = false;
	private float timeSinceLastEscape = 0f;

	[Header("Stats")]
	public int entitiesToSpawn = 5;
	public float timeBetweenCowEscapes = 5f;

	[Header("Misc")]
	public CowSpawner spawner;

	[Header("Events")]
	public CowEventSO cowDied;
	public CowEventSO cowHerded;
	public HerdingEventSO cowHerdingComplete;
	public HerdingEventSO cowHerdingChanged;
	public BoolEventSO penOpenEvent;
	public VoidEventSO restartGameEvent = default;

	public void StartGame()
	{
		cows = spawner.Spawn(entitiesToSpawn).ToList();
		UpdateCowHerding();
	}

	private void ClearAllCows()
	{
		foreach (var cow in deadCows)
			Destroy(cow);

		foreach (var cow in cows)
			Destroy(cow);

		foreach (var cow in herdedCows)
			Destroy(cow);
	}

	private void ResetGame()
	{
		ClearAllCows();

		cows = new List<GameObject>();
		deadCows = new List<GameObject>();
		herdedCows = new List<GameObject>();

		StartGame();
	}

	private void OnEnable()
	{
		if (cowDied != null)
			cowDied.OnEventRaised += CowDied;

		if (cowHerded != null)
			cowHerded.OnEventRaised += CowHerded;

		if (penOpenEvent != null)
			penOpenEvent.OnEventRaised += PenOpened;

		if (restartGameEvent != null)
			restartGameEvent.OnEventRaised += ResetGame;
	}

	private void OnDisable()
	{
		if (cowDied != null)
			cowDied.OnEventRaised -= CowDied;

		if (cowHerded != null)
			cowHerded.OnEventRaised -= CowHerded;

		if (penOpenEvent != null)
			penOpenEvent.OnEventRaised -= PenOpened;

		if (restartGameEvent != null)
			restartGameEvent.OnEventRaised += ResetGame;
	}

	private void Update()
	{
		if (penOpen && herdedCows.Count > 0)
		{
			timeSinceLastEscape += Time.deltaTime;

			if (timeSinceLastEscape > timeBetweenCowEscapes)
			{
				ReleaseCow();
			}
		}
	}

	private void ReleaseCow()
	{
		timeSinceLastEscape = 0;

		var cow = herdedCows.FirstOrDefault();
		cow.SetActive(true);
		herdedCows.Remove(cow);
		cows.Add(cow);

		UpdateCowHerding();
	}

	private void PenOpened(bool open)
	{
		penOpen = open;
		timeSinceLastEscape = timeBetweenCowEscapes;
	}

	private void CowHerded(GameObject cow)
	{
		herdedCows.Add(cow);
		cows.Remove(cow);

		cow.SetActive(false);

		CheckCowHerdingComplete();
	}

	private void CheckCowHerdingComplete()
	{
		if (cows.Count <= 0)
		{
			cowHerdingComplete?.RaiseEvent(new HerdingState()
			{
				cowsSaved = herdedCows.Count
			});
		} else
		{
			UpdateCowHerding();
		}
	}

	private void UpdateCowHerding()
	{
		cowHerdingChanged?.RaiseEvent(new HerdingState()
		{
			looseCows = cows.Count
		});
	}

	private void CowDied(GameObject cow)
	{
		deadCows.Add(cow);
		cows.Remove(cow);

		cow.SetActive(false);

		CheckCowHerdingComplete();
	}
}
