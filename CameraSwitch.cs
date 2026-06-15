using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
	public Camera mainCamera;

	public Camera targetCamera;

	public KeyCode switchKeyCode;

	private bool isTargetCameraActive;

	private void Update()
	{
		if (!isTargetCameraActive)
		{
			if (Physics.Raycast(new Ray(mainCamera.transform.position, mainCamera.transform.forward), out var hitInfo) && hitInfo.collider.CompareTag("Target"))
			{
				SwitchCamera(targetCamera);
			}
		}
		else if (Input.GetKeyDown(switchKeyCode))
		{
			SwitchCamera(mainCamera);
		}
	}

	private void SwitchCamera(Camera newCamera)
	{
		mainCamera.enabled = false;
		targetCamera.enabled = false;
		newCamera.enabled = true;
		isTargetCameraActive = newCamera == targetCamera;
	}
}
