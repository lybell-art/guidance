using System.Collections;
using UnityEngine;

public class PauseUI : FadableContainer
{
    public void Resume()
    {
        StartCoroutine(FadeOut(AfterFadeOut));
    }
    private void AfterFadeOut()
    {
        PauseManager.Instance?.Resume(0);
        gameObject.SetActive(false);
    }
}