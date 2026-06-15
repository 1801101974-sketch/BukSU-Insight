using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
	public static DialogManager Instance;

	public Text dialogText;

	public Button[] responseButtons;

	private DialogNode currentDialogNode;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void SelectResponse(int responseIndex)
	{
		if (responseIndex < currentDialogNode.responses.Count)
		{
			currentDialogNode = currentDialogNode.responses[responseIndex].nextDialogNode;
			UpdateUI();
		}
	}

	private void UpdateUI()
	{
		dialogText.text = currentDialogNode.dialogText;
		for (int i = 0; i < responseButtons.Length; i++)
		{
			if (i < currentDialogNode.responses.Count)
			{
				responseButtons[i].gameObject.SetActive(value: true);
				responseButtons[i].GetComponentInChildren<Text>().text = currentDialogNode.responses[i].responseText;
			}
			else
			{
				responseButtons[i].gameObject.SetActive(value: false);
			}
		}
	}
}
