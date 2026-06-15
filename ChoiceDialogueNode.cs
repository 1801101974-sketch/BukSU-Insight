using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue/Node/Choice")]
public class ChoiceDialogueNode : DialogueNode
{
	[SerializeField]
	private DialogueChoice[] m_Choices;

	public DialogueChoice[] Choices => m_Choices;

	public override bool CanBeFollowedByNode(DialogueNode node)
	{
		return m_Choices.Any((DialogueChoice x) => x.ChoiceNode == node);
	}

	public override void Accept(DialogueNodeVisitor visitor)
	{
		visitor.Visit(this);
	}
}
