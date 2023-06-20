using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningIndicator : MonoBehaviour
{
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnTick(int time)
    {
        if(time <= 0)
        {
            animator.SetBool("fullWarning", true);
            return;
        }
        if(time < 10) Blink();
    }
    public void OnChangeFailedCount(int failCount)
    {
        if(failCount != 0) Blink();
        else animator.ResetTrigger("blinker");
    }

    public void Blink()
    {
        animator.SetTrigger("blinker");
    }
}
