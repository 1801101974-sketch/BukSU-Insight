using System;
using UnityEngine;

[Serializable]
public class DialogueChoice
{
	[SerializeField]
	private string m_ChoicePreview;

	[SerializeField]
	private DialogueNode m_ChoiceNode;

	public string ChoicePreview => m_ChoicePreview;

	public DialogueNode ChoiceNode => m_ChoiceNode;
}
