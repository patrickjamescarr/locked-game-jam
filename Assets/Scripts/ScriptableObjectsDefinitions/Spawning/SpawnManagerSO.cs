using UnityEngine;

[CreateAssetMenu(fileName = "SpawnManager", menuName = "ScriptableObjects/SpawnManager", order = 1)]
public class SpawnManagerSO : ScriptableObject
{
	public string prefabName;

	public int numberOfPrefabsToCreate;
	public Vector3[] spawnPoints;
}
