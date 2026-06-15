using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DialogueEditor;

[Serializable]
[DisallowMultipleComponent]
public class NPCConversation : MonoBehaviour
{
	public const int CurrentVersion = 110;

	private readonly string CHILD_NAME = "ConversationEventInfo";

	[SerializeField]
	public int CurrentIDCounter = 1;

	[SerializeField]
	private string json;

	[SerializeField]
	private int saveVersion;

	[SerializeField]
	public string DefaultName;

	[SerializeField]
	public Sprite DefaultSprite;

	[SerializeField]
	public TMP_FontAsset DefaultFont;

	[FormerlySerializedAs("Events")]
	[SerializeField]
	private List<NodeEventHolder> NodeSerializedDataList;

	[SerializeField]
	public TMP_FontAsset ContinueFont;

	[SerializeField]
	public TMP_FontAsset EndConversationFont;

	public UnityEvent Event;

	public List<EditableParameter> ParameterList;

	public int Version => saveVersion;

	public NodeEventHolder GetNodeData(int id)
	{
		if (NodeSerializedDataList == null)
		{
			NodeSerializedDataList = new List<NodeEventHolder>();
		}
		for (int i = 0; i < NodeSerializedDataList.Count; i++)
		{
			if (NodeSerializedDataList[i].NodeID == id)
			{
				return NodeSerializedDataList[i];
			}
		}
		if (base.transform.Find(CHILD_NAME) == null)
		{
			new GameObject(CHILD_NAME).transform.SetParent(base.transform);
		}
		NodeEventHolder nodeEventHolder = base.transform.Find(CHILD_NAME).gameObject.AddComponent<NodeEventHolder>();
		nodeEventHolder.NodeID = id;
		nodeEventHolder.Event = new UnityEvent();
		NodeSerializedDataList.Add(nodeEventHolder);
		return nodeEventHolder;
	}

	public void DeleteDataForNode(int id)
	{
		if (NodeSerializedDataList == null)
		{
			return;
		}
		for (int i = 0; i < NodeSerializedDataList.Count; i++)
		{
			if (NodeSerializedDataList[i].NodeID == id)
			{
				UnityEngine.Object.DestroyImmediate(NodeSerializedDataList[i]);
				NodeSerializedDataList.RemoveAt(i);
			}
		}
	}

	public EditableParameter GetParameter(string name)
	{
		for (int i = 0; i < ParameterList.Count; i++)
		{
			if (ParameterList[i].ParameterName == name)
			{
				return ParameterList[i];
			}
		}
		return null;
	}

	public void Serialize(EditableConversation conversation)
	{
		saveVersion = 110;
		conversation.Parameters = ParameterList;
		json = Jsonify(conversation);
	}

	public Conversation Deserialize()
	{
		EditableConversation ec = DeserializeForEditor();
		return ConstructConversationObject(ec);
	}

	public EditableConversation DeserializeForEditor()
	{
		EditableConversation editableConversation = Dejsonify();
		if (editableConversation != null)
		{
			ParameterList = editableConversation.Parameters;
			if (editableConversation.SpeechNodes != null)
			{
				for (int i = 0; i < editableConversation.SpeechNodes.Count; i++)
				{
					editableConversation.SpeechNodes[i].DeserializeAssetData(this);
				}
			}
			if (editableConversation.Options != null)
			{
				for (int j = 0; j < editableConversation.Options.Count; j++)
				{
					editableConversation.Options[j].DeserializeAssetData(this);
				}
			}
		}
		else
		{
			editableConversation = new EditableConversation();
		}
		editableConversation.SaveVersion = saveVersion;
		Event = new UnityEvent();
		ReconstructEditableConversation(editableConversation);
		return editableConversation;
	}

