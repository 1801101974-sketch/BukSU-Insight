namespace DialogueEditor;

public class BoolCondition : Condition
{
	public enum eCheckType
	{
		equal,
		notEqual
	}

	public eCheckType CheckType;

	public bool RequiredValue;

	public override eConditionType ConditionType => eConditionType.BoolCondition;
}
