using UnityEngine;

public class Flip : MonoBehaviour
{
	private bool isFlipped;

	private Vector3 originalRotation;

	private Vector3 flippedRotation = new Vector3(90f, 180f, 180f);

	private float touchStartTime;

	private float touchDuration = 0.2f;

	private void Start()
	{
		originalRotation = base.transform.rotation.eulerAngles;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			touchStartTime = Time.time;
		}
		if (Input.GetMouseButtonUp(0) && Time.time - touchStartTime < touchDuration)
		{
			ToggleFlip();
		}
		Input.GetMouseButton(0);
	}

	private void ToggleFlip()
	{
		if (isFlipped)
		{
			base.transform.rotation = Quaternion.Euler(originalRotation);
		}
		else
		{
			base.transform.rotation = Quaternion.Euler(flippedRotation);
		}
		isFlipped = !isFlipped;
	}
}
