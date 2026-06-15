using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public CharacterController controller;

	public float speed = 5f;

	public float gravity = -9.18f;

	public float jumpHeight = 3f;

	public Transform groundCheck;

	public float groundDistance = 0.4f;

	public LayerMask groundMask;

	private Vector3 velocity;

	private bool isGrounded;

	private void Update()
	{
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
		if (isGrounded && velocity.y < 0f)
		{
			velocity.y = -2f;
		}
		if (Input.GetKey("left shift") && isGrounded)
		{
			speed = 10f;
		}
		else
		{
			speed = 5f;
		}
		float axis = Input.GetAxis("Horizontal");
		float axis2 = Input.GetAxis("Vertical");
		Vector3 vector = base.transform.right * axis + base.transform.forward * axis2;
		controller.Move(vector * speed * Time.deltaTime);
		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}
		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}
