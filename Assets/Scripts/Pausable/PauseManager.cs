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

	private int pauseChannel = 0;
	public static bool paused
	{
		get
		{
			if(instance.Equals(null)) return false;
			return instance.pauseChannel != 0;
		}
	}
	public Action OnPauseEvent;
	public Action OnResumeEvent;
	public Action<int> OnChannelPauseEvent;
	public Action<int> OnChannelResumeEvent;

	void Awake()
	{
		if(instance != null) Destroy(gameObject);
		else instance = this;
	}
	void Update()
	{
		if(Input.GetKeyDown("p")) Toggle(0);
	}
	public void Pause(int channel=0)
	{
		this.pauseChannel |= 1 << channel;
		OnChannelPauseEvent?.Invoke(channel);
		if(this.pauseChannel != 0) OnPauseEvent?.Invoke();
	}
	public void Resume(int channel=0)
	{
		this.pauseChannel &= ~(1 << channel);
		OnChannelResumeEvent?.Invoke(channel);
		if(this.pauseChannel == 0) OnResumeEvent?.Invoke();
	}
	public void Toggle(int channel=0)
	{
		if((this.pauseChannel & (1 << channel)) != 0) Resume(channel);
		else Pause(channel);
	}
}