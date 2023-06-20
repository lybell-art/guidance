using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseClickableToggler : MonoBehaviour
{
    private PhysicsRaycaster raycaster;
    private LayerMask layerMask;
    void Awake()
    {
        raycaster = GetComponent<PhysicsRaycaster>();
        layerMask = raycaster.eventMask;
    }
    void OnEnable()
    {
        if(PauseManager.Instance == null) return;
        PauseManager.Instance.OnPauseEvent += OnPause;
        PauseManager.Instance.OnResumeEvent += OnResume;
    }
    void OnDisable()
    {
        if(PauseManager.Instance == null) return;
        PauseManager.Instance.OnPauseEvent -= OnPause;
        PauseManager.Instance.OnResumeEvent -= OnResume;
    }
    void OnPause()
    {
        raycaster.eventMask = 1 << Constants.uiLayer;
    }
    void OnResume()
    {
        raycaster.eventMask = layerMask;
    }
}
