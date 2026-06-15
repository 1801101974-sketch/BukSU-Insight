using System.IO;
using UnityEngine;

public class ImageCapture : MonoBehaviour
{
	public Camera captureCamera;

	public string filePath;

	private void Start()
	{
		filePath = Path.Combine(Application.persistentDataPath, "capturedImage.png");
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.S) && Physics.Raycast(captureCamera.ScreenPointToRay(new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f)), out var hitInfo))
		{
			Texture2D texture = CaptureFrameAtPoint(hitInfo.point);
			SaveTextureToFile(texture, filePath);
		}
	}

	private Texture2D CaptureFrameAtPoint(Vector3 point)
	{
		RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
		captureCamera.targetTexture = renderTexture;
		captureCamera.Render();
		Vector3 vector = captureCamera.WorldToScreenPoint(point);
		Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGB24, mipChain: false);
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(vector.x, vector.y, 1f, 1f), 0, 0);
		texture2D.Apply();
		captureCamera.targetTexture = null;
		RenderTexture.active = null;
		return texture2D;
	}

	private void SaveTextureToFile(Texture2D texture, string filePath)
	{
		byte[] bytes = texture.EncodeToPNG();
		File.WriteAllBytes(filePath, bytes);
		Debug.Log("Image saved to: " + filePath);
	}
}
