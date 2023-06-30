using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMVolumeInitializer : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {
        float bgmVolume = GameManager.Instance.GetFloat("option_bgm", 100f);
        float sfxVolume = GameManager.Instance.GetFloat("option_sfx", 100f);
        mixer.SetFloat("bgm", ConvertSliderToDecibel(bgmVolume));
        mixer.SetFloat("sfx", ConvertSliderToDecibel(sfxVolume));
    }
    private float ConvertSliderToDecibel(float value)
    {
        if(value < 0.01f) return -80f;
        if(value > 100f) return 0f;
        return (Mathf.Log10(value) - 2f) * 20f;
    } 
}
