using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class AssignmentManager : MonoBehaviour, IListContainer<IAssignment>
{
	private SimplePriorityQueue<IAssignment, int> assignments;
	private TilemapManager tileManager;
	private AssignmentFactory factory;
	private IAssignmentRunner student;

	[SerializeField] private GameObject gridObject;
	[SerializeField] private GameObject studentObject;

	void Awake()
	{
		tileManager = gridObject.GetComponent<TilemapManager>();
		student = studentObject.GetComponent<IAssignmentRunner>();
		factory = GetComponent<AssignmentFactory>();
		assignments = new SimplePriorityQueue<IAssignment, int>();
	}

	// 과제를 신에 생성 시도한다.
	public void TryPlaceAssignment(Vector3 mousePos, AssignmentType type, params object[] additionalArgs)
	{
		if(CanPlaceAssignment(mousePos) == false) return;
		Vector3 objectPosition = Utils.GetCellObjectPosition(mousePos);
		AssignmentBase newAssignmentObj = factory.CreateAssignment(objectPosition, type, additionalArgs);
		IAssignment assignment = new AssignmentStateHandler(newAssignmentObj);
		AddItem(assignment);
	}
	// 과제를 배치할 수 있는지 체크
	public bool CanPlaceAssignment(Vector3 position)
	{
		if( IsIntersectToStudent(position) ) return false;
		if( !TileMask.IsPlaceable(tileManager.GetTileMask(position)) ) return false;
		if( !TileMask.IsPlaceable(tileManager.GetTileMask(position + new Vector3(0f,1f,0f))) ) return false;
		return true;
	}
	// 과제를 큐에 추가하고, 만약 top이 바뀌었으면 AssignTask 시행
	public void AddItem(IAssignment item)
	{
		item.SetContainer(this);
		item.SetPathfinder(tileManager.pathfinder);
		IAssignment prev = GetTopAssignment();
		assignments.Enqueue(item, -item.priority);
		IAssignment top = GetTopAssignment();
		if(prev != top) student.AssignTask(top);
		
	}
	// 과제를 큐에서 제거하고, 만약 top이 바뀌었으면 AssignTask 시행
	public void RemoveItem(IAssignment item)
	{
		IAssignment prev = GetTopAssignment();
		assignments.Remove(item);
		IAssignment top = GetTopAssignment();
		if(prev != top) student.AssignTask(top);
	}
	private IAssignment GetTopAssignment()
	{
		if(assignments.Count == 0) return null;
		return assignments.First;
	}
	private bool IsIntersectToStudent(Vector3 point)
	{
		Vector3 center = Utils.GetCellObjectPosition(point);
		center.y += 0.5f;
		Bounds assignmentBound = new Bounds(center, new Vector3(1f, 2f, 1f));
		return student.bounds.Intersects(assignmentBound);
	}
}