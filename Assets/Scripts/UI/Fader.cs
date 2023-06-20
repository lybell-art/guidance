using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    private Image fadeImage;
    [SerializeField] private GameObject fadeUI;
    void Awake()
    {
        fadeImage = fadeUI.GetComponent<Image>();
    }
    public void Show()
    {
        fadeUI.SetActive(true);
    }
    public IEnumerator Fade(float duration)
    {
        fadeUI.SetActive(true);
        fadeImage.color = new Color(0f, 0f, 0f, 0f);
        float time = 0f;
        while(time < duration)
        {
            float alpha = time / duration;
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
            time += Time.deltaTime;
        }
    }
    public IEnumerator FadeIn(float duration)
    {
        fadeUI.SetActive(true);
        fadeImage.color = new Color(0f, 0f, 0f, 1f);
        float time = 0f;
        while(time < duration)
        {
            float alpha = time / duration;
            fadeImage.color = new Color(0f, 0f, 0f, 1f - alpha);
            yield return null;
            time += Time.deltaTime;
        }
        fadeUI.SetActive(false);
    }
    public IEnumerator WhiteFade(float duration)
    {
        fadeUI.SetActive(true);
        fadeImage.color = new Color(1f, 1f, 1f, 0f);
        float time = 0f;
        while(time < duration)
        {
            float alpha = time / duration;
            fadeImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
            time += Time.deltaTime;
        }
    }
}
