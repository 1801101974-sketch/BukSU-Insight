using System;
using TMPro;
using UnityEngine;

public class UIDialogueTextBoxController : MonoBehaviour, DialogueNodeVisitor
{
	[SerializeField]
	private TextMeshProUGUI m_SpeakerText;

	[SerializeField]
	private TextMeshProUGUI m_DialogueText;

	[SerializeField]
	private RectTransform m_ChoicesBoxTransform;

	[SerializeField]
	private UIDialogueChoiceController m_ChoiceControllerPrefab;

	[SerializeField]
	private DialogueChannel m_DialogueChannel;

	private bool m_ListenToInput;

	private DialogueNode m_NextNode;

	private void Awake()
	{
		DialogueChannel dialogueChannel = m_DialogueChannel;
		dialogueChannel.OnDialogueNodeStart = (DialogueChannel.DialogueNodeCallback)Delegate.Combine(dialogueChannel.OnDialogueNodeStart, new DialogueChannel.DialogueNodeCallback(OnDialogueNodeStart));
		DialogueChannel dialogueChannel2 = m_DialogueChannel;
		dialogueChannel2.OnDialogueNodeEnd = (DialogueChannel.DialogueNodeCallback)Delegate.Combine(dialogueChannel2.OnDialogueNodeEnd, new DialogueChannel.DialogueNodeCallback(OnDialogueNodeEnd));
		base.gameObject.SetActive(value: false);
		m_ChoicesBoxTransform.gameObject.SetActive(value: false);
	}

	private void OnDestroy()
	{
		DialogueChannel dialogueChannel = m_DialogueChannel;
		dialogueChannel.OnDialogueNodeEnd = (DialogueChannel.DialogueNodeCallback)Delegate.Remove(dialogueChannel.OnDialogueNodeEnd, new DialogueChannel.DialogueNodeCallback(OnDialogueNodeEnd));
		DialogueChannel dialogueChannel2 = m_DialogueChannel;
		dialogueChannel2.OnDialogueNodeStart = (DialogueChannel.DialogueNodeCallback)Delegate.Remove(dialogueChannel2.OnDialogueNodeStart, new DialogueChannel.DialogueNodeCallback(OnDialogueNodeStart));
	}

	private void Update()
	{
		if (m_ListenToInput && Input.GetButtonDown("Submit"))
		{
			m_DialogueChannel.RaiseRequestDialogueNode(m_NextNode);
		}
	}

	private void OnDialogueNodeStart(DialogueNode node)
	{
		base.gameObject.SetActive(value: true);
		m_DialogueText.text = node.DialogueLine.Text;
		m_SpeakerText.text = node.DialogueLine.Speaker.CharacterName;
		node.Accept(this);
	}

	private void OnDialogueNodeEnd(DialogueNode node)
	{
		m_NextNode = null;
		m_ListenToInput = false;
		m_DialogueText.text = "";
		m_SpeakerText.text = "";
		foreach (Transform item in m_ChoicesBoxTransform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		base.gameObject.SetActive(value: false);
		m_ChoicesBoxTransform.gameObject.SetActive(value: false);
	}

	public void Visit(BasicDialogueNode node)
	{
		m_ListenToInput = true;
		m_NextNode = node.NextNode;
	}

	public void Visit(ChoiceDialogueNode node)
	{
		m_ChoicesBoxTransform.gameObject.SetActive(value: true);
		DialogueChoice[] choices = node.Choices;
		foreach (DialogueChoice choice in choices)
		{
			UnityEngine.Object.Instantiate(m_ChoiceControllerPrefab, m_ChoicesBoxTransform).Choice = choice;
		}
	}
}
