using UnityEngine;

namespace Invector.vCharacterController;

public class vThirdPersonInput : MonoBehaviour
{
	[Header("Controller Input")]
	public string horizontalInput = "Horizontal";

	public string verticallInput = "Vertical";

	public KeyCode jumpInput = KeyCode.Space;

	public KeyCode strafeInput = KeyCode.Tab;

	public KeyCode sprintInput = KeyCode.LeftShift;

	[Header("Camera Input")]
	public string rotateCameraXInput = "Mouse X";

	public string rotateCameraYInput = "Mouse Y";

	[HideInInspector]
	public vThirdPersonController cc;

	[HideInInspector]
	public vThirdPersonCamera tpCamera;

	[HideInInspector]
	public Camera cameraMain;

	protected virtual void Start()
	{
		InitilizeController();
		InitializeTpCamera();
	}

	protected virtual void FixedUpdate()
	{
		cc.UpdateMotor();
		cc.ControlLocomotionType();
		cc.ControlRotationType();
	}

	protected virtual void Update()
	{
		InputHandle();
		cc.UpdateAnimator();
	}

	public virtual void OnAnimatorMove()
	{
		cc.ControlAnimatorRootMotion();
	}

	protected virtual void InitilizeController()
	{
		cc = GetComponent<vThirdPersonController>();
		if (cc != null)
		{
			cc.Init();
		}
	}

	protected virtual void InitializeTpCamera()
	{
		if (tpCamera == null)
		{
			tpCamera = Object.FindObjectOfType<vThirdPersonCamera>();
			if (!(tpCamera == null) && (bool)tpCamera)
			{
				tpCamera.SetMainTarget(base.transform);
				tpCamera.Init();
			}
		}
	}

	protected virtual void InputHandle()
	{
		MoveInput();
		CameraInput();
		SprintInput();
		StrafeInput();
		JumpInput();
	}

	public virtual void MoveInput()
	{
		cc.input.x = Input.GetAxis(horizontalInput);
		cc.input.z = Input.GetAxis(verticallInput);
	}

	protected virtual void CameraInput()
	{
		if (!cameraMain)
		{
			if (!Camera.main)
			{
				Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
			}
			else
			{
				cameraMain = Camera.main;
				cc.rotateTarget = cameraMain.transform;
			}
		}
		if ((bool)cameraMain)
		{
			cc.UpdateMoveDirection(cameraMain.transform);
		}
		if (!(tpCamera == null))
		{
			float axis = Input.GetAxis(rotateCameraYInput);
			float axis2 = Input.GetAxis(rotateCameraXInput);
			tpCamera.RotateCamera(axis2, axis);
		}
	}

	protected virtual void StrafeInput()
	{
		if (Input.GetKeyDown(strafeInput))
		{
			cc.Strafe();
		}
	}

	protected virtual void SprintInput()
	{
		if (Input.GetKeyDown(sprintInput))
		{
			cc.Sprint(value: true);
		}
		else if (Input.GetKeyUp(sprintInput))
		{
			cc.Sprint(value: false);
		}
	}

	protected virtual bool JumpConditions()
	{
		if (cc.isGrounded && cc.GroundAngle() < cc.slopeLimit && !cc.isJumping)
		{
			return !cc.stopMove;
		}
		return false;
	}

	protected virtual void JumpInput()
	{
		if (Input.GetKeyDown(jumpInput) && JumpConditions())
		{
			cc.Jump();
		}
	}
}
