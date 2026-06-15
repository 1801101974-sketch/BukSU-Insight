using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	[SerializeField]
	private float m_MovementSpeed = 1f;

	[SerializeField]
	private Transform m_CameraTransform;

	private CharacterController m_CharacterController;

	private void Start()
	{
		m_CharacterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		Vector3 normalized = (base.transform.position - m_CameraTransform.position).normalized;
		normalized.y = 0f;
		Vector3 normalized2 = Vector3.Cross(Vector3.up, normalized).normalized;
		float num = m_MovementSpeed * Input.GetAxis("Horizontal");
		float num2 = m_MovementSpeed * Input.GetAxis("Vertical");
		Vector3 motion = normalized * num2 + normalized2 * num;
		m_CharacterController.Move(motion);
	}
}
