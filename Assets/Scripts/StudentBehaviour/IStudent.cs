using UnityEngine;

public interface IStudent
{
	Vector2 position{get;}
	bool isJump{get;}
	bool isLaunch{get;}
	bool onMovingPlatform{get;}
	void SetHorizontal(float movement);
	void MovePosition(Vector2 to);
	void Jump();
	void Launch(Vector2 direction);
	void ForceCalculateOnGround();
}

public interface IStudentBrain
{
	void SetTarget(IStudent student);
}