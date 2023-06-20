using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuidancePlatform;

public class AssignmentRunner : MonoBehaviour, IStudentBrain, IAssignmentRunner, IStageLoadable
{
	private BoxCollider2D myCollider;
	private IStudent student;
	private IScoreManager scoreManager;

	private Vector2 stablePosition;
	private IAssignment pendingTask;
	private IAssignment currentTask;
	private IEnumerator runningCoroutine;
	private bool hasPendingTask = false;
	private bool _isRunning = false;
	[SerializeField] private GameObject scoreManagerObj;

	public Action onTaskFilled{get; set;}
	public Action onTaskEmpty{get; set;}
	public bool runningStatus{
		get {return _isRunning;}
		private set
		{
			if(_isRunning != value)
			{
				if(value) onTaskFilled?.Invoke();
				else onTaskEmpty?.Invoke();
			}
			_isRunning = value;
		}
	}
	public bool pausingTask
	{
		get
		{
			return student.onMovingPlatform || PauseManager.paused;
		}
	}

	// bound box
	public Bounds bounds
	{
		get
		{
			return this.myCollider.bounds;
		}
	}

	void Awake()
	{
		myCollider = GetComponent<BoxCollider2D>();
		student = GetComponent<IStudent>();
		scoreManager = scoreManagerObj.GetComponent<IScoreManager>();
	}

	void Update()
	{
		if(PauseManager.paused) return;
		HandleNextAssignment();
	}

	// 스테이지가 시작될 때 호출된다.
	public void Initialize()
	{
		stablePosition = student.position;
		currentTask = null;
		pendingTask = null;
		hasPendingTask = false;
		if(runningCoroutine != null)
		{
			StopCoroutine(runningCoroutine);
			runningCoroutine = null;
		}
	}
	// 스테이지가 로드되었을 때, 학생의 위치를 이동시킨다.
	public void OnLoadStage(int stageNo, StageData stageData)
	{
		Initialize();
		stablePosition = stageData.startPoint;
	}

	public void SetTarget(IStudent student)
	{
		this.student = student;
	}

	public void AssignTask(IAssignment task)
	{
		this.hasPendingTask = true;
		this.pendingTask = task;
	}

	// 학생이 장애물에 닿았을 시 호출
	public void HitObstacle()
	{
		this.currentTask?.FailTask();
		if(this.currentTask is DeliberateAssignment) AssignTask(null);
		student.MovePosition(this.stablePosition);
		scoreManager?.AddFailedTask();
	}

	// 체공 중이 아닐 때 + 대기열에 task가 있을 때, 현재 태스크를 없애고 다음 태스크를 실행
	private void HandleNextAssignment()
	{
		if(!this.hasPendingTask || student.isJump) return;
		this.hasPendingTask = false;
		if(this.currentTask != null)
		{
			if(this.currentTask.status == AssignmentStatus.pendingSuccess)
			{
				this.currentTask.CompleteTask();
			}
			else if(!this.currentTask.IsEndStatus())
			{
				this.currentTask.PauseTask();
			}
		}
		if(this.runningCoroutine != null) StopCoroutine(this.runningCoroutine);

		if(this.pendingTask != null)
		{
			if(this.pendingTask.status == AssignmentStatus.pendingSuccess)
			{
				this.pendingTask.CompleteTask();
				this.pendingTask = null;
			}
			else
			{
				this.pendingTask.StartTask();
				this.runningCoroutine = RunInstruction(this.pendingTask);
				StartCoroutine(this.runningCoroutine);
			}
		}
		else 
		{
			stablePosition = student.position;
			student.SetHorizontal(0f);
			this.runningStatus = false;
		}
		this.currentTask = this.pendingTask;
		this.pendingTask = null;
	}
	// 태스크를 수행
	private IEnumerator RunInstruction(IAssignment task)
	{
		bool IsReached(float playerX, float goalX, int dir)
		{
			if(dir>0) return playerX - goalX > 0f;
			else if(dir<0) return playerX - goalX < 0f;
			return true;
		}
		WaitForSecondsPausable delay = new WaitForSecondsPausable(0.05f);
		WaitUntil untilResume = new WaitUntil(() => this.pausingTask == true);
		if(task == null) yield break;
		this.runningStatus = true;
		while(task.IsEndStatus() == false)
		{
			if(!student.isJump) this.stablePosition = student.position;
			Queue<Instructor> instructors = task.GetInstructors(student.position);
			if(instructors == null || instructors.Count == 0)
			{
				this.runningStatus = false;
				yield break;
			}
			while(instructors.Count > 0)
			{
				Instructor instructor = instructors.Dequeue();
				if(task.status == AssignmentStatus.pendingSuccess) break;
				if(this.pausingTask) break;

				// 다음 명령이 점프면, 점프 시도
				if(instructor.isJump) 
				{
					yield return delay;
					student.Jump();
				}

				// instructor가 강제점프라면 존재한다면 forceJump = true
				bool isReachedX = false;
				bool isForceJump = instructor.cancelWhenGrounded || student.isLaunch;
				bool isLaunched = student.isLaunch;
				int dir = (int)Mathf.Sign(instructor.x - student.position.x);

				// 체공 중이면 반복
				// forceJump 중이면 isReachedX에 상관없이 isJump가 false일 때 빠져나감
				// forceJump가 아니라면 isReachedX가 true일 때 빠져나감
				while(student.isJump || (!isForceJump && !isReachedX))
				{
					// 목적지 도달을 체크
					if(!isReachedX && IsReached(student.position.x, instructor.x, dir))
					{
						isReachedX = true;
						student.ForceCalculateOnGround();
					}
					if(!isForceJump && student.isLaunch)
					{
						isForceJump = true;
						student.ForceCalculateOnGround();
					}
					if(!isLaunched && student.isLaunch) isLaunched = true;

					// x좌표에 도달하기 전까지 horizMover를 변경하여 학생을 이동시킴
					// 사출 중이고 이미 과제를 먹었으면 움직임 갱신을 하지 않음
					if(!isLaunched || task.status != AssignmentStatus.pendingSuccess)
					{
						if(!isReachedX) student.SetHorizontal(dir);
						else student.SetHorizontal(0f);
					}

					yield return null;
				}
				if(isForceJump) {
					yield return null;
					break;
				}
			}
			if(task.status == AssignmentStatus.pendingSuccess) task.CompleteTask();
			if(this.pausingTask) yield return untilResume;
		}
	}
}