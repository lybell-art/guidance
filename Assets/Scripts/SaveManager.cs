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
        return GetFlag(key, false);
    }
    public bool GetFlag(string key, bool defaultValue)
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
    public bool HasKeyBool(string key)
    {
        return tempSaver.ContainsKey(key);
    }
    public bool HasKeyFloat(string key)
    {
        return tempSaverFloat.ContainsKey(key);
    }
    public void Reset()
    {
        tempSaver.Clear();
        tempSaverFloat.Clear();
    }
}

public class PrefsSaveManager : ISaveManager
{
    private MemorySaveManager cache = new MemorySaveManager();
    public void SaveFlag(string key, bool flag)
    {
        PlayerPrefs.SetInt(key, flag ? 1 : 0);
        cache.SaveFlag(key, flag);
    }
    public bool GetFlag(string key)
    {
        return GetFlag(key, false);
    }
    public bool GetFlag(string key, bool defaultValue)
    {
        if(cache.HasKeyBool(key)) return cache.GetFlag(key, defaultValue);

        int flagData = PlayerPrefs.GetInt(key, defaultValue ? 1 : 0);
        cache.SaveFlag(key, flagData != 0);
        return flagData != 0;
    }
    public void SaveFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        cache.SaveFloat(key, value);
    }
    public float GetFloat(string key)
    {
        return GetFloat(key, 0f);
    }
    public float GetFloat(string key, float defaultValue)
    {
        if(cache.HasKeyFloat(key)) return cache.GetFloat(key, defaultValue);

        float result = PlayerPrefs.GetFloat(key, defaultValue);
        cache.SaveFloat(key, result);
        return result;
    }

    public void ApplySave()
    {
        PlayerPrefs.Save();
    }
    public void Reset()
    {
        cache.Reset();
        PlayerPrefs.DeleteAll();
    }
}