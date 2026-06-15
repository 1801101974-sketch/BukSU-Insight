using System.Collections.Generic;
using TMPro;

namespace DialogueEditor;

public abstract class ConversationNode
{
	public enum eNodeType
	{
		Speech,
		Option
	}

	public string Text;

	public List<Connection> Connections;

	public List<SetParamAction> ParamActions;

	public TMP_FontAsset TMPFont;

	public abstract eNodeType NodeType { get; }

	public Connection.eConnectionType ConnectionType
	{
		get
		{
			if (Connections.Count > 0)
			{
				return Connections[0].ConnectionType;
			}
			return Connection.eConnectionType.None;
		}
	}

	public ConversationNode()
	{
		Connections = new List<Connection>();
		ParamActions = new List<SetParamAction>();
	}
}
