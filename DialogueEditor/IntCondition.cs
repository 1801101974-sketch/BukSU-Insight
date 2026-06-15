namespace DialogueEditor;

public class IntCondition : Condition
{
	public enum eCheckType
	{
		equal,
		lessThan,
		greaterThan
	}

	public eCheckType CheckType;

	public int RequiredValue;

	public override eConditionType ConditionType => eConditionType.IntCondition;
}
