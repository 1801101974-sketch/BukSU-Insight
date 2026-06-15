using System;
using UnityEngine;

public class FlowStateMachine : MonoBehaviour
{
	[SerializeField]
	private FlowChannel m_Channel;

	[SerializeField]
	private FlowState m_StartupState;

	private FlowState m_CurrentState;

	private static FlowStateMachine ms_Instance;

	public FlowState CurrentState => m_CurrentState;

	public static FlowStateMachine Instance => ms_Instance;

	private void Awake()
	{
		ms_Instance = this;
		FlowChannel channel = m_Channel;
		channel.OnFlowStateRequested = (FlowChannel.FlowStateCallback)Delegate.Combine(channel.OnFlowStateRequested, new FlowChannel.FlowStateCallback(SetFlowState));
	}

	private void Start()
	{
		SetFlowState(m_StartupState);
	}

	private void OnDestroy()
	{
		FlowChannel channel = m_Channel;
		channel.OnFlowStateRequested = (FlowChannel.FlowStateCallback)Delegate.Remove(channel.OnFlowStateRequested, new FlowChannel.FlowStateCallback(SetFlowState));
		ms_Instance = null;
	}

	private void SetFlowState(FlowState state)
	{
		if (m_CurrentState != state)
		{
			m_CurrentState = state;
			m_Channel.RaiseFlowStateChanged(m_CurrentState);
		}
	}
}
