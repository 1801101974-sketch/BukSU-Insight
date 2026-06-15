using DialogueEditor;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
	[SerializeField]
	public NPCConversation convo;

	public DialogNode initialDialogNode;

	public GameObject StartDialog;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			StartDialog.SetActive(value: true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			StartDialog.SetActive(value: false);
		}
	}

	public void StartConvo()
	{
		ConversationManager.Instance.StartConversation(convo);
	}
}
