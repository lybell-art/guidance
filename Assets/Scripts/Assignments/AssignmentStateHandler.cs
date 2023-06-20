using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentStateHandler : IAssignment
{
    private IListContainer<IAssignment> container;
    private Vector3 position;
    public int priority{get; private set;}
    public AssignmentStatus status{get; private set;}
    private IPathfinder pathfinder;
    private IAssignmentObject gameObject;

    public AssignmentStateHandler(IAssignmentObject obj)
    {
        this.SetGameObject(obj);
        this.SetPriority(obj.priority);
    }

    // IContainerItem implementation
    public void SetContainer<T>(IListContainer<T> container) where T : IContainerItem
    {
        this.container = (IListContainer<IAssignment>)container;
    }
    public void RemoveSelf()
    {
        this.container.RemoveItem(this);
    }
    public void SetGameObject(IAssignmentObject obj)
    {
        this.gameObject = obj;
        obj.SetStateHandler(this);
        this.position = obj.position;
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
    }
    public void FinishTask()
    {
        if(!IsExistingStatus(this.status)) return;
        if(this.status == AssignmentStatus.inQueue) this.SetStatus(AssignmentStatus.success);
        else this.SetStatus(AssignmentStatus.pendingSuccess);
    }
    public void CompleteTask()
    {
        if(this.status != AssignmentStatus.pendingSuccess) return;
        this.SetStatus(AssignmentStatus.success);
    }
    public void FailTask()
    {
        if(IsEndStatus(this.status)) return;
        this.SetStatus(AssignmentStatus.failed);
        if(this.gameObject != null && !this.gameObject.Equals(null)) 
        {
            this.gameObject?.Delete();
        }
        else this.gameObject = null;
    }
    public void CancelTask()
    {
        if(!IsExistingStatus(this.status)) return;
        this.SetStatus(AssignmentStatus.canceled);
    }
    public void PauseTask(){}
    private void SetStatus(AssignmentStatus status)
    {
        this.status = status;
        if(IsEndStatus(status)) RemoveSelf();
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

    public override string ToString()
    {
        return this.gameObject?.ToString();
    }
}
