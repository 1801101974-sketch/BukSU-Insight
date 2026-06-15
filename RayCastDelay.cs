using UnityEngine;

public class RayCastDelay : MonoBehaviour
{
	public float range = 5f;

	public float yOffset = 1f;

	public float xOffset;

	public float activationTime = 2f;

	public GameObject buttonObject;

	private bool isHit;

	private float hitTimer;

	private void Update()
	{
		Vector3 vector = base.transform.position + new Vector3(xOffset, yOffset, 0f);
		Vector3 forward = Vector3.forward;
		Ray ray = new Ray(vector, base.transform.TransformDirection(forward * range));
		Debug.DrawRay(vector, base.transform.TransformDirection(forward * range));
		if (Physics.Raycast(ray, out var hitInfo, range))
		{
			if (hitInfo.collider.tag == "CameraCube")
			{
				isHit = true;
				hitTimer += Time.deltaTime;
				if (isHit && hitTimer >= activationTime)
				{
					buttonObject.SetActive(value: true);
				}
			}
		}
		else
		{
			isHit = false;
			if (hitTimer >= activationTime)
			{
				hitTimer = 0f;
				buttonObject.SetActive(value: false);
			}
		}
	}
}