	private void ReconstructEditableConversation(EditableConversation conversation)
	{
		if (conversation == null)
		{
			conversation = new EditableConversation();
		}
		List<EditableConversationNode> list = new List<EditableConversationNode>();
		for (int i = 0; i < conversation.SpeechNodes.Count; i++)
		{
			list.Add(conversation.SpeechNodes[i]);
		}
		for (int j = 0; j < conversation.Options.Count; j++)
		{
			list.Add(conversation.Options[j]);
		}
		for (int k = 0; k < list.Count; k++)
		{
			list[k].parents = new List<EditableConversationNode>();
			if (conversation.SaveVersion <= 103)
			{
				list[k].Connections = new List<EditableConnection>();
				list[k].ParamActions = new List<EditableSetParamAction>();
				if (list[k].NodeType == EditableConversationNode.eNodeType.Speech)
				{
					EditableSpeechNode editableSpeechNode = list[k] as EditableSpeechNode;
					int count = editableSpeechNode.OptionUIDs.Count;
					for (int l = 0; l < count; l++)
					{
						int uid = editableSpeechNode.OptionUIDs[l];
						EditableOptionNode optionByUID = conversation.GetOptionByUID(uid);
						editableSpeechNode.Connections.Add(new EditableOptionConnection(optionByUID));
					}
					int speechUID = editableSpeechNode.SpeechUID;
					EditableSpeechNode speechByUID = conversation.GetSpeechByUID(speechUID);
					if (speechByUID != null)
					{
						editableSpeechNode.Connections.Add(new EditableSpeechConnection(speechByUID));
					}
				}
				else if (list[k] is EditableOptionNode)
				{
					int speechUID2 = (list[k] as EditableOptionNode).SpeechUID;
					EditableSpeechNode speechByUID2 = conversation.GetSpeechByUID(speechUID2);
					if (speechByUID2 != null)
					{
						list[k].Connections.Add(new EditableSpeechConnection(speechByUID2));
					}
				}
				continue;
			}
			for (int m = 0; m < list[k].Connections.Count; m++)
			{
				if (list[k].Connections[m] is EditableSpeechConnection)
				{
					EditableSpeechNode speechByUID3 = conversation.GetSpeechByUID(list[k].Connections[m].NodeUID);
					(list[k].Connections[m] as EditableSpeechConnection).Speech = speechByUID3;
				}
				else if (list[k].Connections[m] is EditableOptionConnection)
				{
					EditableOptionNode optionByUID2 = conversation.GetOptionByUID(list[k].Connections[m].NodeUID);
					(list[k].Connections[m] as EditableOptionConnection).Option = optionByUID2;
				}
			}
		}
		for (int n = 0; n < list.Count; n++)
		{
			EditableConversationNode editableConversationNode = list[n];
			for (int num = 0; num < editableConversationNode.Connections.Count; num++)
			{
				if (editableConversationNode.Connections[num].ConnectionType == EditableConnection.eConnectiontype.Speech)
				{
					(editableConversationNode.Connections[num] as EditableSpeechConnection).Speech.parents.Add(editableConversationNode);
				}
				else if (editableConversationNode.Connections[num].ConnectionType == EditableConnection.eConnectiontype.Option)
				{
					(editableConversationNode.Connections[num] as EditableOptionConnection).Option.parents.Add(editableConversationNode);
				}
			}
		}
	}

	private string Jsonify(EditableConversation conversation)
	{
		if (conversation == null || conversation.Options == null)
		{
			return "";
		}
		MemoryStream memoryStream = new MemoryStream();
		new DataContractJsonSerializer(typeof(EditableConversation)).WriteObject(memoryStream, conversation);
		byte[] array = memoryStream.ToArray();
		memoryStream.Close();
		return Encoding.UTF8.GetString(array, 0, array.Length);
	}

	private EditableConversation Dejsonify()
	{
		if (json == null || json == "")
		{
			return null;
		}
		EditableConversation editableConversation = new EditableConversation();
		MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
		EditableConversation result = new DataContractJsonSerializer(editableConversation.GetType()).ReadObject(memoryStream) as EditableConversation;
		memoryStream.Close();
		return result;
	}

