using System;
using UnityEngine.Events;

[Serializable]
public class FlowListenerEntry
{
	public FlowState m_State;

	public UnityEvent m_Event;
}
