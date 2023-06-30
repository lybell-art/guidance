using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingCheckbox : MonoBehaviour
{
    [SerializeField] private string settingName;
    void Start()
    {
        Toggle toggler = GetComponent<Toggle>();
        toggler.isOn = GameManager.Instance.GetFlag("option_"+settingName, toggler.isOn);
    }
    public void OnChange(bool value)
    {
        GameManager.Instance.SaveFlag("option_"+settingName, value);
    }
}
