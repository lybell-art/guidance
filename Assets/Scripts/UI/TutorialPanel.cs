using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialPanel : MonoBehaviour, IPointerClickHandler
{
    void Awake()
    {
        if(GameManager.Instance == null) gameObject.SetActive(false);
        if(GameManager.Instance.GetFlag("tutorial") == false) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
    void OnEnable()
    {
        PauseManager.Instance?.Pause(1);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        PauseManager.Instance?.Resume(1);
        gameObject.SetActive(false);
        GameManager.Instance?.SaveFlag("tutorial", true);
    }
}
