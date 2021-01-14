﻿using Pathfinding;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject entityToSpawn;
    public SpawnManagerSO spawnManager;
    int instanceNumber;

    public Transform target;

    private void Start()
	{
		SpawnEnemies();
	}

	private void SpawnEnemies()
	{
        int currentSpawnPointIndex = 0;

        for (int i = 0; i < spawnManager.numberOfPrefabsToCreate; i++)
        {
            // Creates an instance of the prefab at the current spawn point.
            GameObject currentEntity = Instantiate(entityToSpawn, spawnManager.spawnPoints[currentSpawnPointIndex], Quaternion.identity, this.transform);

            // Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
            currentEntity.name = spawnManager.prefabName + instanceNumber;

            // Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
            currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnManager.spawnPoints.Length;

            if (target != null)
			{
                var destinationSetter = currentEntity.GetComponent<AIDestinationSetter>();
                if (destinationSetter != null)
				{
                    destinationSetter.target = target;
				}
			}

            instanceNumber++;
        }
    }
}