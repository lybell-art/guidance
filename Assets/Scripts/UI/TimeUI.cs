using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    private TextMeshProUGUI textUI;
    private Color redColor = new Color(1f, 0.2f, 0.2f, 1f);

    void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    public void OnTick(int time)
    {
        bool isMinus = time < 0;
        int _time = isMinus ? -time : time;
        int minute = _time / 60;
        string seconds = (_time % 60).ToString("D2");
        textUI.color = isMinus ? redColor : Color.white;

        textUI.SetText($"{minute}:{seconds}");
    }
}
