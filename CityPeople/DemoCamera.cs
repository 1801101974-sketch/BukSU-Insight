using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CityPeople;

public class DemoCamera : MonoBehaviour
{
	public AnimationCurve easingCurve;

	public float motionTimeTotal = 1f;

	public Camera[] cameraTargets;

	public GameObject TextLeft;

	public GameObject TextRight;

	public Text TextCharName;

	public string HintMessage;

	public List<Material> MaterialRef;

	public Texture[] Textures;

	private int currentCamera;

	private Vector3 targetPosition;

	private Vector3 startPosition;

	private float motionTime = 2.1f;

	private Collider hoveredCollider;

	private Vector3 hoveredStoredPos;

	private Vector3 hoveredStoredScale;

	private Quaternion hoveredStoredRotation;

	private CityPeople hoveredPeople;

	private void Start()
	{
		motionTime = motionTimeTotal + 0.1f;
		DisableArrows();
		if (cameraTargets.Length != 0 && cameraTargets[currentCamera] != null)
		{
			base.transform.position = cameraTargets[currentCamera].transform.position;
		}
		TextCharName.text = HintMessage;
	}

	private void Update()
	{
		if (motionTime < motionTimeTotal)
		{
			motionTime += Time.deltaTime;
			float time = motionTime / motionTimeTotal;
			float t = easingCurve.Evaluate(time);
			base.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
		}
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			OnRightClick();
		}
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			OnLeftClick();
		}
		CharacterClicks();
		MouseMove();
		HoverSpin();
	}

	private void HoverSpin()
	{
		if (hoveredPeople != null)
		{
			hoveredPeople.transform.Rotate(0f, 90f * Time.deltaTime, 0f);
		}
	}

	private void MouseMove()
	{
		Vector3 mousePosition = Input.mousePosition;
		if (!(mousePosition.x >= 0f) || !(mousePosition.x <= (float)Screen.width) || !(mousePosition.y >= 0f) || !(mousePosition.y <= (float)Screen.height))
		{
			return;
		}
		if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out var hitInfo))
		{
			Collider collider = hitInfo.collider;
			if (collider != hoveredCollider)
			{
				if (hoveredCollider != null)
				{
					ColliderExit(hoveredCollider);
				}
				ColliderEnter(collider);
				hoveredCollider = collider;
			}
		}
		else if (hoveredCollider != null)
		{
			ColliderExit(hoveredCollider);
			hoveredCollider = null;
		}
	}

	private void ColliderEnter(Collider currentCollider)
	{
		hoveredPeople = currentCollider.gameObject.GetComponent<CityPeople>();
		if (hoveredPeople != null)
		{
			if (TextCharName != null)
			{
				TextCharName.text = hoveredPeople.transform.parent.name + " > " + hoveredPeople.name + " (mat: " + hoveredPeople.CurrentPaletteName + ")";
			}
			Transform transform = hoveredPeople.gameObject.transform;
			hoveredStoredPos = transform.position;
			hoveredStoredScale = transform.localScale;
			hoveredStoredRotation = transform.rotation;
			transform.position += new Vector3(0f, 0f, 1f);
			transform.localScale = Vector3.one * 1.5f;
		}
	}

	private string CharacterInfo(CityPeople cityPeople)
	{
		string result = cityPeople.transform.parent.name + " > " + cityPeople.name + " ";
		Renderer[] componentsInChildren = cityPeople.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			_ = componentsInChildren[i];
		}
		return result;
	}

	private void ColliderExit(Collider currentCollider)
	{
		hoveredPeople = currentCollider.gameObject.GetComponent<CityPeople>();
		if (hoveredPeople != null)
		{
			Transform obj = hoveredPeople.gameObject.transform;
			obj.position = hoveredStoredPos;
			obj.localScale = hoveredStoredScale;
		}
		if (TextCharName != null)
		{
			TextCharName.text = HintMessage;
		}
		hoveredPeople = null;
	}

	private void CharacterClicks()
	{
		if (Input.GetMouseButtonDown(0) && hoveredPeople != null)
		{
			int num = MaterialRef.FindIndex((Material m) => m.name == hoveredPeople.CurrentPaletteName);
			num++;
			if (num == MaterialRef.Count)
			{
				num = 0;
			}
			hoveredPeople.SetPalette(MaterialRef[num]);
			if (TextCharName != null)
			{
				TextCharName.text = hoveredPeople.transform.parent.name + " > " + hoveredPeople.name + " (mat: " + hoveredPeople.CurrentPaletteName + ")";
			}
		}
	}

	private void GoTo(Camera cam)
	{
		startPosition = base.transform.position;
		targetPosition = cam.transform.position;
		motionTime = 0f;
	}

	public void OnLeftClick()
	{
		if (currentCamera > 0)
		{
			currentCamera--;
			Camera cam = cameraTargets[currentCamera];
			GoTo(cam);
			DisableArrows();
		}
	}

	public void OnRightClick()
	{
		if (currentCamera < cameraTargets.Length - 1)
		{
			currentCamera++;
			Camera cam = cameraTargets[currentCamera];
			GoTo(cam);
			DisableArrows();
		}
	}

	private void DisableArrows()
	{
		if (TextRight != null && TextLeft != null)
		{
			TextRight.SetActive(currentCamera < cameraTargets.Length - 1);
			TextLeft.SetActive(currentCamera > 0);
		}
	}
}
