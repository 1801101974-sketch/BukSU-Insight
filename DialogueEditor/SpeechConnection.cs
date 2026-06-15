namespace DialogueEditor;

public class SpeechConnection : Connection
{
	public SpeechNode SpeechNode;

	public override eConnectionType ConnectionType => eConnectionType.Speech;

	public SpeechConnection(SpeechNode node)
	{
		SpeechNode = node;
	}
}
