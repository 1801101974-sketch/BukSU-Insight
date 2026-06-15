using UnityEngine;

[CreateAssetMenu(fileName = "SpawnData", menuName = "Game/Spawn Data")]
public class SpawnData : ScriptableObject
{
	[Tooltip("The prefab to spawn in the next scene")]
	public GameObject prefabToSpawn;

	[Tooltip("Position to spawn the object at in the next scene")]
	public Vector3 spawnPosition = Vector3.zero;

	public bool clearAfterSpawn = true;
}
