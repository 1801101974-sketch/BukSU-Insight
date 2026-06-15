using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoIntro : MonoBehaviour
{
	public VideoPlayer videoPlayer;

	public string nextSceneName;

	public float delayBeforeTransition = 1f;

	private bool videoEnded;

	private void Start()
	{
		if (videoPlayer == null)
		{
			Debug.LogError("VideoPlayer is not assigned. Please assign it in the inspector.");
			base.enabled = false;
		}
		else
		{
			videoPlayer.loopPointReached += OnVideoEnd;
			videoPlayer.Play();
		}
	}

	private void OnVideoEnd(VideoPlayer vp)
	{
		videoEnded = true;
		StartCoroutine(DelayedSceneLoad());
	}

	private IEnumerator DelayedSceneLoad()
	{
		yield return new WaitForSeconds(delayBeforeTransition);
		SceneManager.LoadScene(nextSceneName);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !videoEnded)
		{
			videoPlayer.Stop();
			StopAllCoroutines();
			SceneManager.LoadScene(nextSceneName);
		}
	}
}
