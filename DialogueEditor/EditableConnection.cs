using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DialogueEditor;

[DataContract]
[KnownType(typeof(EditableIntCondition))]
[KnownType(typeof(EditableBoolCondition))]
public abstract class EditableConnection
{
	public enum eConnectiontype
	{
		Speech,
		Option
	}

	[DataMember]
	public List<EditableCondition> Conditions;

	[DataMember]
	public int NodeUID;

	public abstract eConnectiontype ConnectionType { get; }

	public EditableConnection()
	{
		Conditions = new List<EditableCondition>();
	}

	public void AddCondition(EditableCondition condition)
	{
		Conditions.Add(condition);
	}
}
