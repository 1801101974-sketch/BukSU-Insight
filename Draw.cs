using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
	public Material lineMaterial;

	public float lineSize = 0.1f;

	public Color lineColor = Color.black;

	private LineRenderer currentLine;

	private List<Vector3> linePositions;

	private RenderTexture doodleTexture;

	private void Start()
	{
		linePositions = new List<Vector3>();
		doodleTexture = new RenderTexture(1024, 1024, 0);
		lineMaterial.mainTexture = doodleTexture;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo) && hitInfo.transform.CompareTag("DoodleObject"))
		{
			CreateNewLine();
		}
		if (Input.GetMouseButton(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo2) && hitInfo2.transform.CompareTag("DoodleObject"))
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = 10f;
			Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePosition);
			UpdateLine(newPosition);
		}
	}

	private void CreateNewLine()
	{
		GameObject gameObject = new GameObject("Line");
		currentLine = gameObject.AddComponent<LineRenderer>();
		currentLine.material = lineMaterial;
		currentLine.startWidth = lineSize;
		currentLine.endWidth = lineSize;
		currentLine.startColor = lineColor;
		currentLine.endColor = lineColor;
		linePositions.Clear();
	}

	private void UpdateLine(Vector3 newPosition)
	{
		linePositions.Add(newPosition);
		currentLine.positionCount = linePositions.Count;
		currentLine.SetPositions(linePositions.ToArray());
	}
}
