using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorController : MonoBehaviour
{
    private bool invisible = false;
    private Indicator indicator;
    [SerializeField] private GameObject indicatorObj;

    // Start is called before the first frame update
    void Start()
    {
        indicator = indicatorObj.GetComponent<Indicator>();
    }

    void Update()
    {
        if(!invisible) return;
        Vector3 position = transform.position;
        indicator.DrawIndicator(position);
    }

    void OnBecameVisible()
    {
        invisible = false;
        if(indicatorObj.Equals(null)) return;
        indicatorObj.SetActive(false);
    }

    void OnBecameInvisible()
    {
        invisible = true;
        if(indicatorObj.Equals(null)) return;
        indicatorObj.SetActive(true);
    }
}