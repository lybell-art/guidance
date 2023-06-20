using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ITimeManager
{
    int currentTime{get;}
    int minutes{get;}
    int seconds{get;}
    void CountTime(int minutes, int seconds);
    void CountTime(int times);
    void StopCount();
}

public class TimeManager : MonoBehaviour, ITimeManager
{
    private int _currentTime;
    private IEnumerator timeCounter;

    public int currentTime{get {return _currentTime;}}
    public int minutes{get {return _currentTime / 60;}}
    public int seconds{get {return _currentTime % 60;}}
    public int timeLimit = 300;
    public UnityEvent<int> tickEvent = new UnityEvent<int>();
    public UnityEvent timeoverEvent = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        CountTime(timeLimit);
    }

    public void CountTime(int minute, int seconds)
    {
        CountTime(minute * 60 + seconds);
    }
    public void CountTime(int maxTime)
    {
        if(timeCounter != null) StopCoroutine(timeCounter);
        timeCounter = CountTimeCoroutine(maxTime);
        StartCoroutine(timeCounter);
    }
    public void StopCount()
    {
        if(timeCounter != null) StopCoroutine(timeCounter);
    }
    private IEnumerator CountTimeCoroutine(int maxTime)
    {
        WaitForSecondsPausable secs = new WaitForSecondsPausable(1f);
        _currentTime = maxTime;
        tickEvent?.Invoke(maxTime);
        while(true)
        {
            yield return secs;
            _currentTime -= 1;
            tickEvent?.Invoke(_currentTime);
            if(_currentTime == 0) timeoverEvent?.Invoke();
        }
    }
}
