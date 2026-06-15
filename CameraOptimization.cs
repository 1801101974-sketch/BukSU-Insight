using UnityEngine;

public class CameraOptimization : MonoBehaviour
{
	[Tooltip("The far clipping plane distance.")]
	public float farClipDistance = 50f;

	[Tooltip("The layer mask to exclude from rendering.")]
	public LayerMask cullingMaskExclude;

	private Camera cam;

	private void Start()
	{
		if (cam == null)
		{
			Debug.LogError("CameraOptimization script needs to be attached to a Camera.");
			base.enabled = false;
		}
		else
		{
			cam.nearClipPlane = 0.1f;
			cam.farClipPlane = farClipDistance;
			cam.cullingMask &= ~(int)cullingMaskExclude;
		}
	}
}
