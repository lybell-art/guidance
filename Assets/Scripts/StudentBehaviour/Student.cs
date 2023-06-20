using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : MonoBehaviour, IStudent, IStageLoadable
{
	// component
	private SpriteRenderer sprite;
	private Rigidbody2D rigid;
	private PausableRigidSaver rigidSaver;
	private BoxCollider2D myCollider;
	private Animator animator;
	private StudentSoundManager soundPlayer;

	// IStudent interface
	public Vector2 position
	{
		get {return rigid.position;}
	}
	// for moving
	private float horizMover;
	public bool isJump {get; private set;}
	public bool isLaunch {get; private set;}
	public bool onMovingPlatform {get; private set;}

	// for animation
	private Vector2 launchVector;
	private bool isLaunchAnim;

	// moving parameters
	public float maxSpeed = 6f;
	public float jumpForce = 17f;
	public float launchForce = 25f;
	public float launchDampForce = 12f;

	void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();
		rigid = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<BoxCollider2D>();
		animator = GetComponent<Animator>();
		soundPlayer = GetComponent<StudentSoundManager>();
		rigidSaver = new PausableRigidSaver(rigid);
	}
	void Start()
	{
		rigid.freezeRotation = true;
		Initialize();
	}
	void FixedUpdate()
	{
		rigidSaver.Run();
        if(PauseManager.paused)
        {
        	animator.speed = 0f;
        	return;
        }
        if(animator.speed != 1f) animator.speed = 1f;

		if(!isLaunch) MoveHorizontal(horizMover);
		else LaunchDamp();
		UpdateJumpStatus();
		UpdateLaunchStatus();
		SetAnimation();
		UpdateWalkSound();
	}
	// 학생의 horizMover를 수정한다.
	public void SetHorizontal(float movement)
	{
		this.horizMover = movement;
	}
	// 학생을 특정 위치로 강제이동한다.
	public void MovePosition(Vector2 position)
	{
		rigid.position = position;
		rigid.velocity = Vector2.zero;
	}
	// 학생을 점프시킨다.
	public void Jump()
	{
		if(isJump) return;
		if(rigid.velocity.y < 0f)
		{
			Vector2 velocity = rigid.velocity;
			velocity.y = 0f;
			rigid.velocity = velocity;
		}
		rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		isJump = true;
		soundPlayer.PlayJump();
	}
	// 학생을 사출한다.
	public void Launch(Vector2 direction)
	{
		rigid.velocity = direction * launchForce;
		isLaunch = true;
		isLaunchAnim = true;
		launchVector = direction;
		horizMover = 0f;
		soundPlayer.PlayLaunch();
	}
	// 
	public void ForceCalculateOnGround()
	{
		if(!isJump && !CheckGround()) isJump = true;
	}

	// 스테이지가 시작될 때 호출된다.
	public void Initialize()
	{
		isJump = false;
		isLaunch = false;
		isLaunchAnim = false;
	}
	// 스테이지가 로드되었을 때, 학생의 위치를 이동시킨다.
	public void OnLoadStage(int stageNo, StageData stageData)
	{
		Initialize();
		rigid.position = stageData.startPoint;
	}

	private void MoveHorizontal(float horizontal)
	{
		if(horizontal != 0f) {
			rigid.AddForce(Vector2.right * horizontal, ForceMode2D.Impulse);
			if(rigid.velocity.x > maxSpeed) {
				rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
			}
			else if(rigid.velocity.x < -maxSpeed) {
				rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);
			}
		}
		else {
			rigid.velocity = new Vector2(rigid.velocity.x * 0.5f, rigid.velocity.y);
			if (Utils.IsZero(rigid.velocity.x)) {
				rigid.velocity = new Vector2(0f, rigid.velocity.y);
			}
		}
	}
	private void UpdateJumpStatus()
	{
		float yVelocity = rigid.velocity.y;
		if(!isJump && !Utils.IsZero(yVelocity)) isJump = true;
		else if(isJump) 
		{
			if(yVelocity < jumpForce * 0.5f && CheckGround()) isJump = false;
			else if(Mathf.Approximately(yVelocity, 0f) && CheckGroundBroad()) isJump = false;
			if(isJump == false) soundPlayer.PlayLand();
		}
	}
	private void UpdateLaunchStatus()
	{
		if(isLaunchAnim && rigid.velocity.magnitude < launchForce * 0.5f) isLaunchAnim = false;
		if(isLaunch && Mathf.Abs(rigid.velocity.x) < maxSpeed * 0.3f) isLaunch = false;
	}
	private void LaunchDamp()
	{
		if(!isLaunch) return;
		float sign = Mathf.Sign(rigid.velocity.x);
		rigid.AddForce(new Vector2(-sign, 0f) * launchDampForce, ForceMode2D.Force);
	}
	private void SetAnimation()
	{
		Vector2 movement = rigid.velocity;
		animator.SetBool("Launch", isLaunchAnim);
		animator.SetBool("Jump", isJump);
		animator.SetFloat("SpeedY", movement.y);
		animator.SetFloat("Speed", movement.magnitude);
		animator.SetFloat("LaunchX", launchVector.x);
		animator.SetFloat("LaunchY", launchVector.y);
		if(!Mathf.Approximately(movement.x, 0.0f)) {
			sprite.flipX = (movement.x < 0f);
		}
	}
	private void UpdateWalkSound()
	{
		if(isLaunch || isJump) soundPlayer.EndWalk();
		else if(Mathf.Approximately(rigid.velocity.x, 0.0f)) soundPlayer.EndWalk();
		else soundPlayer.PlayWalk();
	}
	private bool CheckGround()
	{
		Bounds bounds = this.myCollider.bounds;
		Vector2 center = new Vector2(bounds.center.x, bounds.min.y + 0.1f);
		Vector2 size = new Vector2(bounds.size.x - 0.1f, 0.2f);

		int platformMask = (1 << Constants.platform) | (1 << Constants.interactive);
		RaycastHit2D hit = Physics2D.BoxCast(center, size, 0f, Vector2.down, 0.1f, platformMask);
		return hit.collider != null;
	}
	private bool CheckGroundBroad()
	{
		Bounds bounds = this.myCollider.bounds;
		Vector2 center = new Vector2(bounds.center.x, bounds.min.y + 0.1f);
		Vector2 size = new Vector2(bounds.size.x + 0.1f, 0.2f);

		int platformMask = (1 << Constants.platform) | (1 << Constants.interactive);
		RaycastHit2D hit = Physics2D.BoxCast(center, size, 0f, Vector2.down, 0.1f, platformMask);
		return hit.collider != null;
	}
}