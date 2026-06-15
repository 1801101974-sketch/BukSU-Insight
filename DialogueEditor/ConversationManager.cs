using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueEditor;

public class ConversationManager : MonoBehaviour
{
	private enum eState
	{
		TransitioningDialogueBoxOn,
		ScrollingText,
		TransitioningOptionsOn,
		Idle,
		TransitioningOptionsOff,
		TransitioningDialogueOff,
		Off,
		NONE
	}

	public delegate void ConversationStartEvent();

	public delegate void ConversationEndEvent();

	private const float TRANSITION_TIME = 0.2f;

	public static ConversationStartEvent OnConversationStarted;

	public static ConversationEndEvent OnConversationEnded;

	public bool ScrollText;

	public float ScrollSpeed = 1f;

	public Sprite BackgroundImage;

	public bool BackgroundImageSliced;

	public Sprite OptionImage;

	public bool OptionImageSliced;

	public bool AllowMouseInteraction;

	public RectTransform DialoguePanel;

	public RectTransform OptionsPanel;

	public Image DialogueBackground;

	public Image NpcIcon;

	public TextMeshProUGUI NameText;

	public TextMeshProUGUI DialogueText;

	public AudioSource AudioPlayer;

	public UIConversationButton ButtonPrefab;

	public Sprite BlankSprite;

	private float m_elapsedScrollTime;

	private int m_scrollIndex;

	public int m_targetScrollTextCount;

	private eState m_state;

	private float m_stateTime;

	private Conversation m_conversation;

	private SpeechNode m_currentSpeech;

	private OptionNode m_selectedOption;

	private List<UIConversationButton> m_uiOptions;

	private int m_currentSelectedIndex;

	public static ConversationManager Instance { get; private set; }

