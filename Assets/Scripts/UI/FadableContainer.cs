using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadableContainer : MonoBehaviour
{
	private CanvasGroup canvasGroup;
	protected float baseDuration = 0.4f;
	protected virtual void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.alpha = 0f;
		gameObject.SetActive(false);
	}
	public virtual void Show()
	{
		gameObject.SetActive(true);
		StartCoroutine(FadeIn());
	}
	public virtual void Hide()
	{
		StartCoroutine(FadeOut(()=>gameObject.SetActive(false)));
	}
	protected IEnumerator FadeIn()
	{
		yield return Fade(1f, baseDuration);
	}
	protected IEnumerator FadeOut(Action OnFadeOut)
	{
		yield return Fade(0f, baseDuration);
		OnFadeOut?.Invoke();
	}
	protected IEnumerator Fade(float alpha, float duration)
	{
		if(duration <= 0f)
		{
			canvasGroup.alpha = alpha;
			yield break;
		}

		float time = 0f;
		float startAlpha = canvasGroup.alpha;
		while(time < duration)
		{
			canvasGroup.alpha = Mathf.Lerp(startAlpha, alpha, time / duration);
			yield return null;
			time += Time.deltaTime;
		}
		canvasGroup.alpha = alpha;
	}
}