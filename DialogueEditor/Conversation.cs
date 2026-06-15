using System.Collections.Generic;
using TMPro;

namespace DialogueEditor;

public class Conversation
{
	public SpeechNode Root;

	public List<Parameter> Parameters;

	public TMP_FontAsset ContinueFont;

	public TMP_FontAsset EndConversationFont;

	public Conversation()
	{
		Parameters = new List<Parameter>();
	}

	public void SetInt(string paramName, int value, out eParamStatus status)
	{
		if (GetParameter(paramName) is IntParameter intParameter)
		{
			intParameter.IntValue = value;
			status = eParamStatus.OK;
		}
		else
		{
			status = eParamStatus.NoParamFound;
		}
	}

	public void SetBool(string paramName, bool value, out eParamStatus status)
	{
		if (GetParameter(paramName) is BoolParameter boolParameter)
		{
			boolParameter.BoolValue = value;
			status = eParamStatus.OK;
		}
		else
		{
			status = eParamStatus.NoParamFound;
		}
	}

	public int GetInt(string paramName, out eParamStatus status)
	{
		if (GetParameter(paramName) is IntParameter intParameter)
		{
			status = eParamStatus.OK;
			return intParameter.IntValue;
		}
		status = eParamStatus.NoParamFound;
		return 0;
	}

	public bool GetBool(string paramName, out eParamStatus status)
	{
		if (GetParameter(paramName) is BoolParameter boolParameter)
		{
			status = eParamStatus.OK;
			return boolParameter.BoolValue;
		}
		status = eParamStatus.NoParamFound;
		return false;
	}

	private Parameter GetParameter(string name)
	{
		for (int i = 0; i < Parameters.Count; i++)
		{
			if (Parameters[i].ParameterName == name)
			{
				return Parameters[i];
			}
		}
		return null;
	}
}
