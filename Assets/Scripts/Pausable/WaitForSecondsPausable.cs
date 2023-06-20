using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForSecondsPausable : CustomYieldInstruction
{
    private float time;
    private float until;

    public WaitForSecondsPausable(float time)
    {
        until = time;
    }

    public override bool keepWaiting
    {
        get
        {
            if(!PauseManager.paused) time += Time.deltaTime;
            bool wait = time < until;
            if(!wait) Reset();
            return wait;
        }
    }

    public override void Reset()
    {
        time = 0f;
    }
}