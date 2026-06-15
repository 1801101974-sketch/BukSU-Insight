using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMainMenu : MonoBehaviour
{
	public void Play()
	{
		SceneManager.LoadScene("LoadMenu");
	}

	public void Reset()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void Quit()
	{
		Application.Quit();
		Debug.Log("U quit");
	}
}
