using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentBase : MonoBehaviour, IAssignmentObject
{
    private IAssignment stateHandler;
    private IEnumerator activatedTimeout;
    public int priority{get; protected set;}
    public Vector3 position
    {
        get
        {
            return transform.position;
        }
    }

    public void SetStateHandler(IAssignment handler)
    {
        stateHandler = handler;
    }
    public void Complete()
    {
        this.stateHandler.FinishTask();
        Delete();
    }
    public void Cancel()
    {
        this.stateHandler.CancelTask();
        Delete();
    }
    public void Delete()
    {
        if(activatedTimeout != null) StopCoroutine(activatedTimeout);
        if(gameObject != null) Destroy(gameObject);
    }

    // activation
    public void Activate()
    {
        this.activatedTimeout = TimeoverTask();
        StartCoroutine(this.activatedTimeout);
    }
    protected IEnumerator TimeoverTask()
    {
        yield return new WaitForSecondsPausable(Constants.assignmentTimeout);
        stateHandler.FailTask();
        Delete();
    }
}