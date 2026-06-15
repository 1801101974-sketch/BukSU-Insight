using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CursorLookController : MonoBehaviour
{
	[Header("Camera Setup")]
	public Transform cameraTransform;

	public Transform playerBody;

	public float sensitivity = 0.15f;

	[Header("Look Input Action")]
	public InputActionReference lookAction;

	public InputActionReference clickAction;

	[Header("Movement Input Actions")]
	public InputActionReference moveAction;

	public InputActionReference jumpAction;

	public InputActionReference sprintAction;

	private float xRotation;

	private void Start()
	{
		LockCursor();
	}

	private void Update()
	{
		HandleCursorState();
		HandleCameraLook();
	}

	private void HandleCursorState()
	{
		if (Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			UnlockCursor();
		}
		if (clickAction.action.WasPerformedThisFrame())
		{
			if (IsPointerOverUI())
			{
				UnlockCursor();
			}
			else
			{
				LockCursor();
			}
		}
		Cursor.visible = Cursor.lockState != CursorLockMode.Locked;
	}

	private void HandleCameraLook()
	{
		if (Cursor.lockState == CursorLockMode.Locked && lookAction.action.enabled)
		{
			Vector2 vector = lookAction.action.ReadValue<Vector2>() * sensitivity;
			float x = vector.x;
			float y = vector.y;
			xRotation -= y;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);
			cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
			playerBody.Rotate(Vector3.up * x);
		}
	}

	private void LockCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		lookAction.action.Enable();
		moveAction?.action.Enable();
		jumpAction?.action.Enable();
		sprintAction?.action.Enable();
	}

	private void UnlockCursor()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		lookAction.action.Disable();
		moveAction?.action.Disable();
		jumpAction?.action.Disable();
		sprintAction?.action.Disable();
	}

	private bool IsPointerOverUI()
	{
		if (EventSystem.current == null)
		{
			return false;
		}
		PointerEventData eventData = new PointerEventData(EventSystem.current)
		{
			position = Mouse.current.position.ReadValue()
		};
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, list);
		return list.Count > 0;
	}
}
