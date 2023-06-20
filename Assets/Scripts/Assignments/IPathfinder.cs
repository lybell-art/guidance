using System.Collections.Generic;
using UnityEngine;

public interface IPathfinder
{
	Queue<GuidancePlatform.Instructor> Find(Vector2Int start, Vector2Int end);
	Queue<GuidancePlatform.Instructor> Find(Vector3 start, Vector3 end);
}