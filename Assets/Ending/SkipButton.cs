using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipButton : MonoBehaviour
{
    public string flag;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance == null) gameObject.SetActive(true);
        gameObject.SetActive(GameManager.Instance.GetFlag(flag));
    }
}
