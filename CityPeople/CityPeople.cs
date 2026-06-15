using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CityPeople;

public class CityPeople : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Autoplay random animation clips")]
	private bool AutoPlayAnimations = true;

	[SerializeField]
	[Tooltip("Overrides palette materials, skips other objects")]
	private Material PaletteOverride;

	private AnimationClip[] myClips;

	private Animator animator;

	public const string people_pal_prefix = "people_pal";

	private List<Renderer> _paletteMeshes;

	public string CurrentPaletteName { get; private set; }

	private void Awake()
	{
		Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
		_paletteMeshes = new List<Renderer>();
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			string text = renderer.sharedMaterial.name;
			if (text[..Math.Min("people_pal".Length, text.Length)] == "people_pal")
			{
				_paletteMeshes.Add(renderer);
			}
		}
		if (_paletteMeshes.Count > 0)
		{
			CurrentPaletteName = _paletteMeshes[0].sharedMaterial.name;
		}
		if (PaletteOverride != null)
		{
			SetPalette(PaletteOverride);
		}
	}

	private void Start()
	{
		animator = GetComponent<Animator>();
		if (animator != null)
		{
			myClips = animator.runtimeAnimatorController.animationClips;
			if (AutoPlayAnimations)
			{
				PlayAnyClip();
				StartCoroutine(ShuffleClips());
			}
		}
		if (AutoPlayAnimations)
		{
			CapsuleCollider capsuleCollider = base.gameObject.AddComponent<CapsuleCollider>();
			capsuleCollider.center = new Vector3(0f, 0.8f, 0f);
			capsuleCollider.radius = 0.3f;
			capsuleCollider.height = 1.77f;
			capsuleCollider.direction = 1;
		}
	}

	public void SetPalette(Material mat)
	{
		if (!(mat != null))
		{
			return;
		}
		if (mat.name.Substring(0, "people_pal".Length) == "people_pal")
		{
			CurrentPaletteName = mat.name;
			{
				foreach (Renderer paletteMesh in _paletteMeshes)
				{
					paletteMesh.material = mat;
				}
				return;
			}
		}
		Debug.Log("Material name should start with 'palete_pal...' by convention.");
	}

	public void PlayAnyClip()
	{
		AnimationClip animationClip = myClips[UnityEngine.Random.Range(0, myClips.Length)];
		animator.CrossFadeInFixedTime(animationClip.name, 1f, -1, UnityEngine.Random.value * animationClip.length);
	}

	private IEnumerator ShuffleClips()
	{
		while (true)
		{
			yield return new WaitForSeconds(15f + UnityEngine.Random.value * 5f);
			PlayAnyClip();
		}
	}
}
