using System;
using UnityEngine;

namespace Invector.vCharacterController;

public class vThirdPersonMotor : MonoBehaviour
{
	public enum LocomotionType
	{
		FreeWithStrafe,
		OnlyStrafe,
		OnlyFree
	}

	[Serializable]
	public class vMovementSpeed
	{
		[Range(1f, 20f)]
		public float movementSmooth = 6f;

		[Range(0f, 1f)]
		public float animationSmooth = 0.2f;

		[Tooltip("Rotation speed of the character")]
		public float rotationSpeed = 16f;

		[Tooltip("Character will limit the movement to walk instead of running")]
		public bool walkByDefault;

		[Tooltip("Rotate with the Camera forward when standing idle")]
		public bool rotateWithCamera;

		[Tooltip("Speed to Walk using rigidbody or extra speed if you're using RootMotion")]
		public float walkSpeed = 2f;

		[Tooltip("Speed to Run using rigidbody or extra speed if you're using RootMotion")]
		public float runningSpeed = 4f;

		[Tooltip("Speed to Sprint using rigidbody or extra speed if you're using RootMotion")]
		public float sprintSpeed = 6f;
	}

	[Header("- Movement")]
	[Tooltip("Turn off if you have 'in place' animations and use this values above to move the character, or use with root motion as extra speed")]
	public bool useRootMotion;

	[Tooltip("Use this to rotate the character using the World axis, or false to use the camera axis - CHECK for Isometric Camera")]
	public bool rotateByWorld;

	[Tooltip("Check This to use sprint on press button to your Character run until the stamina finish or movement stops\nIf uncheck your Character will sprint as long as the SprintInput is pressed or the stamina finishes")]
	public bool useContinuousSprint = true;

	[Tooltip("Check this to sprint always in free movement")]
	public bool sprintOnlyFree = true;

	public LocomotionType locomotionType;

	public vMovementSpeed freeSpeed;

	public vMovementSpeed strafeSpeed;

	[Header("- Airborne")]
	[Tooltip("Use the currently Rigidbody Velocity to influence on the Jump Distance")]
	public bool jumpWithRigidbodyForce;

	[Tooltip("Rotate or not while airborne")]
	public bool jumpAndRotate = true;

	[Tooltip("How much time the character will be jumping")]
	public float jumpTimer = 0.3f;

	[Tooltip("Add Extra jump height, if you want to jump only with Root Motion leave the value with 0.")]
	public float jumpHeight = 4f;

	[Tooltip("Speed that the character will move while airborne")]
	public float airSpeed = 5f;

	[Tooltip("Smoothness of the direction while airborne")]
	public float airSmooth = 6f;

	[Tooltip("Apply extra gravity when the character is not grounded")]
	public float extraGravity = -10f;

	[HideInInspector]
	public float limitFallVelocity = -15f;

	[Header("- Ground")]
	[Tooltip("Layers that the character can walk on")]
	public LayerMask groundLayer = 1;

	[Tooltip("Distance to became not grounded")]
	public float groundMinDistance = 0.25f;

	public float groundMaxDistance = 0.5f;

	[Tooltip("Max angle to walk")]
	[Range(30f, 80f)]
	public float slopeLimit = 75f;

	internal Animator animator;

	internal Rigidbody _rigidbody;

	internal PhysicMaterial frictionPhysics;

	internal PhysicMaterial maxFrictionPhysics;

	internal PhysicMaterial slippyPhysics;

	internal CapsuleCollider _capsuleCollider;

	internal bool isJumping;

	internal float inputMagnitude;

	internal float verticalSpeed;

	internal float horizontalSpeed;

	internal float moveSpeed;

	internal float verticalVelocity;

	internal float colliderRadius;

	internal float colliderHeight;

	internal float heightReached;

	internal float jumpCounter;

	internal float groundDistance;

	internal RaycastHit groundHit;

	internal bool lockMovement;

	internal bool lockRotation;

	internal bool _isStrafing;

	internal Transform rotateTarget;

	internal Vector3 input;

	internal Vector3 colliderCenter;

	internal Vector3 inputSmooth;

	internal Vector3 moveDirection;

	internal bool isStrafing
	{
		get
		{
			return _isStrafing;
		}
		set
		{
			_isStrafing = value;
		}
	}

	internal bool isGrounded { get; set; }

	internal bool isSprinting { get; set; }

	public bool stopMove { get; protected set; }

	protected virtual bool jumpFwdCondition
	{
		get
		{
			Vector3 vector = base.transform.position + _capsuleCollider.center + Vector3.up * (0f - _capsuleCollider.height) * 0.5f;
			Vector3 point = vector + Vector3.up * _capsuleCollider.height;
			return Physics.CapsuleCastAll(vector, point, _capsuleCollider.radius * 0.5f, base.transform.forward, 0.6f, groundLayer).Length == 0;
		}
	}

