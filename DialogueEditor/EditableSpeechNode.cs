using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace DialogueEditor;

[DataContract]
public class EditableSpeechNode : EditableConversationNode
{
	[DataMember]
	public string Name;

	public Sprite Icon;

	[DataMember]
	public string IconGUID;

	public AudioClip Audio;

	[DataMember]
	public string AudioGUID;

	[DataMember]
	public float Volume;

	[DataMember]
	public bool AdvanceDialogueAutomatically;

	[DataMember]
	public bool AutoAdvanceShouldDisplayOption;

	[DataMember]
	public float TimeUntilAdvance;

	public List<EditableOptionNode> Options;

	[DataMember]
	public List<int> OptionUIDs;

	public EditableSpeechNode Speech;

	[DataMember]
	public int SpeechUID;

	public override eNodeType NodeType => eNodeType.Speech;

	public void AddOption(EditableOptionNode newOption)
	{
		if (Connections.Count > 0 && Connections[0] is EditableSpeechConnection)
		{
			for (int i = 0; i < Connections.Count; i++)
			{
				(Connections[0] as EditableSpeechConnection).Speech.parents.Remove(this);
			}
			Connections.Clear();
		}
		if (Connections.Count > 0 && Connections[0] is EditableOptionConnection)
		{
			for (int j = 0; j < Connections.Count; j++)
			{
				if ((Connections[0] as EditableOptionConnection).Option == newOption)
				{
					return;
				}
			}
		}
		Connections.Add(new EditableOptionConnection(newOption));
		if (!newOption.parents.Contains(this))
		{
			newOption.parents.Add(this);
		}
	}

	public void AddSpeech(EditableSpeechNode newSpeech)
	{
		if (Connections.Count > 0 && Connections[0] is EditableOptionConnection)
		{
			for (int i = 0; i < Connections.Count; i++)
			{
				(Connections[0] as EditableOptionConnection).Option.parents.Remove(this);
			}
			Connections.Clear();
		}
		if (Connections.Count > 0 && Connections[0] is EditableSpeechConnection)
		{
			for (int j = 0; j < Connections.Count; j++)
			{
				if ((Connections[0] as EditableSpeechConnection).Speech == newSpeech)
				{
					return;
				}
			}
		}
		if (parents.Contains(newSpeech))
		{
			parents.Remove(newSpeech);
			newSpeech.DeleteConnectionChild(this);
		}
		Connections.Add(new EditableSpeechConnection(newSpeech));
		if (!newSpeech.parents.Contains(this))
		{
			newSpeech.parents.Add(this);
		}
	}

	public override void SerializeAssetData(NPCConversation conversation)
	{
		base.SerializeAssetData(conversation);
		conversation.GetNodeData(ID).Audio = Audio;
		conversation.GetNodeData(ID).Icon = Icon;
	}

	public override void DeserializeAssetData(NPCConversation conversation)
	{
		base.DeserializeAssetData(conversation);
		Audio = conversation.GetNodeData(ID).Audio;
		Icon = conversation.GetNodeData(ID).Icon;
	}
}
