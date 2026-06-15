using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public bool isMale;

	public InputField nameInputField;

	public PlayerData playerData;

	public CharacterSettings characterSettings;

	public CharacterSpawner characterSpawner;

	public void SelectMale()
	{
		isMale = true;
	}

	public void SelectFemale()
	{
		isMale = false;
	}

	public void ProceedToNextScene()
	{
		characterSettings.isMale = isMale;
		characterSpawner.characterSettings = characterSettings;
		SceneManager.LoadScene("MainGame");
	}

	public void Play()
	{
		SceneManager.LoadScene("MainGame");
	}

	public void Quit()
	{
		Application.Quit();
		Debug.Log("U quit");
	}
}
