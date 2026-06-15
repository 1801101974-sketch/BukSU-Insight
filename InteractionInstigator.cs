using System.Collections.Generic;
using UnityEngine;

public class InteractionInstigator : MonoBehaviour
{
	private List<Interactable> m_NearbyInteractables = new List<Interactable>();

	public bool HasNearbyInteractables()
	{
		return m_NearbyInteractables.Count != 0;
	}

	private void Update()
	{
		if (HasNearbyInteractables() && Input.GetButtonDown("Submit"))
		{
			m_NearbyInteractables[0].DoInteraction();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Interactable component = other.GetComponent<Interactable>();
		if (component != null)
		{
			m_NearbyInteractables.Add(component);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Interactable component = other.GetComponent<Interactable>();
		if (component != null)
		{
			m_NearbyInteractables.Remove(component);
		}
	}
}
