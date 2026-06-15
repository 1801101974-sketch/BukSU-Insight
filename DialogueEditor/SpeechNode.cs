using UnityEngine;
using UnityEngine.Events;

namespace DialogueEditor;

public class SpeechNode : ConversationNode
{
	public string Name;

	public bool AutomaticallyAdvance;

	public bool AutoAdvanceShouldDisplayOption;

	public float TimeUntilAdvance;

	public Sprite Icon;

	public AudioClip Audio;

	public float Volume;

	public UnityEvent Event;

	public override eNodeType NodeType => eNodeType.Speech;
}
