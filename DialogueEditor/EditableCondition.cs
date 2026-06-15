using System.Runtime.Serialization;

namespace DialogueEditor;

[DataContract]
public abstract class EditableCondition
{
	public enum eConditionType
	{
		IntCondition,
		BoolCondition
	}

	[DataMember]
	public string ParameterName;

	public abstract eConditionType ConditionType { get; }

	public EditableCondition(string name)
	{
		ParameterName = name;
	}
}
