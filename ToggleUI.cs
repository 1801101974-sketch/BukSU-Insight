using UnityEngine;

public class ToggleUI : MonoBehaviour
{
	public GameObject uiPanel;

	public GameObject[] otherUIPanels;

	private void Update()
	{
		if (!Input.GetKeyDown(KeyCode.M))
		{
			return;
		}
		bool flag = !uiPanel.activeSelf;
		uiPanel.SetActive(flag);
		if (!flag)
		{
			return;
		}
		GameObject[] array = otherUIPanels;
		foreach (GameObject gameObject in array)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(value: false);
			}
		}
	}
}
