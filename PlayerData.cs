using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
	public string playerName;

	public Vector3 lastPosition;

	public Vector3 customPosition;
}
