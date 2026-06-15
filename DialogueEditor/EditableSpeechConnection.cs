using System.Runtime.Serialization;

namespace DialogueEditor;

[DataContract]
public class EditableSpeechConnection : EditableConnection
{
	public EditableSpeechNode Speech;

	public override eConnectiontype ConnectionType => eConnectiontype.Speech;

	public EditableSpeechConnection(EditableSpeechNode node)
	{
		Speech = node;
		NodeUID = node.ID;
	}
}
