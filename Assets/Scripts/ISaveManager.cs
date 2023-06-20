using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveManager
{
    void SaveFlag(string name, bool flag);
    bool GetFlag(string name);
    void ApplySave();
}
