﻿using Pathfinding;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	private int instanceNumber;
	private int totalToSpawn = 0;
	private GameObject[] spawnedEntities;

	public GameObject entityToSpawn;
	public SpawnManagerSO spawnManager;
	public bool spawnOnStart = true;

	private void Start()
	{
		totalToSpawn = spawnManager.numberOfPrefabsToCreate;

		if (spawnOnStart)
			SpawnEnemies();
	}

	public GameObject[] Spawn(int numberToSpawn = -1)
	{
		if (numberToSpawn >= 0)
		{
			totalToSpawn = numberToSpawn;
		}

		SpawnEnemies();

		return spawnedEntities;
	}

	private void SpawnEnemies()
	{
		spawnedEntities = new GameObject[totalToSpawn];

		int currentSpawnPointIndex = 0;

		for (int i = 0; i < totalToSpawn; i++)
		{
			// Creates an instance of the prefab at the current spawn point.
			GameObject currentEntity = Instantiate(entityToSpawn, spawnManager.spawnPoints[currentSpawnPointIndex], Quaternion.identity, this.transform);

			// Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
			currentEntity.name = spawnManager.prefabName + instanceNumber;

			// Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
			currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnManager.spawnPoints.Length;

			instanceNumber++;

			spawnedEntities[i] = currentEntity;
		}
	}
}