	public bool IsConversationActive
	{
		get
		{
			if (m_state != eState.NONE)
			{
				return m_state != eState.Off;
			}
			return false;
		}
	}

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		Instance = this;
		m_uiOptions = new List<UIConversationButton>();
		NpcIcon.sprite = BlankSprite;
		DialogueText.text = "";
		TurnOffUI();
	}

	private void OnDestroy()
	{
		Instance = null;
	}

	private void Update()
	{
		switch (m_state)
		{
		case eState.TransitioningDialogueBoxOn:
			TransitioningDialogueBoxOn_Update();
			break;
		case eState.ScrollingText:
			ScrollingText_Update();
			break;
		case eState.TransitioningOptionsOn:
			TransitionOptionsOn_Update();
			break;
		case eState.Idle:
			Idle_Update();
			break;
		case eState.TransitioningOptionsOff:
			TransitionOptionsOff_Update();
			break;
		case eState.TransitioningDialogueOff:
			TransitioningDialogueBoxOff_Update();
			break;
		}
	}

	public void StartConversation(NPCConversation conversation)
	{
		m_conversation = conversation.Deserialize();
		if (OnConversationStarted != null)
		{
			OnConversationStarted();
		}
		TurnOnUI();
		m_currentSpeech = m_conversation.Root;
		SetState(eState.TransitioningDialogueBoxOn);
	}

	public void EndConversation()
	{
		SetState(eState.TransitioningDialogueOff);
		if (OnConversationEnded != null)
		{
			OnConversationEnded();
		}
	}

	public void SelectNextOption()
	{
		int num = m_currentSelectedIndex + 1;
		if (num > m_uiOptions.Count - 1)
		{
			num = 0;
		}
		SetSelectedOption(num);
	}

	public void SelectPreviousOption()
	{
		int num = m_currentSelectedIndex - 1;
		if (num < 0)
		{
			num = m_uiOptions.Count - 1;
		}
		SetSelectedOption(num);
	}

	public void PressSelectedOption()
	{
		if (m_state == eState.Idle && m_currentSelectedIndex >= 0 && m_currentSelectedIndex < m_uiOptions.Count && m_uiOptions.Count != 0)
		{
			m_uiOptions[m_currentSelectedIndex].OnButtonPressed();
		}
	}

	public void AlertHover(UIConversationButton button)
	{
		for (int i = 0; i < m_uiOptions.Count; i++)
		{
			if (m_uiOptions[i] == button && m_currentSelectedIndex != i)
			{
				SetSelectedOption(i);
				return;
			}
		}
		if (button == null)
		{
			UnselectOption();
		}
	}

	public void SetInt(string paramName, int value)
	{
		m_conversation.SetInt(paramName, value, out var status);
		if (status == eParamStatus.NoParamFound)
		{
			LogWarning("parameter '" + paramName + "' does not exist.");
		}
	}

	public void SetBool(string paramName, bool value)
	{
		m_conversation.SetBool(paramName, value, out var status);
		if (status == eParamStatus.NoParamFound)
		{
			LogWarning("parameter '" + paramName + "' does not exist.");
		}
	}

	public int GetInt(string paramName)
	{
		eParamStatus status;
		int result = m_conversation.GetInt(paramName, out status);
		if (status == eParamStatus.NoParamFound)
		{
			LogWarning("parameter '" + paramName + "' does not exist.");
		}
		return result;
	}

	public bool GetBool(string paramName)
	{
		eParamStatus status;
		bool result = m_conversation.GetBool(paramName, out status);
		if (status == eParamStatus.NoParamFound)
		{
			LogWarning("parameter '" + paramName + "' does not exist.");
		}
		return result;
	}

	private void SetState(eState newState)
	{
		switch (m_state)
		{
		case eState.TransitioningOptionsOff:
			m_selectedOption = null;
			break;
		case eState.TransitioningDialogueBoxOn:
			SetColorAlpha(DialogueBackground, 1f);
			SetColorAlpha(NpcIcon, 1f);
			SetColorAlpha(NameText, 1f);
			break;
		}
		m_state = newState;
		m_stateTime = 0f;
		switch (m_state)
		{
		case eState.TransitioningDialogueBoxOn:
			SetColorAlpha(DialogueBackground, 0f);
			SetColorAlpha(NpcIcon, 0f);
			SetColorAlpha(NameText, 0f);
			DialogueText.text = "";
			NameText.text = m_currentSpeech.Name;
			NpcIcon.sprite = ((m_currentSpeech.Icon != null) ? m_currentSpeech.Icon : BlankSprite);
			break;
		case eState.ScrollingText:
			SetColorAlpha(DialogueText, 1f);
			break;
		case eState.TransitioningOptionsOn:
		{
			SetColorAlpha(DialogueText, 1f);
			CreateUIOptions();
			for (int i = 0; i < m_uiOptions.Count; i++)
			{
				m_uiOptions[i].gameObject.SetActive(value: true);
			}
			break;
		}
		}
	}

	private void TransitioningDialogueBoxOn_Update()
	{
		m_stateTime += Time.deltaTime;
		float num = m_stateTime / 0.2f;
		if (num > 1f)
		{
			SetupSpeech(m_currentSpeech);
			return;
		}
		SetColorAlpha(DialogueBackground, num);
		SetColorAlpha(NpcIcon, num);
		SetColorAlpha(NameText, num);
	}

	private void ScrollingText_Update()
	{
		float num = 0.04f;
		num *= ScrollSpeed;
		m_elapsedScrollTime += Time.deltaTime;
		if (m_elapsedScrollTime > num)
		{
			m_elapsedScrollTime = 0f;
			DialogueText.maxVisibleCharacters = m_scrollIndex;
			m_scrollIndex++;
			if (m_scrollIndex >= m_targetScrollTextCount)
			{
				SetState(eState.TransitioningOptionsOn);
			}
		}
	}

	private void TransitionOptionsOn_Update()
	{
		m_stateTime += Time.deltaTime;
		float num = m_stateTime / 0.2f;
		if (num > 1f)
		{
			SetState(eState.Idle);
			return;
		}
		for (int i = 0; i < m_uiOptions.Count; i++)
		{
			m_uiOptions[i].SetAlpha(num);
		}
	}

	private void Idle_Update()
	{
		m_stateTime += Time.deltaTime;
		if (m_currentSpeech.AutomaticallyAdvance && (m_currentSpeech.ConnectionType == Connection.eConnectionType.None || m_currentSpeech.ConnectionType == Connection.eConnectionType.Speech) && m_stateTime > m_currentSpeech.TimeUntilAdvance)
		{
			SetState(eState.TransitioningOptionsOff);
		}
	}

	private void TransitionOptionsOff_Update()
	{
		m_stateTime += Time.deltaTime;
		float num = m_stateTime / 0.2f;
		if (num > 1f)
		{
			ClearOptions();
			if (m_currentSpeech.AutomaticallyAdvance && IsAutoAdvance())
			{
				return;
			}
			if (m_selectedOption == null)
			{
				EndConversation();
				return;
			}
			SpeechNode validSpeechOfNode = GetValidSpeechOfNode(m_selectedOption);
			if (validSpeechOfNode == null)
			{
				EndConversation();
			}
			else
			{
				SetupSpeech(validSpeechOfNode);
			}
		}
		else
		{
			for (int i = 0; i < m_uiOptions.Count; i++)
			{
				m_uiOptions[i].SetAlpha(1f - num);
			}
			SetColorAlpha(DialogueText, 1f - num);
		}
	}

	private void TransitioningDialogueBoxOff_Update()
	{
		m_stateTime += Time.deltaTime;
		float num = m_stateTime / 0.2f;
		if (num > 1f)
		{
			TurnOffUI();
			return;
		}
		SetColorAlpha(DialogueBackground, 1f - num);
		SetColorAlpha(NpcIcon, 1f - num);
		SetColorAlpha(NameText, 1f - num);
	}

	private void SetupSpeech(SpeechNode speech)
	{
		if (speech == null)
		{
			EndConversation();
			return;
		}
		m_currentSpeech = speech;
		ClearOptions();
		m_currentSelectedIndex = 0;
		if (speech.Icon == null)
		{
			NpcIcon.sprite = BlankSprite;
		}
		else
		{
			NpcIcon.sprite = speech.Icon;
		}
		if (speech.TMPFont != null)
		{
			DialogueText.font = speech.TMPFont;
		}
		else
		{
			DialogueText.font = null;
		}
		NameText.text = speech.Name;
		if (string.IsNullOrEmpty(speech.Text))
		{
			if (ScrollText)
			{
				DialogueText.text = "";
				m_targetScrollTextCount = 0;
				DialogueText.maxVisibleCharacters = 0;
				m_elapsedScrollTime = 0f;
				m_scrollIndex = 0;
			}
			else
			{
				DialogueText.text = "";
				DialogueText.maxVisibleCharacters = 1;
			}
		}
		else if (ScrollText)
		{
			DialogueText.text = speech.Text;
			m_targetScrollTextCount = speech.Text.Length + 1;
			DialogueText.maxVisibleCharacters = 0;
			m_elapsedScrollTime = 0f;
			m_scrollIndex = 0;
		}
		else
		{
			DialogueText.text = speech.Text;
			DialogueText.maxVisibleCharacters = speech.Text.Length;
		}
		if (speech.Event != null)
		{
			speech.Event.Invoke();
		}
		DoParamAction(speech);
		if (speech.Audio != null)
		{
			AudioPlayer.clip = speech.Audio;
			AudioPlayer.volume = speech.Volume;
			AudioPlayer.Play();
		}
		if (ScrollText)
		{
			SetState(eState.ScrollingText);
		}
		else
		{
			SetState(eState.TransitioningOptionsOn);
		}
	}

	public void SpeechSelected(SpeechNode speech)
	{
		SetupSpeech(speech);
	}

	public void OptionSelected(OptionNode option)
	{
		m_selectedOption = option;
		DoParamAction(option);
		if (option.Event != null)
		{
			option.Event.Invoke();
		}
		SetState(eState.TransitioningOptionsOff);
	}

	public void EndButtonSelected()
	{
		m_selectedOption = null;
		SetState(eState.TransitioningOptionsOff);
	}

	private bool IsAutoAdvance()
	{
		if (m_currentSpeech.ConnectionType == Connection.eConnectionType.Speech)
		{
			SpeechNode validSpeechOfNode = GetValidSpeechOfNode(m_currentSpeech);
			if (validSpeechOfNode != null)
			{
				SetupSpeech(validSpeechOfNode);
				return true;
			}
		}
		else if (m_currentSpeech.ConnectionType == Connection.eConnectionType.None)
		{
			EndConversation();
			return true;
		}
		return false;
	}

	private SpeechNode GetValidSpeechOfNode(ConversationNode parentNode)
	{
		if (parentNode.ConnectionType != Connection.eConnectionType.Speech)
		{
			return null;
		}
		if (parentNode.Connections.Count == 0)
		{
			return null;
		}
		for (int i = 0; i < parentNode.Connections.Count; i++)
		{
			SpeechConnection speechConnection = parentNode.Connections[i] as SpeechConnection;
			if (ConditionsMet(speechConnection))
			{
				return speechConnection.SpeechNode;
			}
		}
		return null;
	}

	private void TurnOnUI()
	{
		DialoguePanel.gameObject.SetActive(value: true);
		OptionsPanel.gameObject.SetActive(value: true);
		if (BackgroundImage != null)
		{
			DialogueBackground.sprite = BackgroundImage;
			if (BackgroundImageSliced)
			{
				DialogueBackground.type = Image.Type.Sliced;
			}
			else
			{
				DialogueBackground.type = Image.Type.Simple;
			}
		}
		NpcIcon.sprite = BlankSprite;
	}

	private void TurnOffUI()
	{
		DialoguePanel.gameObject.SetActive(value: false);
		OptionsPanel.gameObject.SetActive(value: false);
		SetState(eState.Off);
	}

	private void CreateUIOptions()
	{
		if (m_currentSpeech.ConnectionType == Connection.eConnectionType.Option)
		{
			for (int i = 0; i < m_currentSpeech.Connections.Count; i++)
			{
				OptionConnection optionConnection = m_currentSpeech.Connections[i] as OptionConnection;
				if (ConditionsMet(optionConnection))
				{
					CreateButton().SetupButton(UIConversationButton.eButtonType.Option, optionConnection.OptionNode);
				}
			}
		}
		else
		{
			bool num = !m_currentSpeech.AutomaticallyAdvance;
			bool flag = m_currentSpeech.AutomaticallyAdvance && m_currentSpeech.AutoAdvanceShouldDisplayOption;
			if (num || flag)
			{
				if (m_currentSpeech.ConnectionType == Connection.eConnectionType.Speech)
				{
					UIConversationButton uIConversationButton = CreateButton();
					SpeechNode validSpeechOfNode = GetValidSpeechOfNode(m_currentSpeech);
					if (validSpeechOfNode == null)
					{
						uIConversationButton.SetupButton(UIConversationButton.eButtonType.End, null, null, m_conversation.EndConversationFont);
					}
					else
					{
						uIConversationButton.SetupButton(UIConversationButton.eButtonType.Speech, validSpeechOfNode, m_conversation.ContinueFont);
					}
				}
				else if (m_currentSpeech.ConnectionType == Connection.eConnectionType.None)
				{
					CreateButton().SetupButton(UIConversationButton.eButtonType.End, null, null, m_conversation.EndConversationFont);
				}
			}
		}
		SetSelectedOption(0);
		for (int j = 0; j < m_uiOptions.Count; j++)
		{
			m_uiOptions[j].SetImage(OptionImage, OptionImageSliced);
			m_uiOptions[j].SetAlpha(0f);
			m_uiOptions[j].gameObject.SetActive(value: false);
		}
	}

	private void ClearOptions()
	{
		while (m_uiOptions.Count != 0)
		{
			Object.Destroy(m_uiOptions[0].gameObject);
			m_uiOptions.RemoveAt(0);
		}
	}

	private void SetColorAlpha(MaskableGraphic graphic, float a)
	{
		Color color = graphic.color;
		color.a = a;
		graphic.color = color;
	}

	private void SetSelectedOption(int index)
	{
		if (m_uiOptions.Count != 0)
		{
			if (index < 0)
			{
				index = 0;
			}
			if (index > m_uiOptions.Count - 1)
			{
				index = m_uiOptions.Count - 1;
			}
			if (m_currentSelectedIndex >= 0)
			{
				m_uiOptions[m_currentSelectedIndex].SetHovering(selected: false);
			}
			m_currentSelectedIndex = index;
			m_uiOptions[index].SetHovering(selected: true);
		}
	}

	private void UnselectOption()
	{
		if (m_currentSelectedIndex >= 0)
		{
			m_uiOptions[m_currentSelectedIndex].SetHovering(selected: false);
			m_currentSelectedIndex = -1;
		}
	}

	private UIConversationButton CreateButton()
	{
		UIConversationButton uIConversationButton = Object.Instantiate(ButtonPrefab, OptionsPanel);
		m_uiOptions.Add(uIConversationButton);
		return uIConversationButton;
	}

	private bool ConditionsMet(Connection connection)
	{
		List<Condition> conditions = connection.Conditions;
		for (int i = 0; i < conditions.Count; i++)
		{
			bool flag = false;
			if (conditions[i].ConditionType == Condition.eConditionType.IntCondition)
			{
				IntCondition obj = conditions[i] as IntCondition;
				string parameterName = obj.ParameterName;
				int requiredValue = obj.RequiredValue;
				eParamStatus status;
				int num = m_conversation.GetInt(parameterName, out status);
				switch (obj.CheckType)
				{
				case IntCondition.eCheckType.equal:
					flag = num == requiredValue;
					break;
				case IntCondition.eCheckType.greaterThan:
					flag = num > requiredValue;
					break;
				case IntCondition.eCheckType.lessThan:
					flag = num < requiredValue;
					break;
				}
			}
			if (conditions[i].ConditionType == Condition.eConditionType.BoolCondition)
			{
				BoolCondition obj2 = conditions[i] as BoolCondition;
				string parameterName2 = obj2.ParameterName;
				bool requiredValue2 = obj2.RequiredValue;
				eParamStatus status2;
				bool flag2 = m_conversation.GetBool(parameterName2, out status2);
				switch (obj2.CheckType)
				{
				case BoolCondition.eCheckType.equal:
					flag = flag2 == requiredValue2;
					break;
				case BoolCondition.eCheckType.notEqual:
					flag = flag2 != requiredValue2;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	public void DoParamAction(ConversationNode node)
	{
		if (node.ParamActions == null)
		{
			return;
		}
		for (int i = 0; i < node.ParamActions.Count; i++)
		{
			string parameterName = node.ParamActions[i].ParameterName;
			if (node.ParamActions[i].ParamActionType == SetParamAction.eParamActionType.Int)
			{
				int value = (node.ParamActions[i] as SetIntParamAction).Value;
				SetInt(parameterName, value);
			}
			else if (node.ParamActions[i].ParamActionType == SetParamAction.eParamActionType.Bool)
			{
				bool value2 = (node.ParamActions[i] as SetBoolParamAction).Value;
				SetBool(parameterName, value2);
			}
		}
	}

	private void LogWarning(string warning)
	{
	}
}
