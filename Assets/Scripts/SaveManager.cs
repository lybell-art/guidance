using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorySaveManager : ISaveManager
{
    private Dictionary<string, bool> tempSaver = new Dictionary<string, bool>();
    public void SaveFlag(string key, bool flag)
    {
        tempSaver[key] = flag;
    }
    public bool GetFlag(string key)
    {
        if(tempSaver.ContainsKey(key)) return tempSaver[key];
        return false;
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
    public void ApplySave()
    {
        PlayerPrefs.Save();
    }
}