using System.Runtime.Serialization;

namespace DialogueEditor;

[DataContract]
public class EditableIntParameter : EditableParameter
{
	[DataMember]
	public int IntValue;

	public override eParamType ParameterType => eParamType.Int;

	public EditableIntParameter(string name)
		: base(name)
	{
	}
}
