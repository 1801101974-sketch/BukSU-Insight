namespace DialogueEditor;

public class SetBoolParamAction : SetParamAction
{
	public bool Value;

	public override eParamActionType ParamActionType => eParamActionType.Bool;
}
