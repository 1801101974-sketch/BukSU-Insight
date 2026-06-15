using System.Runtime.Serialization;

namespace DialogueEditor;

[DataContract]
public class EditableBoolParameter : EditableParameter
{
	[DataMember]
	public bool BoolValue;

	public override eParamType ParameterType => eParamType.Bool;

	public EditableBoolParameter(string name)
		: base(name)
	{
	}
}
