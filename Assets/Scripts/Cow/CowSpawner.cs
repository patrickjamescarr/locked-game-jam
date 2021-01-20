using UnityEngine;

public class CowSpawner : MonoBehaviour
{
	private int instanceNumber;
	private int numberOfPrefabsToCreate = 0;

	public string prefabName;
	public GameObject cowPrefab;
	public Vector3[] spawnPoints;
	private int currentSpawnPointIndex = 0;

	public GameObject Spawn(Transform parent)
	{
		// Creates an instance of the prefab at the current spawn point.
		GameObject currentEntity = Instantiate(cowPrefab, spawnPoints[currentSpawnPointIndex], Quaternion.identity, parent);

		// Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
		currentEntity.name = prefabName + instanceNumber;

		// Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
		currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnPoints.Length;

		instanceNumber++;

		return currentEntity;
	}

	public GameObject[] Spawn(int numberToSpawn = -1)
	{
		GameObject[] spawnedEntities;

		if (numberToSpawn >= 0)
		{
			numberOfPrefabsToCreate = numberToSpawn;
		}

		spawnedEntities = new GameObject[numberOfPrefabsToCreate];

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
