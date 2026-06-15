using System.Runtime.Serialization;

namespace DialogueEditor;

[DataContract]
public class EditableIntCondition : EditableCondition
{
	public enum eCheckType
	{
		equal,
		lessThan,
		greaterThan
	}

	[DataMember]
	public eCheckType CheckType;

	[DataMember]
	public int RequiredValue;

	public override eConditionType ConditionType => eConditionType.IntCondition;

	public EditableIntCondition(string name)
		: base(name)
	{
	}
}
