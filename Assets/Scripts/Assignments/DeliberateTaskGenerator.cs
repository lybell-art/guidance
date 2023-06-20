using System.Collections;
using UnityEngine;

public class DeliberateTaskGenerator : MonoBehaviour, IDeliberateTaskGenerator
{
	private DeliberateAssignment _goalAssignment;
	private TilemapManager tileManager;
	private IScoreManager scoreManager;

	[SerializeField] private GameObject tileManagerObj;
	[SerializeField] private GameObject scoreManagerObj;
	
	void Awake()
	{
		tileManager = tileManagerObj.GetComponent<TilemapManager>();
		scoreManager = scoreManagerObj.GetComponent<IScoreManager>();
	}

	public DeliberateAssignment deliberateAssignment
	{
		get
		{
			if(_goalAssignment != null) return _goalAssignment;
			_goalAssignment = new DeliberateAssignment(scoreManager);
			_goalAssignment.SetPosition(transform.position);
			_goalAssignment.SetPathfinder(tileManager.pathfinder);
			return _goalAssignment;
		}
	}
}
