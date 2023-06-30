using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorySaveManager : ISaveManager
{
    private Dictionary<string, bool> tempSaver = new Dictionary<string, bool>();
    private Dictionary<string, float> tempSaverFloat = new Dictionary<string, float>();
    public void SaveFlag(string key, bool flag)
    {
        tempSaver[key] = flag;
    }
    public bool GetFlag(string key)
    {
        if(tempSaver.ContainsKey(key)) return tempSaver[key];
        return false;
    }
    public void SaveFloat(string key, float value)
    {
        tempSaverFloat[key] = value;
    }
    public float GetFloat(string key)
    {
        return GetFloat(key, 0f);
    }
    public float GetFloat(string key, float defaultValue)
    {
        if(tempSaverFloat.ContainsKey(key)) return tempSaverFloat[key];
        return defaultValue;
    }
    public void ApplySave()
    {

    }
}

public class PrefsSaveManager : ISaveManager
{
    public void SaveFlag(string key, bool flag)
    {
        PlayerPrefs.SetInt(key, flag ? 1 : 0);
    }
    public bool GetFlag(string key)
    {
        int flagData = PlayerPrefs.GetInt(key, 0);
        return flagData != 0;
    }
    public void SaveFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key, 0f);
    }
    public float GetFloat(string key, float defaultValue)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public void ApplySave()
    {
        PlayerPrefs.Save();
    }
}