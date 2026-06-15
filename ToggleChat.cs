using UnityEngine;

public class ToggleChat : MonoBehaviour
{
	public GameObject uiPanel;

	public GameObject[] otherUIPanels;

	private bool[] previousStates;

	private void Start()
	{
		previousStates = new bool[otherUIPanels.Length];
	}

	private void Update()
	{
		if (!Input.GetKeyDown(KeyCode.F))
		{
			return;
		}
		bool flag = !uiPanel.activeSelf;
		uiPanel.SetActive(flag);
		if (flag)
		{
			for (int i = 0; i < otherUIPanels.Length; i++)
			{
				if (otherUIPanels[i] != null)
				{
					previousStates[i] = otherUIPanels[i].activeSelf;
					otherUIPanels[i].SetActive(value: false);
				}
			}
			return;
		}
		for (int j = 0; j < otherUIPanels.Length; j++)
		{
			if (otherUIPanels[j] != null)
			{
				otherUIPanels[j].SetActive(previousStates[j]);
			}
		}
	}
}
