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
	public BoolEventSO penOpenEvent;
	public VoidEventSO restartGameEvent = default;

	public void StartGame()
	{
		cows = spawner.Spawn(entitiesToSpawn).ToList();
	}

	private void ResetGame()
	{
		cows.Clear();
		cows = new List<GameObject>();
		deadCows = new List<GameObject>();
		herdedCows = new List<GameObject>();
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

		if (cows.Count <= 0)
		{
			cowHerdingComplete?.RaiseEvent(new HerdingState()
			{
				cowsSaved = herdedCows.Count
			});
		}
	}

	private void CowDied(GameObject cow)
	{
		deadCows.Add(cow);
		cows.Remove(cow);

		cow.SetActive(false);
	}
}
