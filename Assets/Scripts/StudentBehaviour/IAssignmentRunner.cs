using System;
using UnityEngine;

public interface IAssignmentRunner
{
	public Action onTaskFilled{get; set;}
	public Action onTaskEmpty{get; set;}
	public Bounds bounds{get;}
	public void AssignTask(IAssignment task);
	public void HitObstacle();
}