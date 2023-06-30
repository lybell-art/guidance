using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMBackgroundPauser : MonoBehaviour
{
    private AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnApplicationPaused(bool outFocus)
    {
        if(outFocus) audioSource.Pause();
        else audioSource.UnPause();
    }
}
