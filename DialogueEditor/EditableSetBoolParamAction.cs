using System.Runtime.Serialization;

namespace DialogueEditor;

[DataContract]
public class EditableSetBoolParamAction : EditableSetParamAction
{
	[DataMember]
	public bool Value;

	public override eParamActionType ParamActionType => eParamActionType.Bool;

	public EditableSetBoolParamAction(string paramName)
		: base(paramName)
	{
	}
}
