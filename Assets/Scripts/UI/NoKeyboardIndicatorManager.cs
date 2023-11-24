using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoKeyboardIndicatorManager : MonoBehaviour
{
    private NoKeyboardIndicator indicator;
    [SerializeField] private GameObject ui;
    void Awake()
    {
        indicator = ui.GetComponent<NoKeyboardIndicator>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical") || Input.GetKeyDown(KeyCode.Space))
        {
            if(GameManager.Instance?.GetFlag("noKeyboard") != true)
            {
                ui.SetActive(true);
                indicator.Activate();
            }
        }
    }
}
