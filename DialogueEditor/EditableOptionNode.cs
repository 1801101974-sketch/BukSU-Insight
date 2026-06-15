using System.Runtime.Serialization;

namespace DialogueEditor;

[DataContract]
public class EditableOptionNode : EditableConversationNode
{
	public EditableSpeechNode Speech;

	[DataMember]
	public int SpeechUID;

	public override eNodeType NodeType => eNodeType.Option;

	public EditableOptionNode()
	{
		SpeechUID = -1;
	}

	public void AddSpeech(EditableSpeechNode newSpeech)
	{
		Connections.Add(new EditableSpeechConnection(newSpeech));
		newSpeech.parents.Add(this);
	}
}
