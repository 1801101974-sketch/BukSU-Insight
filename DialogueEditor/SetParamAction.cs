namespace DialogueEditor;

public abstract class SetParamAction
{
	public enum eParamActionType
	{
		Int,
		Bool
	}

	public string ParameterName;

	public abstract eParamActionType ParamActionType { get; }
}
