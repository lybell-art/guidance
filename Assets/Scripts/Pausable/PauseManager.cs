using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
	private static PauseManager instance;
	public static PauseManager Instance
	{
		get
		{
			if(instance == null || instance.Equals(null)) return null;
			return instance;
		}
	}

	private bool _paused = false;
	public static bool paused
	{
		get
		{
			if(instance.Equals(null)) return false;
			return instance._paused;
		}
	}
	public Action OnPauseEvent;
	public Action OnResumeEvent;

	void Awake()
	{
		if(instance != null) Destroy(gameObject);
		else instance = this;
	}
	void Update()
	{
		if(Input.GetKeyDown("p")) Toggle();
	}
	public void Pause()
	{
		this._paused = true;
		OnPauseEvent?.Invoke();
	}
	public void Resume()
	{
		this._paused = false;
		OnResumeEvent?.Invoke();
	}
	public void Toggle()
	{
		if(this._paused) Resume();
		else Pause();
	}
}