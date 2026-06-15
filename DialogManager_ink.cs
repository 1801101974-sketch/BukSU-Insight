using System.Collections;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager_ink : MonoBehaviour
{
	public TextAsset inkJSONAsset;

	private Story story;

	public Text dialogueText;

	public Button[] optionButtons;

	public GameObject dialoguePanel;

	public Button startDialogueButton;

	public float typingSpeed = 0.02f;

	public GameObject UIPanel;

	public GameObject UIScene;

	private void Start()
	{
		dialoguePanel.SetActive(value: false);
		startDialogueButton.onClick.AddListener(StartDialogue);
	}

	private void StartDialogue()
	{
		dialoguePanel.SetActive(value: true);
		StartStory();
	}

	private void StartStory()
	{
		story = new Story(inkJSONAsset.text);
		RefreshView();
	}

	private void RefreshView()
	{
		dialogueText.text = "";
		Button[] array = optionButtons;
		foreach (Button obj in array)
		{
			obj.gameObject.SetActive(value: false);
			obj.onClick.RemoveAllListeners();
		}
		StartCoroutine(DisplayNextLineWithTypingEffect());
	}

	private IEnumerator DisplayNextLineWithTypingEffect()
	{
		if (story.canContinue)
		{
			string text = story.Continue();
			yield return StartCoroutine(TypeText(text));
		}
		if (story.currentChoices.Count > 0)
		{
			for (int i = 0; i < story.currentChoices.Count; i++)
			{
				Choice choice = story.currentChoices[i];
				optionButtons[i].gameObject.SetActive(value: true);
				optionButtons[i].GetComponentInChildren<Text>().text = choice.text;
				int choiceIndex = i;
				optionButtons[i].onClick.AddListener(delegate
				{
					OnClickChoiceButton(choiceIndex);
				});
			}
		}
		else
		{
			dialogueText.text += "\n\n<End of conversation>";
			EndDialogue();
		}
	}

	private IEnumerator TypeText(string text)
	{
		dialogueText.text = "";
		char[] array = text.ToCharArray();
		foreach (char c in array)
		{
			dialogueText.text += c;
			yield return new WaitForSeconds(typingSpeed);
		}
	}

	private void OnClickChoiceButton(int choiceIndex)
	{
		story.ChooseChoiceIndex(choiceIndex);
		RefreshView();
	}

	private void EndDialogue()
	{
		dialoguePanel.SetActive(value: false);
		UIPanel.SetActive(value: true);
		UIScene.SetActive(value: true);
	}
}
