using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveManager
{
    void SaveFlag(string name, bool flag);
    bool GetFlag(string name);
    bool GetFlag(string name, bool defaultValue);

    void SaveFloat(string name, float value);
    float GetFloat(string name);
    float GetFloat(string name, float defaultValue);
    
    void ApplySave();
}
