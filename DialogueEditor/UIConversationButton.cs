using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueEditor;

public class UIConversationButton : MonoBehaviour
{
	public enum eHoverState
	{
		idleOff,
		animatingOn,
		idleOn,
		animatingOff
	}

	public enum eButtonType
	{
		Option,
		Speech,
		End
	}

	[SerializeField]
	private TextMeshProUGUI TextMesh;

	[SerializeField]
	private Image OptionBackgroundImage;

	private RectTransform m_rect;

	private eButtonType m_buttonType;

	private ConversationNode m_node;

	private float m_hoverT;

	private eHoverState m_hoverState;

	public eButtonType ButtonType => m_buttonType;

	private bool Hovering
	{
		get
		{
			if (m_hoverState != eHoverState.animatingOn)
			{
				return m_hoverState == eHoverState.animatingOff;
			}
			return true;
		}
	}

	private Vector3 BigSize => Vector3.one * 1.2f;

	private void Awake()
	{
		m_rect = GetComponent<RectTransform>();
	}

	private void Update()
	{
		if (Hovering)
		{
			m_hoverT += Time.deltaTime;
			float num = m_hoverT / 0.2f;
			bool flag = false;
			if (num >= 1f)
			{
				num = 1f;
				flag = true;
			}
			Vector3 localScale = Vector3.one;
			float t = EaseOutQuart(num);
			switch (m_hoverState)
			{
			case eHoverState.animatingOn:
				localScale = Vector3.Lerp(Vector3.one, BigSize, t);
				break;
			case eHoverState.animatingOff:
				localScale = Vector3.Lerp(BigSize, Vector3.one, t);
				break;
			}
			m_rect.localScale = localScale;
			if (flag)
			{
				m_hoverState = ((m_hoverState == eHoverState.animatingOn) ? eHoverState.idleOn : eHoverState.idleOff);
			}
		}
	}

	public void OnHover(bool hovering)
	{
		if (ConversationManager.Instance.AllowMouseInteraction)
		{
			if (hovering)
			{
				ConversationManager.Instance.AlertHover(this);
			}
			else
			{
				ConversationManager.Instance.AlertHover(null);
			}
		}
	}

	public void OnClick()
	{
		if (ConversationManager.Instance.AllowMouseInteraction)
		{
			DoClickBehaviour();
		}
	}

	public void OnButtonPressed()
	{
		DoClickBehaviour();
	}

	public void SetHovering(bool selected)
	{
		if ((!selected || (m_hoverState != eHoverState.animatingOn && m_hoverState != eHoverState.idleOn)) && (selected || (m_hoverState != eHoverState.animatingOff && m_hoverState != eHoverState.idleOff)))
		{
			if (selected)
			{
				m_hoverState = eHoverState.animatingOn;
			}
			else
			{
				m_hoverState = eHoverState.animatingOff;
			}
			m_hoverT = 0f;
		}
	}

	public void SetImage(Sprite sprite, bool sliced)
	{
		if (sprite != null)
		{
			OptionBackgroundImage.sprite = sprite;
			if (sliced)
			{
				OptionBackgroundImage.type = Image.Type.Sliced;
			}
			else
			{
				OptionBackgroundImage.type = Image.Type.Simple;
			}
		}
	}

	public void InitButton(OptionNode option)
	{
		if (option.TMPFont != null)
		{
			TextMesh.font = option.TMPFont;
		}
		else
		{
			TextMesh.font = null;
		}
	}

	public void SetAlpha(float a)
	{
		Color color = OptionBackgroundImage.color;
		Color color2 = TextMesh.color;
		color.a = a;
		color2.a = a;
		OptionBackgroundImage.color = color;
		TextMesh.color = color2;
	}

	public void SetupButton(eButtonType buttonType, ConversationNode node, TMP_FontAsset continueFont = null, TMP_FontAsset endFont = null)
	{
		m_buttonType = buttonType;
		m_node = node;
		switch (m_buttonType)
		{
		case eButtonType.Option:
			TextMesh.text = node.Text;
			TextMesh.font = node.TMPFont;
			break;
		case eButtonType.Speech:
			TextMesh.text = "Continue.";
			TextMesh.font = continueFont;
			break;
		case eButtonType.End:
			TextMesh.text = "End.";
			TextMesh.font = endFont;
			break;
		}
	}

	private void DoClickBehaviour()
	{
		switch (m_buttonType)
		{
		case eButtonType.Speech:
			ConversationManager.Instance.SpeechSelected(m_node as SpeechNode);
			break;
		case eButtonType.Option:
			ConversationManager.Instance.OptionSelected(m_node as OptionNode);
			break;
		case eButtonType.End:
			ConversationManager.Instance.EndButtonSelected();
			break;
		}
	}

	private static float EaseOutQuart(float normalized)
	{
		return 1f - Mathf.Pow(1f - normalized, 4f);
	}
}
