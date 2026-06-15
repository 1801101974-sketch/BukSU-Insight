using UnityEngine;

namespace Invector.vCharacterController;

public class vThirdPersonAnimator : vThirdPersonMotor
{
	public const float walkSpeed = 0.5f;

	public const float runningSpeed = 1f;

	public const float sprintSpeed = 1.5f;

	public virtual void UpdateAnimator()
	{
		if (!(animator == null) && animator.enabled)
		{
			animator.SetBool(vAnimatorParameters.IsStrafing, base.isStrafing);
			animator.SetBool(vAnimatorParameters.IsSprinting, base.isSprinting);
			animator.SetBool(vAnimatorParameters.IsGrounded, base.isGrounded);
			animator.SetFloat(vAnimatorParameters.GroundDistance, groundDistance);
			if (base.isStrafing)
			{
				animator.SetFloat(vAnimatorParameters.InputHorizontal, base.stopMove ? 0f : horizontalSpeed, strafeSpeed.animationSmooth, Time.deltaTime);
				animator.SetFloat(vAnimatorParameters.InputVertical, base.stopMove ? 0f : verticalSpeed, strafeSpeed.animationSmooth, Time.deltaTime);
			}
			else
			{
				animator.SetFloat(vAnimatorParameters.InputVertical, base.stopMove ? 0f : verticalSpeed, freeSpeed.animationSmooth, Time.deltaTime);
			}
			animator.SetFloat(vAnimatorParameters.InputMagnitude, base.stopMove ? 0f : inputMagnitude, base.isStrafing ? strafeSpeed.animationSmooth : freeSpeed.animationSmooth, Time.deltaTime);
		}
	}

	public virtual void SetAnimatorMoveSpeed(vMovementSpeed speed)
	{
		Vector3 vector = base.transform.InverseTransformDirection(moveDirection);
		verticalSpeed = vector.z;
		horizontalSpeed = vector.x;
		Vector2 vector2 = new Vector2(verticalSpeed, horizontalSpeed);
		if (speed.walkByDefault)
		{
			inputMagnitude = Mathf.Clamp(vector2.magnitude, 0f, base.isSprinting ? 1f : 0.5f);
		}
		else
		{
			inputMagnitude = Mathf.Clamp(base.isSprinting ? (vector2.magnitude + 0.5f) : vector2.magnitude, 0f, base.isSprinting ? 1.5f : 1f);
		}
	}
}
