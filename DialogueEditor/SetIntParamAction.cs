namespace DialogueEditor;

public class SetIntParamAction : SetParamAction
{
	public int Value;

	public override eParamActionType ParamActionType => eParamActionType.Int;
}
