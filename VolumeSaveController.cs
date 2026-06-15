using UnityEngine;
using UnityEngine.UI;

public class VolumeSaveController : MonoBehaviour
{
	[SerializeField]
	private Slider volumeSlider;

	[SerializeField]
	private Text volumeTextUI;

	public void VolumeSlider(float volume)
	{
		volumeTextUI.text = volume.ToString(".100");
	}
}
