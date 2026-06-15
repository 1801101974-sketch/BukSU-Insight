using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
	[SerializeField]
	private Slider soundSlider;

	[SerializeField]
	private AudioMixer masterMixer;

	private void Start()
	{
		SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100f));
	}

	public void SetVolume(float _value)
	{
		if (_value < 1f)
		{
			_value = 0.001f;
		}
		RefreshSlider(_value);
		PlayerPrefs.SetFloat("SavedMasterVolume", _value);
		masterMixer.SetFloat("MasterVolume", Mathf.Log10(_value / 100f) * 20f);
	}

	public void SetVolumeFromSlider()
	{
		SetVolume(soundSlider.value);
	}

	public void RefreshSlider(float _value)
	{
		soundSlider.value = _value;
	}
}
