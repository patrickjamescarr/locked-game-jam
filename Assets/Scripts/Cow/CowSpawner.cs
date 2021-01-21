using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CowSpawner : MonoBehaviour
{
	private int instanceNumber;
	private int numberOfPrefabsToCreate = 0;

	public string prefabName;
	public GameObject cowPrefab;
	public Tilemap ground;

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
			GameObject currentEntity = Instantiate(cowPrefab, GetSpawnSpot(), Quaternion.identity, this.transform);

			// Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
			currentEntity.name = prefabName + instanceNumber;

			instanceNumber++;

			spawnedEntities[i] = currentEntity;
		}

		return spawnedEntities;
	}

	private Vector3 GetSpawnSpot()
	{
		while (true)
		{
			int xVal = Random.Range(ground.origin.x + 1, ground.origin.x + ground.size.x - 1);
			int yVal = Random.Range(ground.origin.y + 1, ground.origin.y + ground.size.y - 1);

			Vector3 pos = new Vector3(xVal, yVal);

			GraphNode node = AstarPath.active.GetNearest(pos).node;
			if (node.Walkable)
			{
				return (Vector3)node.position;
			}
		}
	}
}
