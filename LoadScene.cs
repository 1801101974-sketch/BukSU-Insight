using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
	public void LoadScenery()
	{
		SceneManager.LoadScene("1.1 Minigame");
	}

	public void LoadScenery2()
	{
		SceneManager.LoadScene("1.2 Minigame");
	}

	public void LoadScenery3()
	{
		SceneManager.LoadScene("1.3.1 Minigame");
	}

	public void LoadScenery4()
	{
		SceneManager.LoadScene("1.3.2 Minigame");
	}

	public void LoadScenery5()
	{
		SceneManager.LoadScene("MainGame");
	}

	public void LoadScenery6()
	{
		SceneManager.LoadScene("2.2 Minigame");
	}
}
