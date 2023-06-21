using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliberateAssignment : IAssignment
{
    private Vector3 position;
    private float activateStartTime;
    private float activateTime;
    private IScoreManager scoreManager;
    private IPathfinder pathfinder;
    public int priority{get; private set;}
    public AssignmentStatus status{get; private set;}

    public DeliberateAssignment(IScoreManager scoreManager)
    {
        this.scoreManager = scoreManager;
        this.SetPriority(-9999);
    }

    public void SetPosition(Vector3 position)
    {
        this.position = position;
    }
    public void SetPriority(int priority)
    {
        this.priority = priority;
    }
    // IAssignment implementation
    public void SetPathfinder(IPathfinder pathfinder)
    {
        this.pathfinder = pathfinder;
    }
    public Queue<GuidancePlatform.Instructor> GetInstructors(Vector3 targetPosition)
    {
        return this.pathfinder.Find(targetPosition, position);
    }
    public void StartTask()
    {
        if(!IsExistingStatus(this.status)) return;
        this.SetStatus(AssignmentStatus.running);
        activateStartTime = Time.time;
    }
    public void PauseTask()
    {
        if(this.status != AssignmentStatus.running) return;
        activateTime = Time.time - activateStartTime;
        activateStartTime = Time.time;
        scoreManager?.AddDeliberation(activateTime);
    }
    public void FinishTask()
    {
        if(!IsExistingStatus(this.status)) return;
        PauseTask();
        this.SetStatus(AssignmentStatus.success);
    }
    public void CompleteTask()
    {
        if(this.status != AssignmentStatus.pendingSuccess) return;
        this.SetStatus(AssignmentStatus.success);
    }
    public void FailTask()
    {
        if(IsEndStatus(this.status)) return;
        PauseTask();
        this.SetStatus(AssignmentStatus.inQueue);
    }
    public void CancelTask()
    {
        if(!IsExistingStatus(this.status)) return;
        this.SetStatus(AssignmentStatus.inQueue);
    }
    private void SetStatus(AssignmentStatus status)
    {
        this.status = status;
    }

    public bool IsEndStatus()
    {
        return this.IsEndStatus(this.status);
    }
    
    private bool IsExistingStatus(AssignmentStatus status)
    {
        switch(status)
        {
            case AssignmentStatus.pendingSuccess:
            case AssignmentStatus.success:
            case AssignmentStatus.failed:
            case AssignmentStatus.canceled:
                return false;
            default:
                return true;
        }
    }
    private bool IsEndStatus(AssignmentStatus status)
    {
        switch(status)
        {
            case AssignmentStatus.success:
            case AssignmentStatus.failed:
            case AssignmentStatus.canceled:
                return true;
            default:
                return false;
        }
    }

    // don't use this
    public void SetContainer<T>(IListContainer<T> container) where T : IContainerItem{}
    public void RemoveSelf(){}
    public void SetGameObject(IAssignmentObject obj){}
}
