using UnityEngine;
using UnityEngine.UI;

public class Collect : MonoBehaviour
{
	public GameObject CollectButton;

	public Text counterText;

	public GameObject targetGameObject;

	private int collectCount;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			CollectButton.SetActive(value: true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			CollectButton.SetActive(value: false);
		}
	}

	public void OnCollectButtonTap()
	{
		IncreaseCounter();
	}

	private void IncreaseCounter()
	{
		collectCount++;
		UpdateCounterText();
		if (collectCount == 12 && targetGameObject != null)
		{
			targetGameObject.SetActive(value: true);
		}
	}

	private void UpdateCounterText()
	{
		if (counterText != null)
		{
			counterText.text = "Collected: " + collectCount;
		}
	}
}
