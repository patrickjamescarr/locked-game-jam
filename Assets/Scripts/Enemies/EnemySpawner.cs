using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	private int instanceNumber;
	// private int totalToSpawn = 0;
	// private GameObject[] spawnedEntities;
	private List<GameObject> spawnedEntities = new List<GameObject>();

	public GameObject entityToSpawn;
	public SpawnManagerSO spawnManager;
	public bool spawnOnStart = true;

	private int currentWave = 0;
	public float timeBetweenWaves = 5f;
	private float timeSinceLastWave = 0f;

	[Header("Events")]
	public VoidEventSO restartGameEvent = default;
	[SerializeField] private IntEventSO waveIncreaseEvent = default;

	private void Start()
	{
		StartSpawning();
		InitWaves();
	}

	private void Update()
	{
		if (timeSinceLastWave >= timeBetweenWaves)
		{
			SpawnWave();
			timeSinceLastWave = 0f;
		}
		else
		{
			timeSinceLastWave += Time.deltaTime;
		}
	}

	private void SpawnWave()
	{
		SpawnEnemies(spawnManager.numberOfPrefabsToCreate * (currentWave + 1));
	}

	private void InitWaves()
	{
		currentWave = 0;
		timeSinceLastWave = timeBetweenWaves;
	}

	private void StartSpawning()
	{
		if (spawnOnStart)
			SpawnEnemies(spawnManager.numberOfPrefabsToCreate);
	}

	private void OnEnable()
	{
		if (restartGameEvent != null)
			restartGameEvent.OnEventRaised += ResetGame;

		if (waveIncreaseEvent != null)
			waveIncreaseEvent.OnEventRaised += WaveIncrease;
	}

	private void OnDisable()
	{
		if (restartGameEvent != null)
			restartGameEvent.OnEventRaised -= ResetGame;

		if (waveIncreaseEvent != null)
			waveIncreaseEvent.OnEventRaised -= WaveIncrease;
	}

	public void ResetGame()
	{
		ClearEntities();
		InitWaves();
		StartSpawning();
	}

	private void WaveIncrease(int wave)
	{
		currentWave = wave;
	}

	public void ClearEntities()
	{
		foreach (var entity in spawnedEntities)
		{
			Destroy(entity);
		}

		spawnedEntities.Clear();
	}

	public List<GameObject> Spawn(int numberToSpawn = -1)
	{
		SpawnEnemies(numberToSpawn >= 0 ? numberToSpawn : spawnManager.numberOfPrefabsToCreate);

		return spawnedEntities;
	}

	private void SpawnEnemies(int totalToSpawn)
	{
		int currentSpawnPointIndex = 0;

		for (int i = 0; i < totalToSpawn; i++)
		{
			currentSpawnPointIndex = Random.Range(0, spawnManager.spawnPoints.Length - 1);

			// Creates an instance of the prefab at the current spawn point.
			GameObject currentEntity = Instantiate(entityToSpawn, spawnManager.spawnPoints[currentSpawnPointIndex], Quaternion.identity, this.transform);

			// Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
			currentEntity.name = spawnManager.prefabName + instanceNumber;

			// Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
			// currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnManager.spawnPoints.Length;

			instanceNumber++;

			spawnedEntities.Add(currentEntity);
		}
	}
}
