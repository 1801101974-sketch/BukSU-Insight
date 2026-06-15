using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneSpawner : MonoBehaviour
{
	[Tooltip("Reference to the SpawnData ScriptableObject")]
	public SpawnData spawnData;

	[Tooltip("Prefab to spawn in the next scene")]
	public GameObject prefabToSpawn;

	[Tooltip("Name of the next scene to load")]
	public string nextSceneName;

	[Tooltip("Spawn position in the next scene")]
	public Vector3 spawnPosition = Vector3.zero;

	public void OnNextSceneButton()
	{
		spawnData.prefabToSpawn = prefabToSpawn;
		spawnData.spawnPosition = spawnPosition;
		SceneManager.LoadScene(nextSceneName);
	}
}
