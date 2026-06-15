using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
	public float range = 5f;

	public float yOffset = 1f;

	public float xOffset;

	public GameObject button;

	public GameObject targetGameObject;

	public GameObject targetGameObject2;

	public Camera mainCamera;

	public Camera targetCamera;

	public Button switchButton;

	public Image targetImage;

	public Image targetImage2;

	private bool isTargetCameraActive;

	private bool isRaycastingActive = true;

	private void Start()
	{
		switchButton.onClick.AddListener(SwitchToMainCamera);
	}

	private void Update()
	{
		if (!isRaycastingActive)
		{
			return;
		}
		Vector3 vector = base.transform.position + new Vector3(xOffset, yOffset, 0f);
		Vector3 forward = Vector3.forward;
		Ray ray = new Ray(vector, base.transform.TransformDirection(forward * range));
		Debug.DrawRay(vector, base.transform.TransformDirection(forward * range));
		if (!Physics.Raycast(ray, out var hitInfo, range))
		{
			return;
		}
		button.SetActive(value: false);
		if (hitInfo.collider.tag == "Player")
		{
			MonoBehaviour.print("Player hit");
			button.SetActive(value: true);
			SwitchCamera(targetCamera);
			if (targetGameObject != null)
			{
				targetGameObject.SetActive(value: true);
			}
			if (targetGameObject2 != null)
			{
				targetGameObject2.SetActive(value: true);
			}
			isRaycastingActive = false;
			if (targetImage != null)
			{
				targetImage.gameObject.SetActive(value: false);
			}
			if (targetImage2 != null)
			{
				targetImage2.gameObject.SetActive(value: true);
			}
		}
	}

	private void SwitchCamera(Camera newCamera)
	{
		mainCamera.enabled = false;
		targetCamera.enabled = false;
		newCamera.enabled = true;
		isTargetCameraActive = newCamera == targetCamera;
	}

	private void SwitchToMainCamera()
	{
		SwitchCamera(mainCamera);
		if (targetGameObject != null)
		{
			targetGameObject.SetActive(value: false);
		}
	}
}
