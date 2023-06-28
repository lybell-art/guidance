using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
	private const float dragThreshold = 128f;
	private const float holdThreshold = 0.3f;
	private const float flickThreshold = 600f;

	private bool isTouch = false;
	private bool isPointerStarted = false;
	private Vector2 downPosition;
	private Vector3 downWorldPosition;
	private float holdTime;

	public Action OnPointerStart;
	public Action OnPointerEnd;
	public Action OnPointerCanceled;
	public Action<Vector3> OnClick;
	public Action<Vector3> OnHoldStart;
	public Action<Vector3, float> OnHolding;
	public Action<Vector3, float> OnHoldEnd;
	public Action<Vector3> OnDrag;
	public Action<Vector3, Vector2> OnFlick;

	// Update is called once per frame
	void Update()
	{
		if(PauseManager.paused) return;

		// pointer start
		HandlePointerStart();
		// pointer end
		if(isPointerStarted && IsPointerUp()) HandlePointerEnd();
		if(isTouch && Input.touchCount > 1)
		{
			isPointerStarted = false;
			OnPointerCanceled?.Invoke();
		}
		if(isPointerStarted)
		{
			if(holdTime <= holdThreshold && holdTime + Time.deltaTime > holdThreshold)
			{
				OnHoldStart?.Invoke(downWorldPosition);
			}
			if(holdTime > holdThreshold) OnHolding?.Invoke(downWorldPosition, holdTime);
			holdTime += Time.deltaTime;
		}
	}
	public bool IsPointerUp()
	{
		if(isTouch) return Input.GetTouch(0).phase == TouchPhase.Ended;
		return Input.GetMouseButtonUp(0);
	}
	public Vector2 GetPointerPosition()
	{
		if(isTouch) return Input.GetTouch(0).position;
		return Input.mousePosition;
	}
	private void InitializePointerStart()
	{
		isPointerStarted = true;
		holdTime = 0f;
		downPosition = GetPointerPosition();
		downWorldPosition = Camera.main.ScreenToWorldPoint(downPosition);
		OnPointerStart?.Invoke();
	}

	private void HandlePointerStart()
	{
		if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
			isTouch = true;
			InitializePointerStart();
		}
		else if(Input.GetMouseButtonDown(0))
		{
			if(EventSystem.current.IsPointerOverGameObject()) return;
			isTouch = false;
			InitializePointerStart();
		}
	}
	private void HandlePointerEnd()
	{
		Vector2 currentPosition = GetPointerPosition();
		float distance = Vector2.Distance(currentPosition, downPosition);
		OnPointerEnd?.Invoke();
		if(distance > dragThreshold)
		{
			float mag = distance / (holdTime == 0 ? 1/60f : holdTime);
			if(mag > flickThreshold)
			{
				Vector2 direction = (currentPosition - downPosition).normalized;
				OnFlick?.Invoke(downWorldPosition, direction);
			}
			else OnDrag?.Invoke(downWorldPosition);
		}
		else
		{
			if(holdTime > holdThreshold) OnHoldEnd?.Invoke(downWorldPosition, holdTime);
			else OnClick?.Invoke(downWorldPosition);
		}
		isPointerStarted = false;
		holdTime = 0f;
	}
}
