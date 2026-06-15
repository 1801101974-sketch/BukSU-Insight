using UnityEngine;

[CreateAssetMenu(fileName = "ActiveState", menuName = "Game/Active State")]
public class ActiveStateSO : ScriptableObject
{
	[Tooltip("Controls whether the object should be active or not")]
	public bool isActive;
}
