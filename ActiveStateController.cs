using UnityEngine;

public class ActiveStateController : MonoBehaviour
{
	[Tooltip("Reference to the ScriptableObject that holds the active state")]
	public ActiveStateSO activeState;

	public GameObject Open;

	private void Start()
	{
		base.gameObject.SetActive(activeState.isActive);
		SpawnCharacter();
	}

	private void SpawnCharacter()
	{
		if (activeState.isActive)
		{
			Open.SetActive(value: true);
		}
		else
		{
			Open.SetActive(value: false);
		}
	}

	public void SetActiveState(bool value)
	{
		activeState.isActive = value;
		base.gameObject.SetActive(value);
	}
}
