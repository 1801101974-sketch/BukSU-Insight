namespace DialogueEditor;

public class BoolParameter : Parameter
{
	public bool BoolValue;

	public BoolParameter(string name, bool defaultValue)
		: base(name)
	{
		BoolValue = defaultValue;
	}
}
