using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
	public GameObject player;

	private void Update()
	{
		if (!(player == null))
		{
			base.transform.position = new Vector3(player.transform.position.x, base.transform.position.y, player.transform.position.z);
		}
	}
}