	public void Init()
	{
		animator = GetComponent<Animator>();
		animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
		frictionPhysics = new PhysicMaterial();
		frictionPhysics.name = "frictionPhysics";
		frictionPhysics.staticFriction = 0.25f;
		frictionPhysics.dynamicFriction = 0.25f;
		frictionPhysics.frictionCombine = PhysicMaterialCombine.Multiply;
		maxFrictionPhysics = new PhysicMaterial();
		maxFrictionPhysics.name = "maxFrictionPhysics";
		maxFrictionPhysics.staticFriction = 1f;
		maxFrictionPhysics.dynamicFriction = 1f;
		maxFrictionPhysics.frictionCombine = PhysicMaterialCombine.Maximum;
		slippyPhysics = new PhysicMaterial();
		slippyPhysics.name = "slippyPhysics";
		slippyPhysics.staticFriction = 0f;
		slippyPhysics.dynamicFriction = 0f;
		slippyPhysics.frictionCombine = PhysicMaterialCombine.Minimum;
		_rigidbody = GetComponent<Rigidbody>();
		_capsuleCollider = GetComponent<CapsuleCollider>();
		colliderCenter = GetComponent<CapsuleCollider>().center;
		colliderRadius = GetComponent<CapsuleCollider>().radius;
		colliderHeight = GetComponent<CapsuleCollider>().height;
		isGrounded = true;
	}

	public virtual void UpdateMotor()
	{
		CheckGround();
		CheckSlopeLimit();
		ControlJumpBehaviour();
		AirControl();
	}

	public virtual void SetControllerMoveSpeed(vMovementSpeed speed)
	{
		if (speed.walkByDefault)
		{
			moveSpeed = Mathf.Lerp(moveSpeed, isSprinting ? speed.runningSpeed : speed.walkSpeed, speed.movementSmooth * Time.deltaTime);
		}
		else
		{
			moveSpeed = Mathf.Lerp(moveSpeed, isSprinting ? speed.sprintSpeed : speed.runningSpeed, speed.movementSmooth * Time.deltaTime);
		}
	}

	public virtual void MoveCharacter(Vector3 _direction)
	{
		inputSmooth = Vector3.Lerp(inputSmooth, input, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
		if (isGrounded && !isJumping)
		{
			_direction.y = 0f;
			_direction.x = Mathf.Clamp(_direction.x, -1f, 1f);
			_direction.z = Mathf.Clamp(_direction.z, -1f, 1f);
			if (_direction.magnitude > 1f)
			{
				_direction.Normalize();
			}
			Vector3 velocity = ((useRootMotion ? animator.rootPosition : _rigidbody.position) + _direction * (stopMove ? 0f : moveSpeed) * Time.deltaTime - base.transform.position) / Time.deltaTime;
			if (true)
			{
				velocity.y = _rigidbody.velocity.y;
			}
			_rigidbody.velocity = velocity;
		}
	}

	public virtual void CheckSlopeLimit()
	{
		if ((double)input.sqrMagnitude < 0.1)
		{
			return;
		}
		float num = 0f;
		if (Physics.Linecast(base.transform.position + Vector3.up * (_capsuleCollider.height * 0.5f), base.transform.position + moveDirection.normalized * (_capsuleCollider.radius + 0.2f), out var hitInfo, groundLayer))
		{
			num = Vector3.Angle(Vector3.up, hitInfo.normal);
			Vector3 end = hitInfo.point + moveDirection.normalized * _capsuleCollider.radius;
			if (num > slopeLimit && Physics.Linecast(base.transform.position + Vector3.up * (_capsuleCollider.height * 0.5f), end, out hitInfo, groundLayer))
			{
				num = Vector3.Angle(Vector3.up, hitInfo.normal);
				if (num > slopeLimit && num < 85f)
				{
					stopMove = true;
					return;
				}
			}
		}
		stopMove = false;
	}

	public virtual void RotateToPosition(Vector3 position)
	{
		RotateToDirection((position - base.transform.position).normalized);
	}

	public virtual void RotateToDirection(Vector3 direction)
	{
		RotateToDirection(direction, isStrafing ? strafeSpeed.rotationSpeed : freeSpeed.rotationSpeed);
	}

	public virtual void RotateToDirection(Vector3 direction, float rotationSpeed)
	{
		if (jumpAndRotate || isGrounded)
		{
			direction.y = 0f;
			Quaternion rotation = Quaternion.LookRotation(Vector3.RotateTowards(base.transform.forward, direction.normalized, rotationSpeed * Time.deltaTime, 0.1f));
			base.transform.rotation = rotation;
		}
	}

	protected virtual void ControlJumpBehaviour()
	{
		if (isJumping)
		{
			jumpCounter -= Time.deltaTime;
			if (jumpCounter <= 0f)
			{
				jumpCounter = 0f;
				isJumping = false;
			}
			Vector3 velocity = _rigidbody.velocity;
			velocity.y = jumpHeight;
			_rigidbody.velocity = velocity;
		}
	}

	public virtual void AirControl()
	{
		if (!isGrounded || isJumping)
		{
			if (base.transform.position.y > heightReached)
			{
				heightReached = base.transform.position.y;
			}
			inputSmooth = Vector3.Lerp(inputSmooth, input, airSmooth * Time.deltaTime);
			if (jumpWithRigidbodyForce && !isGrounded)
			{
				_rigidbody.AddForce(moveDirection * airSpeed * Time.deltaTime, ForceMode.VelocityChange);
				return;
			}
			moveDirection.y = 0f;
			moveDirection.x = Mathf.Clamp(moveDirection.x, -1f, 1f);
			moveDirection.z = Mathf.Clamp(moveDirection.z, -1f, 1f);
			Vector3 b = (_rigidbody.position + moveDirection * airSpeed * Time.deltaTime - base.transform.position) / Time.deltaTime;
			b.y = _rigidbody.velocity.y;
			_rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, b, airSmooth * Time.deltaTime);
		}
	}

