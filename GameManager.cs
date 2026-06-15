using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public DialogManager dialogManager;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}
}
