using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSettings", menuName = "ScriptableObjects/CharacterSettings", order = 1)]
public class CharacterSettings : ScriptableObject
{
	public bool isMale;

	public GameObject maleCharacterPrefab;

	public GameObject femaleCharacterPrefab;
}
