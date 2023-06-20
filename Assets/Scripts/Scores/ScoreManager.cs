using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IScoreManager
{
    int score {get;}
    bool isGoodEnding {get;}
    int failedCount {get;}
    int collectedWizdomCount {get;}
    float deliberationDuration {get;}
    void AddFailedTask();
    void AddWizdom();
    void AddDeliberation(float time);
    void SetRemainTime(int remainTime);
}

public struct ScoreData
{
    public int remainTime;
    public int score;
    public int failedCount;
    public int collectedWizdomCount;
    public float deliberationDuration;
}

public class ScoreManager: MonoBehaviour, IScoreManager, IStageLoadable
{
    private int remainTime;
    public UnityEvent<int> onChangeFailedCount;
    public UnityEvent<int> onChangeWizdomCount;
    public int score {
        get
        {
            return remainTime * 100 - failedCount * 500 + collectedWizdomCount * 10000;
        }
    }
    public bool isGoodEnding
    {
        get
        {
            return deliberationDuration >= 20f && remainTime < 0;
        }
    }
    public int failedCount {get; private set;}
    public int collectedWizdomCount {get; private set;}
    public float deliberationDuration {get; private set;}
    public ScoreData scoreData
    {
        get{
            return new ScoreData{
                remainTime = this.remainTime,
                score = this.score,
                failedCount = this.failedCount,
                collectedWizdomCount = this.collectedWizdomCount,
                deliberationDuration = this.deliberationDuration
            };
        }
    }
    public void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        remainTime = 0;
        failedCount = 0;
        collectedWizdomCount = 0;
        deliberationDuration = 0f;
        onChangeFailedCount?.Invoke(0);
        onChangeWizdomCount?.Invoke(0);
    }
    public void OnLoadStage(int stageNo, StageData stageData)
    {
        Initialize();
    }
    public void AddFailedTask()
    {
        failedCount++;
        onChangeFailedCount?.Invoke(failedCount);
    }
    public void AddWizdom()
    {
        collectedWizdomCount++;
        onChangeWizdomCount?.Invoke(collectedWizdomCount);
    }
    public void AddDeliberation(float duration)
    {
        deliberationDuration += duration;
    }
    public void SetRemainTime(int remainTime)
    {
        this.remainTime = remainTime;
    }
}