using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CowManager : MonoBehaviour
{
    private List<GameObject> cows;
    private List<GameObject> deadCows;
    private List<GameObject> herdedCows;

    public EnemySpawner spawner;

    public CowEventSO cowDied;
    public CowEventSO cowHerded;

    public void StartGame()
    {
        cows = spawner.Spawn().ToList();
        Reset();
    }

	private void Reset()
	{
        deadCows = new List<GameObject>();
        herdedCows = new List<GameObject>();
	}

	private void OnEnable()
	{
        if (cowDied != null)
            cowDied.OnEventRaised += CowDied;

        if (cowHerded != null)
            cowHerded.OnEventRaised += CowHerded;
	}

    private void OnDisable()
    {
        if (cowDied != null)
            cowDied.OnEventRaised -= CowDied;

        if (cowHerded != null)
            cowHerded.OnEventRaised -= CowHerded;
    }

    private void CowHerded(GameObject cow)
	{
        herdedCows.Add(cow);
        cows.Remove(cow);

        cow.SetActive(false);
	}

	private void CowDied(GameObject cow)
	{
        deadCows.Add(cow);
        cows.Remove(cow);
	}
}
