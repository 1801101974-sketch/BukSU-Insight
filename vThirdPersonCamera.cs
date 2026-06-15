using Invector;
using UnityEngine;

public class vThirdPersonCamera : MonoBehaviour
{
	public Transform target;

	[Tooltip("Lerp speed between Camera States")]
	public float smoothCameraRotation = 12f;

	[Tooltip("What layer will be culled")]
	public LayerMask cullingLayer = 1;

	[Tooltip("Debug purposes, lock the camera behind the character for better align the states")]
	public bool lockCamera;

	public float rightOffset;

	public float defaultDistance = 2.5f;

	public float height = 1.4f;

	public float smoothFollow = 10f;

	public float xMouseSensitivity = 3f;

	public float yMouseSensitivity = 3f;

	public float yMinLimit = -40f;

	public float yMaxLimit = 80f;

	[HideInInspector]
	public int indexList;

	[HideInInspector]
	public int indexLookPoint;

	[HideInInspector]
	public float offSetPlayerPivot;

	[HideInInspector]
	public string currentStateName;

	[HideInInspector]
	public Transform currentTarget;

	[HideInInspector]
	public Vector2 movementSpeed;

	private Transform targetLookAt;

	private Vector3 currentTargetPos;

	private Vector3 lookPoint;

	private Vector3 current_cPos;

	private Vector3 desired_cPos;

	private Camera _camera;

	private float distance = 5f;

	private float mouseY;

	private float mouseX;

	private float currentHeight;

	private float cullingDistance;

	private float checkHeightRadius = 0.4f;

	private float clipPlaneMargin;

	private float forward = -1f;

	private float xMinLimit = -360f;

	private float xMaxLimit = 360f;

	private float cullingHeight = 0.2f;

	private float cullingMinDist = 0.1f;

	private void Start()
	{
		Init();
	}

	public void Init()
	{
		if (!(target == null))
		{
			_camera = GetComponent<Camera>();
			currentTarget = target;
			currentTargetPos = new Vector3(currentTarget.position.x, currentTarget.position.y + offSetPlayerPivot, currentTarget.position.z);
			targetLookAt = new GameObject("targetLookAt").transform;
			targetLookAt.position = currentTarget.position;
			targetLookAt.hideFlags = HideFlags.HideInHierarchy;
			targetLookAt.rotation = currentTarget.rotation;
			mouseY = currentTarget.eulerAngles.x;
			mouseX = currentTarget.eulerAngles.y;
			distance = defaultDistance;
			currentHeight = height;
		}
	}

	private void FixedUpdate()
	{
		if (!(target == null) && !(targetLookAt == null))
		{
			CameraMovement();
		}
	}

	public void SetTarget(Transform newTarget)
	{
		currentTarget = (newTarget ? newTarget : target);
	}

	public void SetMainTarget(Transform newTarget)
	{
		target = newTarget;
		currentTarget = newTarget;
		mouseY = currentTarget.rotation.eulerAngles.x;
		mouseX = currentTarget.rotation.eulerAngles.y;
		Init();
	}

	public Ray ScreenPointToRay(Vector3 Point)
	{
		return GetComponent<Camera>().ScreenPointToRay(Point);
	}

	public void RotateCamera(float x, float y)
	{
		mouseX += x * xMouseSensitivity;
		mouseY -= y * yMouseSensitivity;
		movementSpeed.x = x;
		movementSpeed.y = 0f - y;
		if (!lockCamera)
		{
			mouseY = vExtensions.ClampAngle(mouseY, yMinLimit, yMaxLimit);
			mouseX = vExtensions.ClampAngle(mouseX, xMinLimit, xMaxLimit);
		}
		else
		{
			mouseY = currentTarget.root.localEulerAngles.x;
			mouseX = currentTarget.root.localEulerAngles.y;
		}
	}

