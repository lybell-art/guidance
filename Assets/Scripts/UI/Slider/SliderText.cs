using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliderText : MonoBehaviour
{
    private TextMeshProUGUI textUI;
    void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }
    public void OnChange(float value)
    {
        textUI.SetText(((int)value).ToString());
    }
}