	protected virtual void CheckGround()
	{
		CheckGroundDistance();
		ControlMaterialPhysics();
		if (groundDistance <= groundMinDistance)
		{
			isGrounded = true;
			if (!isJumping && groundDistance > 0.05f)
			{
				_rigidbody.AddForce(base.transform.up * (extraGravity * 2f * Time.deltaTime), ForceMode.VelocityChange);
			}
			heightReached = base.transform.position.y;
		}
		else if (groundDistance >= groundMaxDistance)
		{
			isGrounded = false;
			verticalVelocity = _rigidbody.velocity.y;
			if (!isJumping)
			{
				_rigidbody.AddForce(base.transform.up * extraGravity * Time.deltaTime, ForceMode.VelocityChange);
			}
		}
		else if (!isJumping)
		{
			_rigidbody.AddForce(base.transform.up * (extraGravity * 2f * Time.deltaTime), ForceMode.VelocityChange);
		}
	}

	protected virtual void ControlMaterialPhysics()
	{
		_capsuleCollider.material = ((isGrounded && GroundAngle() <= slopeLimit + 1f) ? frictionPhysics : slippyPhysics);
		if (isGrounded && input == Vector3.zero)
		{
			_capsuleCollider.material = maxFrictionPhysics;
		}
		else if (isGrounded && input != Vector3.zero)
		{
			_capsuleCollider.material = frictionPhysics;
		}
		else
		{
			_capsuleCollider.material = slippyPhysics;
		}
	}

	protected virtual void CheckGroundDistance()
	{
		if (!(_capsuleCollider != null))
		{
			return;
		}
		float radius = _capsuleCollider.radius * 0.9f;
		float num = 10f;
		if (Physics.Raycast(new Ray(base.transform.position + new Vector3(0f, colliderHeight / 2f, 0f), Vector3.down), out groundHit, colliderHeight / 2f + num, groundLayer) && !groundHit.collider.isTrigger)
		{
			num = base.transform.position.y - groundHit.point.y;
		}
		if (num >= groundMinDistance)
		{
			Vector3 origin = base.transform.position + Vector3.up * _capsuleCollider.radius;
			if (Physics.SphereCast(new Ray(origin, -Vector3.up), radius, out groundHit, _capsuleCollider.radius + groundMaxDistance, groundLayer) && !groundHit.collider.isTrigger)
			{
				Physics.Linecast(groundHit.point + Vector3.up * 0.1f, groundHit.point + Vector3.down * 0.15f, out groundHit, groundLayer);
				float num2 = base.transform.position.y - groundHit.point.y;
				if (num > num2)
				{
					num = num2;
				}
			}
		}
		groundDistance = (float)Math.Round(num, 2);
	}

	public virtual float GroundAngle()
	{
		return Vector3.Angle(groundHit.normal, Vector3.up);
	}

	public virtual float GroundAngleFromDirection()
	{
		return Vector3.Angle((isStrafing && input.magnitude > 0f) ? (base.transform.right * input.x + base.transform.forward * input.z).normalized : base.transform.forward, groundHit.normal) - 90f;
	}
}
