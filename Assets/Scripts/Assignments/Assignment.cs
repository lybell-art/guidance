using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Assignment : AssignmentBase, IPointerClickHandler
{
	protected virtual void Start()
	{
		Activate();
	}
	public virtual void Initialize(params object[] additionalArgs)
	{
	}
	public virtual void OnTriggerEnter2D(Collider2D other)
	{
		IAssignmentRunner student = other.GetComponent<IAssignmentRunner>();
		if(student == null) return;
		Complete();
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		int clickID = eventData.pointerId;
		if(clickID != -1 && clickID != 0) return;
		Cancel();
	}
}