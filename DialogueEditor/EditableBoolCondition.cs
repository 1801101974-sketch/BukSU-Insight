using System.Runtime.Serialization;

namespace DialogueEditor;

[DataContract]
public class EditableBoolCondition : EditableCondition
{
	public enum eCheckType
	{
		equal,
		notEqual
	}

	[DataMember]
	public eCheckType CheckType;

	[DataMember]
	public bool RequiredValue;

	public override eConditionType ConditionType => eConditionType.BoolCondition;

	public EditableBoolCondition(string name)
		: base(name)
	{
	}
}
