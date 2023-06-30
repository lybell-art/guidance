using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseUIManager : MonoBehaviour
{
    private PauseUI pauseUIController;
    [SerializeField] private GameObject pauseUIObj;
    void Awake()
    {
        pauseUIController = pauseUIObj.GetComponent<PauseUI>();
    }
    void OnEnable()
    {
        if(PauseManager.Instance == null) return;
        PauseManager.Instance.OnChannelPauseEvent += EnableUI;
        PauseManager.Instance.OnChannelResumeEvent += DisableUI;
    }
    void OnDisable()
    {
        if(PauseManager.Instance == null) return;
        PauseManager.Instance.OnChannelPauseEvent -= EnableUI;
        PauseManager.Instance.OnChannelResumeEvent -= DisableUI;
    }
    void EnableUI(int channel)
    {
        if(channel == 0) pauseUIController.Show();
    }
    void DisableUI(int channel)
    {
        if(channel == 0) pauseUIController.Hide();
    }
}
