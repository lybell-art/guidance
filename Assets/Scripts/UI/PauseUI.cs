using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
    void OnEnable()
    {
        StartCoroutine(Fade(1.0f, 0.4f));
    }
    public void Resume()
    {
        StartCoroutine(FadeOutResume());
    }
    public void Hide()
    {
        StartCoroutine(FadeOut());
    }
    
    private IEnumerator FadeOut()
    {
        yield return Fade(0.0f, 0.4f);
        gameObject.SetActive(false);
    }
    private IEnumerator FadeOutResume()
    {
        yield return Fade(0.0f, 0.4f);
        PauseManager.Instance?.Resume(0);
        gameObject.SetActive(false);
    }
    private IEnumerator Fade(float alpha, float duration)
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