	private void CameraMovement()
	{
		if (currentTarget == null)
		{
			return;
		}
		distance = Mathf.Lerp(distance, defaultDistance, smoothFollow * Time.deltaTime);
		cullingDistance = Mathf.Lerp(cullingDistance, distance, Time.deltaTime);
		Vector3 normalized = (forward * targetLookAt.forward + rightOffset * targetLookAt.right).normalized;
		Vector3 vector = (currentTargetPos = new Vector3(currentTarget.position.x, currentTarget.position.y + offSetPlayerPivot, currentTarget.position.z));
		desired_cPos = vector + new Vector3(0f, height, 0f);
		current_cPos = currentTargetPos + new Vector3(0f, currentHeight, 0f);
		ClipPlanePoints to = _camera.NearClipPlanePoints(current_cPos + normalized * distance, clipPlaneMargin);
		ClipPlanePoints to2 = _camera.NearClipPlanePoints(desired_cPos + normalized * distance, clipPlaneMargin);
		if (Physics.SphereCast(vector, checkHeightRadius, Vector3.up, out var hitInfo, cullingHeight + 0.2f, cullingLayer))
		{
			float num = hitInfo.distance - 0.2f;
			num -= height;
			num /= cullingHeight - height;
			cullingHeight = Mathf.Lerp(height, cullingHeight, Mathf.Clamp(num, 0f, 1f));
		}
		if (CullingRayCast(desired_cPos, to2, out hitInfo, distance + 0.2f, cullingLayer, Color.blue))
		{
			distance = hitInfo.distance - 0.2f;
			if (distance < defaultDistance)
			{
				float num2 = hitInfo.distance;
				num2 -= cullingMinDist;
				num2 /= cullingMinDist;
				currentHeight = Mathf.Lerp(cullingHeight, height, Mathf.Clamp(num2, 0f, 1f));
				current_cPos = currentTargetPos + new Vector3(0f, currentHeight, 0f);
			}
		}
		else
		{
			currentHeight = height;
		}
		if (CullingRayCast(current_cPos, to, out hitInfo, distance, cullingLayer, Color.cyan))
		{
			distance = Mathf.Clamp(cullingDistance, 0f, defaultDistance);
		}
		Vector3 vector2 = current_cPos + targetLookAt.forward * 2f + targetLookAt.right * Vector3.Dot(normalized * distance, targetLookAt.right);
		targetLookAt.position = current_cPos;
		Quaternion b = Quaternion.Euler(mouseY, mouseX, 0f);
		targetLookAt.rotation = Quaternion.Slerp(targetLookAt.rotation, b, smoothCameraRotation * Time.deltaTime);
		base.transform.position = current_cPos + normalized * distance;
		Quaternion rotation = Quaternion.LookRotation(vector2 - base.transform.position);
		base.transform.rotation = rotation;
		movementSpeed = Vector2.zero;
	}

	private bool CullingRayCast(Vector3 from, ClipPlanePoints _to, out RaycastHit hitInfo, float distance, LayerMask cullingLayer, Color color)
	{
		bool flag = false;
		if (Physics.Raycast(from, _to.LowerLeft - from, out hitInfo, distance, cullingLayer))
		{
			flag = true;
			cullingDistance = hitInfo.distance;
		}
		if (Physics.Raycast(from, _to.LowerRight - from, out hitInfo, distance, cullingLayer))
		{
			flag = true;
			if (cullingDistance > hitInfo.distance)
			{
				cullingDistance = hitInfo.distance;
			}
		}
		if (Physics.Raycast(from, _to.UpperLeft - from, out hitInfo, distance, cullingLayer))
		{
			flag = true;
			if (cullingDistance > hitInfo.distance)
			{
				cullingDistance = hitInfo.distance;
			}
		}
		if (Physics.Raycast(from, _to.UpperRight - from, out hitInfo, distance, cullingLayer))
		{
			flag = true;
			if (cullingDistance > hitInfo.distance)
			{
				cullingDistance = hitInfo.distance;
			}
		}
		return (bool)hitInfo.collider && flag;
	}
}
