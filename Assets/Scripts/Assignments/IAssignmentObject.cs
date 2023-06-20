using System.Collections.Generic;
using UnityEngine;
using GuidancePlatform;

public interface IAssignmentObject
{
	int priority {get;}
	Vector3 position {get;}
	void SetStateHandler(IAssignment assignment);
	void Complete();
	void Cancel();
	void Delete();
}