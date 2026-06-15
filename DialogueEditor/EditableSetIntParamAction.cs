using System.Runtime.Serialization;

namespace DialogueEditor;

[DataContract]
public class EditableSetIntParamAction : EditableSetParamAction
{
	[DataMember]
	public int Value;

	public override eParamActionType ParamActionType => eParamActionType.Int;

	public EditableSetIntParamAction(string paramName)
		: base(paramName)
	{
	}
}
