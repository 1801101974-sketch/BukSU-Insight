using UnityEngine;

namespace Invector.vCharacterController;

public class vThirdPersonController : vThirdPersonAnimator
{
	public virtual void ControlAnimatorRootMotion()
	{
		if (base.enabled)
		{
			if (inputSmooth == Vector3.zero)
			{
				base.transform.position = animator.rootPosition;
				base.transform.rotation = animator.rootRotation;
			}
			if (useRootMotion)
			{
				MoveCharacter(moveDirection);
			}
		}
	}

	public virtual void ControlLocomotionType()
	{
		if (!lockMovement)
		{
			if ((locomotionType.Equals(LocomotionType.FreeWithStrafe) && !base.isStrafing) || locomotionType.Equals(LocomotionType.OnlyFree))
			{
				SetControllerMoveSpeed(freeSpeed);
				SetAnimatorMoveSpeed(freeSpeed);
			}
			else if (locomotionType.Equals(LocomotionType.OnlyStrafe) || (locomotionType.Equals(LocomotionType.FreeWithStrafe) && base.isStrafing))
			{
				base.isStrafing = true;
				SetControllerMoveSpeed(strafeSpeed);
				SetAnimatorMoveSpeed(strafeSpeed);
			}
			if (!useRootMotion)
			{
				MoveCharacter(moveDirection);
			}
		}
	}

	public virtual void ControlRotationType()
	{
		if (!lockRotation && (input != Vector3.zero || (base.isStrafing ? strafeSpeed.rotateWithCamera : freeSpeed.rotateWithCamera)))
		{
			inputSmooth = Vector3.Lerp(inputSmooth, input, (base.isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
			Vector3 direction = ((((base.isStrafing && (!base.isSprinting || !sprintOnlyFree)) || (freeSpeed.rotateWithCamera && input == Vector3.zero)) && (bool)rotateTarget) ? rotateTarget.forward : moveDirection);
			RotateToDirection(direction);
		}
	}

	public virtual void UpdateMoveDirection(Transform referenceTransform = null)
	{
		if ((double)input.magnitude <= 0.01)
		{
			moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, (base.isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
		}
		else if ((bool)referenceTransform && !rotateByWorld)
		{
			Vector3 right = referenceTransform.right;
			right.y = 0f;
			Vector3 vector = Quaternion.AngleAxis(-90f, Vector3.up) * right;
			moveDirection = inputSmooth.x * right + inputSmooth.z * vector;
		}
		else
		{
			moveDirection = new Vector3(inputSmooth.x, 0f, inputSmooth.z);
		}
	}

	public virtual void Sprint(bool value)
	{
		bool flag = input.sqrMagnitude > 0.1f && base.isGrounded && (!base.isStrafing || strafeSpeed.walkByDefault || (!((double)horizontalSpeed >= 0.5) && !((double)horizontalSpeed <= -0.5) && !(verticalSpeed <= 0.1f)));
		if (value && flag)
		{
			if (input.sqrMagnitude > 0.1f)
			{
				if (base.isGrounded && useContinuousSprint)
				{
					base.isSprinting = !base.isSprinting;
				}
				else if (!base.isSprinting)
				{
					base.isSprinting = true;
				}
			}
			else if (!useContinuousSprint && base.isSprinting)
			{
				base.isSprinting = false;
			}
		}
		else if (base.isSprinting)
		{
			base.isSprinting = false;
		}
	}

	public virtual void Strafe()
	{
		base.isStrafing = !base.isStrafing;
	}

	public virtual void Jump()
	{
		jumpCounter = jumpTimer;
		isJumping = true;
		if (input.sqrMagnitude < 0.1f)
		{
			animator.CrossFadeInFixedTime("Jump", 0.1f);
		}
		else
		{
			animator.CrossFadeInFixedTime("JumpMove", 0.2f);
		}
	}
}
