using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalResult : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!GameManager.Instance.GetFlag("seen_badend") && !GameManager.Instance.GetFlag("seen_goodend"))
        {
            gameObject.SetActive(false);
        }
    }
}
