namespace DialogueEditor;

public abstract class Condition
{
	public enum eConditionType
	{
		IntCondition,
		BoolCondition
	}

	public string ParameterName;

	public abstract eConditionType ConditionType { get; }
}
