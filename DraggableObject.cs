using UnityEngine;

public class DraggableObject : MonoBehaviour
{
	private Vector2 startPosition;

	private bool isDragging;

	private void Start()
	{
		startPosition = base.transform.position;
	}

	private void Update()
	{
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out var hitInfo, 100f) && hitInfo.collider.gameObject == base.gameObject)
		{
			isDragging = true;
			startPosition = base.transform.position;
		}
		if (isDragging)
		{
			Vector2 position = Input.GetTouch(0).position;
			Vector3 vector = Camera.main.ScreenToWorldPoint(position);
			base.transform.position = new Vector3(vector.x, vector.y, base.transform.position.z);
		}
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			isDragging = false;
		}
	}
}
