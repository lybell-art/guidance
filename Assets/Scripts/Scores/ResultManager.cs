using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    private TextMeshProUGUI failCountUI;
    private TextMeshProUGUI reachTimeUI;
    private TextMeshProUGUI scoreUI;
    private TextMeshProUGUI wisdomUI;
    private TextMeshProUGUI deliberationUI;
    [SerializeField] private GameObject failCountObj;
    [SerializeField] private GameObject remainTimeObj;
    [SerializeField] private GameObject scoreObj;
    [SerializeField] private GameObject wisdomObj;
    [SerializeField] private GameObject deliberationObj;
    void Awake()
    {
        failCountUI = failCountObj.GetComponent<TextMeshProUGUI>();
        reachTimeUI = remainTimeObj.GetComponent<TextMeshProUGUI>();
        scoreUI = scoreObj.GetComponent<TextMeshProUGUI>();
        wisdomUI = wisdomObj.GetComponent<TextMeshProUGUI>();
        deliberationUI = deliberationObj.GetComponent<TextMeshProUGUI>();
    }
    public void SetScoreData(ScoreData scoreData)
    {
        failCountUI.SetText(scoreData.failedCount.ToString());
        reachTimeUI.SetText(GetTimeString(scoreData.remainTime));
        scoreUI.SetText(scoreData.score.ToString());
        wisdomUI.SetText(scoreData.collectedWizdomCount.ToString());
        deliberationUI.SetText(GetTimeString((int)scoreData.deliberationDuration));
    }
    private string GetTimeString(int time)
    {
        bool isMinus = time < 0;
        int _time = isMinus ? -time : time;
        int minute = _time / 60;
        string seconds = (_time % 60).ToString("D2");

        return $"{(isMinus ? "-" : "")}{minute}:{seconds}";
    }
}
