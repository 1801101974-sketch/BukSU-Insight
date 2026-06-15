using UnityEngine;
using UnityEngine.UI;

public class NPCRotate : MonoBehaviour
{
	public Transform playerTransform;

	public Button rotateButton;

	private void Start()
	{
		rotateButton.onClick.AddListener(RotateTowardsPlayer);
	}

	private void RotateTowardsPlayer()
	{
		if (playerTransform != null)
		{
			Vector3 forward = playerTransform.position - base.transform.position;
			base.transform.rotation = Quaternion.LookRotation(forward);
		}
		else
		{
			Debug.LogError("Player transform not assigned!");
		}
	}
}
