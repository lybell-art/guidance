using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityCursor : MonoBehaviour
{
    private int priority = 1;
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetPosition(Vector3 mousePos)
    {
        transform.position = Utils.GetCellObjectPosition(mousePos);
    }
    public void SetPriority(int priority)
    {
        if(!this.enabled) return;
        if(priority != this.priority) animator.Play(Constants.priorityAssignmentPre + priority.ToString());
        this.priority = priority;
    }
}
