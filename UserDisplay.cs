using UnityEngine;
using UnityEngine.UI;

public class UserDisplay : MonoBehaviour
{
	public PlayerData data;

	public Text nameText;

	private void Start()
	{
		nameText.text = data.playerName;
	}
}
