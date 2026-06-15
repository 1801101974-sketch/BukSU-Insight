namespace DialogueEditor;

public class IntParameter : Parameter
{
	public int IntValue;

	public IntParameter(string name, int defalutValue)
		: base(name)
	{
		IntValue = defalutValue;
	}
}
