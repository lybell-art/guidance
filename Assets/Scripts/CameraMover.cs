using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMover : MonoBehaviour, IStageLoadable
{
	private readonly float touchZoomFactor = 0.005f;
	private readonly float mouseZoomFactor = -0.5f;
	private readonly float panThreshold = 2f;
	private readonly float snapThreshold = 1f;

	private bool _isLocked = false;
	private bool _isHardLocked = false;
	private bool _isTempLocked = false;
	private Vector3 prevMouseWheelDelta;
	private Collider2D confiner;

	private Camera realCamera;
	private CinemachineVirtualCamera virtualCamera;
	private ClickManager clickManager;

	[SerializeField] private GameObject cameraObj;
	[SerializeField] private GameObject virtualMover;
	[SerializeField] private GameObject follower;

	public bool isFollowing
	{
		get
		{
			return !(_isLocked || _isHardLocked || _isTempLocked);
		}
		set
		{
			_isLocked = !value;
		}
	}

	public float zoomLevel
	{
		get
		{
			if(virtualCamera == null) return 1f;
			return virtualCamera.m_Lens.OrthographicSize;
		}
		set
		{
			if(virtualCamera == null) return;
			float size = Mathf.Clamp(value, Constants.cameraBaseSize, Constants.cameraBaseSize * 3);
			virtualCamera.m_Lens.OrthographicSize = size;
		}
	}

	void Awake()
	{
		virtualCamera = cameraObj.GetComponent<CinemachineVirtualCamera>();
		realCamera = virtualCamera.VirtualCameraGameObject.GetComponent<Camera>();
		CinemachineConfiner confinerObj = cameraObj.GetComponent<CinemachineConfiner>();
		if(confinerObj != null) confiner = confinerObj.m_BoundingShape2D as Collider2D;
		clickManager = GetComponent<ClickManager>();
	}

	void OnEnable()
	{
		clickManager.OnPointerStart += this.OnPointerStart;
		clickManager.OnPointerEnd += this.OnPointerEnd;
		clickManager.OnPointerCanceled += this.OnPointerEnd;
	}
	void OnDisable()
	{
		clickManager.OnPointerStart -= this.OnPointerStart;
		clickManager.OnPointerEnd -= this.OnPointerEnd;
		clickManager.OnPointerCanceled -= this.OnPointerEnd;
	}

	// Update is called once per frame
	void LateUpdate()
	{
		if(PauseManager.paused) return;
		
		Vector2 panDelta;
		float zoomDelta;
		if(Input.touchCount == 2) (panDelta, zoomDelta) = GetTouchCameraInput();
		else
		{
			_isHardLocked = false;
			(panDelta, zoomDelta) = GetMouseCameraInput();
		}

		if(panDelta.magnitude > panThreshold) _isLocked = true;
		else if(_isLocked && !_isHardLocked) TrySnap();

		if(isFollowing) followObject();
		else moveObject(panDelta);
		zoomCamera(zoomDelta);
	}
	public void Initialize()
	{
		_isLocked = false;
		_isHardLocked = false;
		_isTempLocked = false;
	}
	public void OnLoadStage(int stageNo, StageData stageData)
	{
		Initialize();
		virtualMover.transform.position = stageData.startPoint;
	}

	public void SnapToFollower()
	{
		Debug.Log("nye!");
		this.isFollowing = true;
	}

	private (Vector2, float) GetTouchCameraInput()
	{
		_isHardLocked = true;
		
		Touch touch1 = Input.GetTouch(0);
		Touch touch2 = Input.GetTouch(1);

		Vector2 curTouchPos0 = touch1.position;
		Vector2 curTouchPos1 = touch2.position;
		Vector2 preTouchPos0 = touch1.phase == TouchPhase.Began ? curTouchPos0 : curTouchPos0 - touch1.deltaPosition;
		Vector2 preTouchPos1 = touch2.phase == TouchPhase.Began ? curTouchPos1 : curTouchPos1 - touch2.deltaPosition;

		// get zoom factor
		float preDist = Vector2.Distance(preTouchPos0, preTouchPos1);
		float curDist = Vector2.Distance(curTouchPos0, curTouchPos1);
		float zoomDelta = (preDist - curDist) * touchZoomFactor;

		// get pan factor
		Vector2 curTouchCenter = (curTouchPos0 + curTouchPos1) / 2f;
		Vector2 preTouchCenter = (preTouchPos0 + preTouchPos1) / 2f;
		Vector2 panDelta = curTouchCenter - preTouchCenter;

		return (-panDelta, zoomDelta);
	}
	private (Vector2, float) GetMouseCameraInput()
	{
		Vector2 panDelta = new Vector2();
		if(Input.GetMouseButtonDown(2))
		{
			prevMouseWheelDelta = Input.mousePosition;
			_isHardLocked = true;
		}
		if(Input.GetMouseButton(2))
		{
			panDelta = Input.mousePosition - prevMouseWheelDelta;
			prevMouseWheelDelta = Input.mousePosition;
		}
		if(Input.GetMouseButtonUp(2)) _isHardLocked = false;
		float zoomDelta = Input.mouseScrollDelta.y * mouseZoomFactor;
		return (-panDelta, zoomDelta);
	}

	private void followObject()
	{
		virtualMover.transform.position = follower.transform.position;
	}
	private void moveObject(Vector2 delta)
	{
		float cellSize = Screen.height / (this.zoomLevel*2);
		Vector3 worldDelta = (Vector3)delta / cellSize;

		Vector3 prevPos = ConfineBound(virtualMover.transform.position);
		Vector3 newPos = prevPos + (Vector3)worldDelta;
		virtualMover.transform.position = newPos;
	}
	private void zoomCamera(float zoomDelta)
	{
		if(virtualCamera == null) return;
		this.zoomLevel += zoomDelta;
	}
	private Vector2 ConfineBound(Vector2 position)
	{
		if(confiner == null) return position;
		Bounds bounds = confiner.bounds;
		float halfHeight = this.zoomLevel;
		float halfWidth = halfHeight * Camera.main.aspect;
		float left = bounds.min.x + halfWidth;
		float right = bounds.max.x - halfWidth; 
		float top = bounds.max.y - halfHeight;
		float bottom = bounds.min.y + halfHeight; 

		float x = (left > right) ? bounds.center.x : Mathf.Clamp(position.x, left, right);
		float y = (top < bottom) ? bounds.center.y : Mathf.Clamp(position.y, bottom, top);
		return new Vector2(x, y);
	}
	private void TrySnap()
	{
		Vector2 cameraPos = virtualMover.transform.position;
		Vector2 followerPos = follower.transform.position;
		if(Vector2.Distance(cameraPos, followerPos) < snapThreshold) _isLocked = false;
	}

	private void OnPointerStart()
	{
		_isTempLocked = true;
	}
	private void OnPointerEnd()
	{
		_isTempLocked = false;
	}
}
