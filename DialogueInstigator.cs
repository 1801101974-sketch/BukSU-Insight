using System;
using UnityEngine;

public class DialogueInstigator : MonoBehaviour
{
	[SerializeField]
	private DialogueChannel m_DialogueChannel;

	[SerializeField]
	private FlowChannel m_FlowChannel;

	[SerializeField]
	private FlowState m_DialogueState;

	private DialogueSequencer m_DialogueSequencer;

	private FlowState m_CachedFlowState;

	private void Awake()
	{
		m_DialogueSequencer = new DialogueSequencer();
		DialogueSequencer dialogueSequencer = m_DialogueSequencer;
		dialogueSequencer.OnDialogueStart = (DialogueSequencer.DialogueCallback)Delegate.Combine(dialogueSequencer.OnDialogueStart, new DialogueSequencer.DialogueCallback(OnDialogueStart));
		DialogueSequencer dialogueSequencer2 = m_DialogueSequencer;
		dialogueSequencer2.OnDialogueEnd = (DialogueSequencer.DialogueCallback)Delegate.Combine(dialogueSequencer2.OnDialogueEnd, new DialogueSequencer.DialogueCallback(OnDialogueEnd));
		DialogueSequencer dialogueSequencer3 = m_DialogueSequencer;
		dialogueSequencer3.OnDialogueNodeStart = (DialogueSequencer.DialogueNodeCallback)Delegate.Combine(dialogueSequencer3.OnDialogueNodeStart, new DialogueSequencer.DialogueNodeCallback(m_DialogueChannel.RaiseDialogueNodeStart));
		DialogueSequencer dialogueSequencer4 = m_DialogueSequencer;
		dialogueSequencer4.OnDialogueNodeEnd = (DialogueSequencer.DialogueNodeCallback)Delegate.Combine(dialogueSequencer4.OnDialogueNodeEnd, new DialogueSequencer.DialogueNodeCallback(m_DialogueChannel.RaiseDialogueNodeEnd));
		DialogueChannel dialogueChannel = m_DialogueChannel;
		dialogueChannel.OnDialogueRequested = (DialogueChannel.DialogueCallback)Delegate.Combine(dialogueChannel.OnDialogueRequested, new DialogueChannel.DialogueCallback(m_DialogueSequencer.StartDialogue));
		DialogueChannel dialogueChannel2 = m_DialogueChannel;
		dialogueChannel2.OnDialogueNodeRequested = (DialogueChannel.DialogueNodeCallback)Delegate.Combine(dialogueChannel2.OnDialogueNodeRequested, new DialogueChannel.DialogueNodeCallback(m_DialogueSequencer.StartDialogueNode));
	}

	private void OnDestroy()
	{
		DialogueChannel dialogueChannel = m_DialogueChannel;
		dialogueChannel.OnDialogueNodeRequested = (DialogueChannel.DialogueNodeCallback)Delegate.Remove(dialogueChannel.OnDialogueNodeRequested, new DialogueChannel.DialogueNodeCallback(m_DialogueSequencer.StartDialogueNode));
		DialogueChannel dialogueChannel2 = m_DialogueChannel;
		dialogueChannel2.OnDialogueRequested = (DialogueChannel.DialogueCallback)Delegate.Remove(dialogueChannel2.OnDialogueRequested, new DialogueChannel.DialogueCallback(m_DialogueSequencer.StartDialogue));
		DialogueSequencer dialogueSequencer = m_DialogueSequencer;
		dialogueSequencer.OnDialogueNodeEnd = (DialogueSequencer.DialogueNodeCallback)Delegate.Remove(dialogueSequencer.OnDialogueNodeEnd, new DialogueSequencer.DialogueNodeCallback(m_DialogueChannel.RaiseDialogueNodeEnd));
		DialogueSequencer dialogueSequencer2 = m_DialogueSequencer;
		dialogueSequencer2.OnDialogueNodeStart = (DialogueSequencer.DialogueNodeCallback)Delegate.Remove(dialogueSequencer2.OnDialogueNodeStart, new DialogueSequencer.DialogueNodeCallback(m_DialogueChannel.RaiseDialogueNodeStart));
		DialogueSequencer dialogueSequencer3 = m_DialogueSequencer;
		dialogueSequencer3.OnDialogueEnd = (DialogueSequencer.DialogueCallback)Delegate.Remove(dialogueSequencer3.OnDialogueEnd, new DialogueSequencer.DialogueCallback(OnDialogueEnd));
		DialogueSequencer dialogueSequencer4 = m_DialogueSequencer;
		dialogueSequencer4.OnDialogueStart = (DialogueSequencer.DialogueCallback)Delegate.Remove(dialogueSequencer4.OnDialogueStart, new DialogueSequencer.DialogueCallback(OnDialogueStart));
		m_DialogueSequencer = null;
	}

	private void OnDialogueStart(Dialogue dialogue)
	{
		m_DialogueChannel.RaiseDialogueStart(dialogue);
		m_CachedFlowState = FlowStateMachine.Instance.CurrentState;
		m_FlowChannel.RaiseFlowStateRequest(m_DialogueState);
	}

	private void OnDialogueEnd(Dialogue dialogue)
	{
		m_FlowChannel.RaiseFlowStateRequest(m_CachedFlowState);
		m_CachedFlowState = null;
		m_DialogueChannel.RaiseDialogueEnd(dialogue);
	}
}
