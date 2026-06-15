using System.Runtime.Serialization;

namespace DialogueEditor;

[DataContract]
public abstract class EditableSetParamAction
{
	public enum eParamActionType
	{
		Int,
		Bool
	}

	[DataMember]
	public string ParameterName;

	public abstract eParamActionType ParamActionType { get; }

	public EditableSetParamAction(string paramName)
	{
		ParameterName = paramName;
	}
}
