using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterHazard : MonoBehaviour
{
    private Animator animator;
    public float duration=3f;
    public float delay=0f;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        StartCoroutine(Run(duration, delay));
    }
    void OnEnable()
    {
        if(PauseManager.Instance == null) return;
        PauseManager.Instance.OnPauseEvent += OnPause;
        PauseManager.Instance.OnResumeEvent += OnResume;
    }
    void OnDisable()
    {
        if(PauseManager.Instance == null) return;
        PauseManager.Instance.OnPauseEvent -= OnPause;
        PauseManager.Instance.OnResumeEvent -= OnResume;
    }
    IEnumerator Run(float duration, float startDelay)
    {
        WaitForSecondsPausable loopDelay = new WaitForSecondsPausable(duration);
        yield return new WaitForSecondsPausable(startDelay);
        while(true)
        {
            yield return loopDelay;
            animator.SetTrigger("activate");
        }
    }
    void OnPause()
    {
        animator.speed = 0f;
    }
    void OnResume()
    {
        animator.speed = 1f;
    }
}
