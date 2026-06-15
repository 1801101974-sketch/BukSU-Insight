using System;
using UnityEngine;

public class FlowListener : MonoBehaviour
{
	[SerializeField]
	private FlowChannel m_Channel;

	[SerializeField]
	private FlowListenerEntry[] m_Entries;

	private void Awake()
	{
		FlowChannel channel = m_Channel;
		channel.OnFlowStateChanged = (FlowChannel.FlowStateCallback)Delegate.Combine(channel.OnFlowStateChanged, new FlowChannel.FlowStateCallback(OnFlowStateChanged));
	}

	private void OnDestroy()
	{
		FlowChannel channel = m_Channel;
		channel.OnFlowStateChanged = (FlowChannel.FlowStateCallback)Delegate.Remove(channel.OnFlowStateChanged, new FlowChannel.FlowStateCallback(OnFlowStateChanged));
	}

	private void OnFlowStateChanged(FlowState state)
	{
		Array.Find(m_Entries, (FlowListenerEntry x) => x.m_State == state)?.m_Event.Invoke();
	}
}
