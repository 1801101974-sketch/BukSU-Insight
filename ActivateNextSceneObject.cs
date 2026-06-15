using UnityEngine;

public class ActivateNextSceneObject : MonoBehaviour
{
	public GameObject targetObjectNameInNextScene;

	public void OnButtonClick()
	{
		targetObjectNameInNextScene.SetActive(value: true);
	}
}
