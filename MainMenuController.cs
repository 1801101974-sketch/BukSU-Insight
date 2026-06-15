using UnityEngine;

public class MainMenuController : MonoBehaviour
{
	public ActivationFlag activationFlag;

	public void StartGame()
	{
		activationFlag.shouldActivate = true;
	}

	public void StartGameWithoutActivation()
	{
		activationFlag.shouldActivate = false;
	}
}
