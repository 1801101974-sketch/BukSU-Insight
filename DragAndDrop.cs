using System.Collections;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
	private bool isDragging;

	private Vector3 offset;

	public GameObject targetObject;

	public float snapDistanceThreshold = 0.5f;

	public GameObject Active1;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && !isDragging)
		{
			OnDragStart(Input.mousePosition);
		}
		else if (Input.GetMouseButtonUp(0) && isDragging)
		{
			OnDragEnd();
		}
		else if (isDragging)
		{
			OnDragging(Input.mousePosition);
		}
		if (Input.touchCount <= 0)
		{
			return;
		}
		Touch touch = Input.GetTouch(0);
		switch (touch.phase)
		{
		case TouchPhase.Began:
			if (!isDragging)
			{
				OnDragStart(touch.position);
			}
			break;
		case TouchPhase.Moved:
			if (isDragging)
			{
				OnDragging(touch.position);
			}
			break;
		case TouchPhase.Ended:
			if (isDragging)
			{
				OnDragEnd();
			}
			break;
		case TouchPhase.Stationary:
			break;
		}
	}

	private void OnDragStart(Vector2 inputPosition)
	{
		if (Physics.Raycast(Camera.main.ScreenPointToRay(inputPosition), out var hitInfo) && hitInfo.collider.gameObject == base.gameObject)
		{
			isDragging = true;
			offset = base.transform.position - hitInfo.point;
		}
	}

	private void OnDragging(Vector2 inputPosition)
	{
		if (Physics.Raycast(Camera.main.ScreenPointToRay(inputPosition), out var hitInfo))
		{
			Vector3 vector = hitInfo.point + offset;
			if (Vector3.Distance(vector, targetObject.transform.position) < snapDistanceThreshold)
			{
				base.transform.position = targetObject.transform.position;
				base.transform.parent = targetObject.transform;
				StartCoroutine(ShowPanelAfterDelay(1.5f));
			}
			else
			{
				base.transform.parent = null;
				base.transform.position = vector;
			}
		}
		IEnumerator ShowPanelAfterDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			Active1.SetActive(value: true);
		}
	}

	private void OnDragEnd()
	{
		isDragging = false;
	}
}
