using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
	private void Start()
	{
		if (!string.IsNullOrEmpty(SceneDataCarrier.objectToActivateName))
		{
			GameObject gameObject = GameObject.Find(SceneDataCarrier.objectToActivateName);
			if (gameObject != null)
			{
				gameObject.SetActive(value: true);
			}
		}
	}
}
