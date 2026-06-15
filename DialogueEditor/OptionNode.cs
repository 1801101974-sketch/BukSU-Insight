using UnityEngine.Events;

namespace DialogueEditor;

public class OptionNode : ConversationNode
{
	public UnityEvent Event;

	public override eNodeType NodeType => eNodeType.Option;
}
