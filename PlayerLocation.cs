using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
	public PlayerData playerData;

	public void Start()
	{
		LoadPlayerPosition();
	}

	public void OnApplicationQuit()
	{
		SavePlayerPosition();
	}

	public void SavePlayerPosition()
	{
		playerData.lastPosition = base.transform.position;
	}

	public void LoadPlayerPosition()
	{
		if (playerData != null)
		{
			base.transform.position = playerData.lastPosition;
		}
	}

	public void SaveCustomPosition(Vector3 customPosition)
	{
		playerData.lastPosition = customPosition;
	}

	public void LoadCustomPosition()
	{
		if (playerData != null)
		{
			base.transform.position = playerData.customPosition;
		}
	}
}
