namespace DialogueEditor;

public class OptionConnection : Connection
{
	public OptionNode OptionNode;

	public override eConnectionType ConnectionType => eConnectionType.Option;

	public OptionConnection(OptionNode node)
	{
		OptionNode = node;
	}
}
