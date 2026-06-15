using UnityEngine;

[CreateAssetMenu(fileName = "New Response", menuName = "Dialog System/Response")]
public class Response : ScriptableObject
{
	public string responseText;

	public DialogNode nextDialogNode;
}
