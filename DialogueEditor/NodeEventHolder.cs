using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueEditor;

[Serializable]
public class NodeEventHolder : MonoBehaviour
{
	[SerializeField]
	public UnityEvent Event;

	[SerializeField]
	public int NodeID;

	[SerializeField]
	public TMP_FontAsset TMPFont;

	[SerializeField]
	public Sprite Icon;

	[SerializeField]
	public AudioClip Audio;
}
