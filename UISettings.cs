using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
	[Header("Audio Settings")]
	public AudioMixer audioMixer;

	public Slider volumeSlider;

	[Header("Resolution Settings")]
	public TMP_Dropdown resolutionDropdown;

	private Resolution[] resolutions;

	[Header("Quality Settings")]
	public TMP_Dropdown qualityDropdown;

	private void Start()
	{
		InitializeAudioSettings();
	}

	private void InitializeAudioSettings()
	{
		if (audioMixer == null || volumeSlider == null)
		{
			Debug.LogError("AudioMixer or Volume Slider is not assigned!");
			return;
		}
		float num;
		if (PlayerPrefs.HasKey("Volume"))
		{
			num = PlayerPrefs.GetFloat("Volume");
		}
		else
		{
			num = 0.75f;
			PlayerPrefs.SetFloat("Volume", num);
		}
		volumeSlider.value = num;
		SetVolume(num);
	}

	public void SetVolume(float volume)
	{
		if (audioMixer != null)
		{
			audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20f);
			PlayerPrefs.SetFloat("Volume", volume);
			PlayerPrefs.Save();
		}
		else
		{
			Debug.LogError("AudioMixer is not assigned!");
		}
	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}

	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
	}

	public void SetFullscreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}
}