	private Conversation ConstructConversationObject(EditableConversation ec)
	{
		Conversation conversation = new Conversation();
		CreateParameters(ec, conversation);
		conversation.ContinueFont = ContinueFont;
		conversation.EndConversationFont = EndConversationFont;
		Dictionary<int, SpeechNode> dictionary = new Dictionary<int, SpeechNode>();
		Dictionary<int, OptionNode> dictionary2 = new Dictionary<int, OptionNode>();
		for (int i = 0; i < ec.SpeechNodes.Count; i++)
		{
			SpeechNode value = CreateSpeechNode(ec.SpeechNodes[i]);
			dictionary.Add(ec.SpeechNodes[i].ID, value);
		}
		for (int j = 0; j < ec.Options.Count; j++)
		{
			OptionNode value2 = CreateOptionNode(ec.Options[j]);
			dictionary2.Add(ec.Options[j].ID, value2);
		}
		ReconstructTree(ec, conversation, dictionary, dictionary2);
		return conversation;
	}

	private void CreateParameters(EditableConversation ec, Conversation conversation)
	{
		for (int i = 0; i < ec.Parameters.Count; i++)
		{
			if (ec.Parameters[i].ParameterType == EditableParameter.eParamType.Bool)
			{
				EditableBoolParameter editableBoolParameter = ec.Parameters[i] as EditableBoolParameter;
				BoolParameter item = new BoolParameter(editableBoolParameter.ParameterName, editableBoolParameter.BoolValue);
				conversation.Parameters.Add(item);
			}
			else if (ec.Parameters[i].ParameterType == EditableParameter.eParamType.Int)
			{
				EditableIntParameter editableIntParameter = ec.Parameters[i] as EditableIntParameter;
				IntParameter item2 = new IntParameter(editableIntParameter.ParameterName, editableIntParameter.IntValue);
				conversation.Parameters.Add(item2);
			}
		}
	}

	private SpeechNode CreateSpeechNode(EditableSpeechNode editableNode)
	{
		SpeechNode speechNode = new SpeechNode();
		speechNode.Name = editableNode.Name;
		speechNode.Text = editableNode.Text;
		speechNode.AutomaticallyAdvance = editableNode.AdvanceDialogueAutomatically;
		speechNode.AutoAdvanceShouldDisplayOption = editableNode.AutoAdvanceShouldDisplayOption;
		speechNode.TimeUntilAdvance = editableNode.TimeUntilAdvance;
		speechNode.TMPFont = editableNode.TMPFont;
		speechNode.Icon = editableNode.Icon;
		speechNode.Audio = editableNode.Audio;
		speechNode.Volume = editableNode.Volume;
		CopyParamActions(editableNode, speechNode);
		NodeEventHolder nodeData = GetNodeData(editableNode.ID);
		if (nodeData != null)
		{
			speechNode.Event = nodeData.Event;
		}
		return speechNode;
	}

	private OptionNode CreateOptionNode(EditableOptionNode editableNode)
	{
		OptionNode optionNode = new OptionNode();
		optionNode.Text = editableNode.Text;
		optionNode.TMPFont = editableNode.TMPFont;
		CopyParamActions(editableNode, optionNode);
		NodeEventHolder nodeData = GetNodeData(editableNode.ID);
		if (nodeData != null)
		{
			optionNode.Event = nodeData.Event;
		}
		return optionNode;
	}

	public void CopyParamActions(EditableConversationNode editable, ConversationNode node)
	{
		node.ParamActions = new List<SetParamAction>();
		for (int i = 0; i < editable.ParamActions.Count; i++)
		{
			if (editable.ParamActions[i].ParamActionType == EditableSetParamAction.eParamActionType.Int)
			{
				EditableSetIntParamAction editableSetIntParamAction = editable.ParamActions[i] as EditableSetIntParamAction;
				SetIntParamAction setIntParamAction = new SetIntParamAction();
				setIntParamAction.ParameterName = editableSetIntParamAction.ParameterName;
				setIntParamAction.Value = editableSetIntParamAction.Value;
				node.ParamActions.Add(setIntParamAction);
			}
			else if (editable.ParamActions[i].ParamActionType == EditableSetParamAction.eParamActionType.Bool)
			{
				EditableSetBoolParamAction editableSetBoolParamAction = editable.ParamActions[i] as EditableSetBoolParamAction;
				SetBoolParamAction setBoolParamAction = new SetBoolParamAction();
				setBoolParamAction.ParameterName = editableSetBoolParamAction.ParameterName;
				setBoolParamAction.Value = editableSetBoolParamAction.Value;
				node.ParamActions.Add(setBoolParamAction);
			}
		}
	}

