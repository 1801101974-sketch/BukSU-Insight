using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
	public CharacterSettings characterSettings;

	public MainMenu main;

	public GameObject Male;

	public GameObject Female;

	public GameObject Spawn;

	private void Start()
	{
		SpawnCharacter();
	}

	private void SpawnCharacter()
	{
		GameObject gameObject = (characterSettings.isMale ? characterSettings.maleCharacterPrefab : characterSettings.femaleCharacterPrefab);
		if (characterSettings.isMale)
		{
			Male.SetActive(value: true);
			Female.SetActive(value: false);
		}
		else
		{
			Female.SetActive(value: true);
			Male.SetActive(value: false);
		}
		Spawn.SetActive(value: false);
		if (gameObject != null)
		{
			Object.Instantiate(gameObject, base.transform.position, base.transform.rotation);
		}
		else
		{
			Debug.LogError("Character prefab is not assigned!");
		}
	}
}
