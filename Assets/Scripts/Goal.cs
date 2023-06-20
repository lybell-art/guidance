using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour, IStageLoadable
{
    private DeliberateTaskGenerator deliberateTaskGenerator;
    void Awake()
    {
        deliberateTaskGenerator = GetComponent<DeliberateTaskGenerator>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        Student student = other.GetComponent<Student>();
        if(student == null) return;
        deliberateTaskGenerator.deliberateAssignment?.FinishTask();
        GameManager.Instance.GameClear();
    }
    public void Initialize()
    {
    }
    public void OnLoadStage(int stageNo, StageData stageData)
    {
        transform.position = stageData.endPoint;
    }
}
