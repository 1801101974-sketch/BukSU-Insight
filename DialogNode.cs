using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog Node", menuName = "Dialog System/Dialog Node")]
public class DialogNode : ScriptableObject
{
	public string dialogText;

	public string speaker;

	public List<Response> responses;
}
