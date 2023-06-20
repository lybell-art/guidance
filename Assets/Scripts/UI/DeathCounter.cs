using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCounter : MonoBehaviour
{
    private TextMeshProUGUI textUI;

    void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    public void OnChangeFailedCount(int failCount)
    {
        textUI.SetText(failCount.ToString());
    }
}