	private void ReconstructTree(EditableConversation ec, Conversation conversation, Dictionary<int, SpeechNode> dialogues, Dictionary<int, OptionNode> options)
	{
		List<EditableSpeechNode> speechNodes = ec.SpeechNodes;
		for (int i = 0; i < speechNodes.Count; i++)
		{
			EditableSpeechNode editableSpeechNode = speechNodes[i];
			SpeechNode speechNode = dialogues[editableSpeechNode.ID];
			List<EditableConnection> connections = editableSpeechNode.Connections;
			for (int j = 0; j < connections.Count; j++)
			{
				int nodeUID = connections[j].NodeUID;
				if (connections[j].ConnectionType == EditableConnection.eConnectiontype.Speech)
				{
					SpeechConnection speechConnection = new SpeechConnection(dialogues[nodeUID]);
					CopyConnectionConditions(connections[j], speechConnection);
					speechNode.Connections.Add(speechConnection);
				}
				else if (connections[j].ConnectionType == EditableConnection.eConnectiontype.Option)
				{
					OptionConnection optionConnection = new OptionConnection(options[nodeUID]);
					CopyConnectionConditions(connections[j], optionConnection);
					speechNode.Connections.Add(optionConnection);
				}
			}
			if (editableSpeechNode.EditorInfo.isRoot)
			{
				conversation.Root = dialogues[editableSpeechNode.ID];
			}
		}
		List<EditableOptionNode> options2 = ec.Options;
		for (int k = 0; k < options2.Count; k++)
		{
			EditableOptionNode editableOptionNode = options2[k];
			OptionNode optionNode = options[editableOptionNode.ID];
			List<EditableConnection> connections2 = editableOptionNode.Connections;
			for (int l = 0; l < connections2.Count; l++)
			{
				int nodeUID2 = connections2[l].NodeUID;
				if (connections2[l].ConnectionType == EditableConnection.eConnectiontype.Speech)
				{
					SpeechConnection speechConnection2 = new SpeechConnection(dialogues[nodeUID2]);
					CopyConnectionConditions(connections2[l], speechConnection2);
					optionNode.Connections.Add(speechConnection2);
				}
			}
		}
	}

	private void CopyConnectionConditions(EditableConnection editableConnection, Connection connection)
	{
		List<EditableCondition> conditions = editableConnection.Conditions;
		for (int i = 0; i < conditions.Count; i++)
		{
			if (conditions[i].ConditionType == EditableCondition.eConditionType.BoolCondition)
			{
				EditableBoolCondition editableBoolCondition = conditions[i] as EditableBoolCondition;
				BoolCondition boolCondition = new BoolCondition();
				boolCondition.ParameterName = editableBoolCondition.ParameterName;
				switch (editableBoolCondition.CheckType)
				{
				case EditableBoolCondition.eCheckType.equal:
					boolCondition.CheckType = BoolCondition.eCheckType.equal;
					break;
				case EditableBoolCondition.eCheckType.notEqual:
					boolCondition.CheckType = BoolCondition.eCheckType.notEqual;
					break;
				}
				boolCondition.RequiredValue = editableBoolCondition.RequiredValue;
				connection.Conditions.Add(boolCondition);
			}
			else if (conditions[i].ConditionType == EditableCondition.eConditionType.IntCondition)
			{
				EditableIntCondition editableIntCondition = conditions[i] as EditableIntCondition;
				IntCondition intCondition = new IntCondition();
				intCondition.ParameterName = editableIntCondition.ParameterName;
				switch (editableIntCondition.CheckType)
				{
				case EditableIntCondition.eCheckType.equal:
					intCondition.CheckType = IntCondition.eCheckType.equal;
					break;
				case EditableIntCondition.eCheckType.greaterThan:
					intCondition.CheckType = IntCondition.eCheckType.greaterThan;
					break;
				case EditableIntCondition.eCheckType.lessThan:
					intCondition.CheckType = IntCondition.eCheckType.lessThan;
					break;
				}
				intCondition.RequiredValue = editableIntCondition.RequiredValue;
				connection.Conditions.Add(intCondition);
			}
		}
	}
}
