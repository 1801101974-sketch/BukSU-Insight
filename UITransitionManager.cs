using System.Collections;
using UnityEngine;

public class UITransitionManager : MonoBehaviour
{
	public GameObject currentPanel;

	public GameObject nextPanel;

	public float delay = 5f;

	public void OnButtonClick()
	{
		StartCoroutine(TransitionToNextPanel());
	}

	private IEnumerator TransitionToNextPanel()
	{
		Debug.Log("Delay started...");
		yield return new WaitForSeconds(delay);
		if (currentPanel != null)
		{
			currentPanel.SetActive(value: false);
		}
		if (nextPanel != null)
		{
			nextPanel.SetActive(value: true);
		}
		Debug.Log("Transition completed.");
	}
}
