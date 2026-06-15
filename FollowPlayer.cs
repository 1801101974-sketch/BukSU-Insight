using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	public string playerTag = "Player";

	public float horizontalOffset;

	public float verticalOffset;

	public float height = 10f;

	private void LateUpdate()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag(playerTag);
		if (gameObject != null)
		{
			base.transform.position = new Vector3(gameObject.transform.position.x, height, gameObject.transform.position.z);
			base.transform.LookAt(gameObject.transform.position + new Vector3(horizontalOffset, verticalOffset, 0f));
		}
	}
}
