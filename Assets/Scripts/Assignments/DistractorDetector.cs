using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractorDetector : MonoBehaviour
{
	private Distractor parent;
	void Start()
	{
		GameObject parentObject = transform.parent?.gameObject;
		if(parentObject != null) this.parent = parentObject.GetComponent<Distractor>();
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		IAssignmentRunner student = other.GetComponent<IAssignmentRunner>();
		if(student == null) return;
		parent?.OnDetectStudent();
		Destroy(gameObject);
	}
}