using UnityEngine;

public class FaceNPCController : MonoBehaviour
{
	public Transform player;

	public Transform npc;

	public void MakeThemLookAtEachOther()
	{
		if (!(player == null) && !(npc == null))
		{
			Vector3 forward = npc.position - player.position;
			forward.y = 0f;
			player.rotation = Quaternion.LookRotation(forward);
			Vector3 forward2 = player.position - npc.position;
			forward2.y = 0f;
			npc.rotation = Quaternion.LookRotation(forward2);
		}
	}
}
