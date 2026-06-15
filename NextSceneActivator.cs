using UnityEngine;

public class NextSceneActivator : MonoBehaviour
{
	public ActivationFlag activationFlag;

	public GameObject objectToActivate;

	private void Start()
	{
		objectToActivate.SetActive(activationFlag.shouldActivate);
	}

	public void EndActive()
	{
		activationFlag.shouldActivate = false;
	}
}
