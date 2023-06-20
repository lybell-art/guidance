using System.Collections.Generic;
using UnityEngine;
using GuidancePlatform;

public interface IAssignment : IContainerItem
{
	int priority {get;}
	AssignmentStatus status {get;}
	void SetPathfinder(IPathfinder pathfinder);
	Queue<Instructor> GetInstructors(Vector3 position);
	void SetPriority(int priority);
	void StartTask();
	void PauseTask();
	void FinishTask();
	void CompleteTask();
	void FailTask();
	void CancelTask();
	bool IsEndStatus();
}