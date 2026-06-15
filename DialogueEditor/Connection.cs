using System.Collections.Generic;

namespace DialogueEditor;

public abstract class Connection
{
	public enum eConnectionType
	{
		None,
		Speech,
		Option
	}

	public List<Condition> Conditions;

	public abstract eConnectionType ConnectionType { get; }

	public Connection()
	{
		Conditions = new List<Condition>();
	}
}
