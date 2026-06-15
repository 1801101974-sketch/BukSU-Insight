using UnityEngine;
using UnityEngine.SceneManagement;

public class LIbrary : MonoBehaviour
{
	public string nextSceneName;

	public GameObject objectToDestroy;

	public GameObject Enter;

	public PlayerLocation playerLocation;

	public Vector3 customSavePosition;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Enter.SetActive(value: true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Enter.SetActive(value: false);
		}
	}

	public void EnterLibrary()
	{
		SceneManager.LoadScene(nextSceneName);
		Object.Destroy(objectToDestroy, 3f);
		playerLocation.SaveCustomPosition(customSavePosition);
	}
}
