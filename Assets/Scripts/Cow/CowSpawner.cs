using UnityEngine;

public class CowSpawner : MonoBehaviour
{
	private int instanceNumber;
	private int numberOfPrefabsToCreate = 0;
	private GameObject[] spawnedEntities;

	public string prefabName;
	public GameObject cowPrefab;
	public Vector3[] spawnPoints;

	public void Reset()
	{
		foreach (var entity in spawnedEntities)
		{
			Destroy(entity);
		}
	}

	public GameObject[] Spawn(int numberToSpawn = -1)
	{
		if (numberToSpawn >= 0)
		{
			numberOfPrefabsToCreate = numberToSpawn;
		}

		spawnedEntities = new GameObject[numberOfPrefabsToCreate];

		int currentSpawnPointIndex = 0;

		for (int i = 0; i < numberOfPrefabsToCreate; i++)
		{
			// Creates an instance of the prefab at the current spawn point.
			GameObject currentEntity = Instantiate(cowPrefab, spawnPoints[currentSpawnPointIndex], Quaternion.identity, this.transform);

			// Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
			currentEntity.name = prefabName + instanceNumber;

			// Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
			currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnPoints.Length;

			instanceNumber++;

			spawnedEntities[i] = currentEntity;
		}

		return spawnedEntities;
	}
}
