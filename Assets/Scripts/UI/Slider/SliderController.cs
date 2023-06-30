using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour, ISliderController
{
    private bool isMuted = false;
    private float originValue = 100f;
    private Slider slider;
    void Awake()
    {
        slider = GetComponent<Slider>();
    }
    public void Initialize(float initialValue)
    {
        slider.value = initialValue;
    }
    public void ToggleMute()
    {
        if(isMuted)
        {
            slider.value = originValue;
            isMuted = false;
        }
        else
        {
            originValue = slider.value;
            slider.value = 0f;
            isMuted = true;
        }
    }
    public void OnChange(float value)
    {
        isMuted = Mathf.Approximately(value, 0);
    }
}
