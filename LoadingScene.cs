using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
	[Tooltip("The loading screen GameObject")]
	public GameObject LoadingScreen;

	[Tooltip("The fill image of the loading bar")]
	public Image loadingBarFill;

	public void LoadScene(int sceneId)
	{
		StartCoroutine(LoadSceneAsync(sceneId));
	}

	private IEnumerator LoadSceneAsync(int sceneId)
	{
		if (LoadingScreen == null || loadingBarFill == null)
		{
			Debug.LogError("LoadingScreen or loadingBarFill not assigned in the inspector!");
			yield break;
		}
		LoadingScreen.SetActive(value: true);
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId);
		asyncLoad.allowSceneActivation = false;
		while (!asyncLoad.isDone)
		{
			float fillAmount = Mathf.Clamp01(asyncLoad.progress / 0.9f);
			loadingBarFill.fillAmount = fillAmount;
			if (asyncLoad.progress >= 0.9f)
			{
				Debug.Log("Press the space bar to start the game");
				yield return null;
				asyncLoad.allowSceneActivation = true;
			}
			yield return null;
		}
	}
}
