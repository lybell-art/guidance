using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicSliderOption : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string channelName;
    void Start()
    {
        SliderController sliderController = GetComponent<SliderController>();
        float volume = GameManager.Instance.GetFloat("option_"+channelName, 100f);
        sliderController.Initialize(volume);
    }
    public void OnSliderChange(float value)
    {
        mixer.SetFloat(channelName, ConvertSliderToDecibel(value));
        GameManager.Instance.SaveFloat("option_"+channelName, value);
    }
    private float ConvertSliderToDecibel(float value)
    {
        if(value < 0.01f) return -80f;
        if(value > 100f) return 0f;
        return (Mathf.Log10(value) - 2f) * 20f;
    } 
}
