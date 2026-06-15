using System.Runtime.Serialization;

namespace DialogueEditor;

[DataContract]
public abstract class EditableParameter
{
	public enum eParamType
	{
		Bool,
		Int
	}

	public const int MAX_NAME_SIZE = 24;

	[DataMember]
	public string ParameterName;

	public abstract eParamType ParameterType { get; }

	public EditableParameter(string name)
	{
		ParameterName = name;
	}
}
