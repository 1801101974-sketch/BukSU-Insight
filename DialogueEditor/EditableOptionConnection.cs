using System.Runtime.Serialization;

namespace DialogueEditor;

[DataContract]
public class EditableOptionConnection : EditableConnection
{
	public EditableOptionNode Option;

	public override eConnectiontype ConnectionType => eConnectiontype.Option;

	public EditableOptionConnection(EditableOptionNode node)
	{
		Option = node;
		NodeUID = node.ID;
	}
}
