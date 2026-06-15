using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;

namespace DialogueEditor;

[DataContract]
[KnownType(typeof(EditableSpeechConnection))]
[KnownType(typeof(EditableOptionConnection))]
[KnownType(typeof(EditableSetIntParamAction))]
[KnownType(typeof(EditableSetBoolParamAction))]
public abstract class EditableConversationNode
{
	public enum eNodeType
	{
		Speech,
		Option
	}

	[DataContract]
	public class EditorArgs
	{
		[DataMember]
		public float xPos;

		[DataMember]
		public float yPos;

		[DataMember]
		public bool isRoot;
	}

	[DataMember]
	public EditorArgs EditorInfo;

	[DataMember]
	public int ID;

	[DataMember]
	public string Text;

	[DataMember]
	public List<EditableConnection> Connections;

	[DataMember]
	public List<int> parentUIDs;

	[DataMember]
	public List<EditableSetParamAction> ParamActions;

	[DataMember]
	public string TMPFontGUID;

	public TMP_FontAsset TMPFont;

	public List<EditableConversationNode> parents;

	public abstract eNodeType NodeType { get; }

	public EditableConversationNode()
	{
		parents = new List<EditableConversationNode>();
		Connections = new List<EditableConnection>();
		ParamActions = new List<EditableSetParamAction>();
		parentUIDs = new List<int>();
		EditorInfo = new EditorArgs
		{
			xPos = 0f,
			yPos = 0f,
			isRoot = false
		};
	}

	public void RegisterUIDs()
	{
		if (parentUIDs != null)
		{
			parentUIDs.Clear();
		}
		parentUIDs = new List<int>();
		for (int i = 0; i < parents.Count; i++)
		{
			parentUIDs.Add(parents[i].ID);
		}
	}

	public void RemoveSelfFromTree()
	{
		for (int i = 0; i < Connections.Count; i++)
		{
			if (Connections[i] is EditableSpeechConnection)
			{
				(Connections[i] as EditableSpeechConnection).Speech.parents.Remove(this);
			}
			else if (Connections[i] is EditableOptionConnection)
			{
				(Connections[i] as EditableOptionConnection).Option.parents.Remove(this);
			}
		}
		for (int j = 0; j < parents.Count; j++)
		{
			parents[j].DeleteConnectionChild(this);
		}
	}

	public void DeleteConnectionChild(EditableConversationNode node)
	{
		if (Connections.Count == 0)
		{
			return;
		}
		if (node.NodeType == eNodeType.Speech && Connections[0] is EditableSpeechConnection)
		{
			EditableSpeechNode editableSpeechNode = node as EditableSpeechNode;
			for (int i = 0; i < Connections.Count; i++)
			{
				if ((Connections[i] as EditableSpeechConnection).Speech == editableSpeechNode)
				{
					Connections.RemoveAt(i);
					break;
				}
			}
		}
		else
		{
			if (!(node is EditableOptionNode) || !(Connections[0] is EditableOptionConnection))
			{
				return;
			}
			EditableOptionNode editableOptionNode = node as EditableOptionNode;
			for (int j = 0; j < Connections.Count; j++)
			{
				if ((Connections[j] as EditableOptionConnection).Option == editableOptionNode)
				{
					Connections.RemoveAt(j);
					break;
				}
			}
		}
	}

	public virtual void SerializeAssetData(NPCConversation conversation)
	{
		conversation.GetNodeData(ID).TMPFont = TMPFont;
	}

	public virtual void DeserializeAssetData(NPCConversation conversation)
	{
		TMPFont = conversation.GetNodeData(ID).TMPFont;
	}
}
