using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBarrier : MonoBehaviour, IBarrier
{
    private Animator animator;
    private Collider2D platformCollider;
    private GameObject topDetectorObj;
    private GameObject bottomDetectorObj;
    private GlassBarrierDetector topDetector;
    private GlassBarrierDetector bottomDetector;

    private bool _isVertical = false;
    public bool isVertical
    {
        get{return _isVertical;}
    }
    public Vector2 position
    {
        get{return transform.position;}
    }
    public bool isBroken{get; private set;}

    void Awake()
    {
        animator = GetComponent<Animator>();
        platformCollider = transform.GetChild(0).GetComponent<Collider2D>();

        topDetectorObj = transform.GetChild(1).gameObject;
        bottomDetectorObj = transform.GetChild(2).gameObject;

        topDetector = topDetectorObj.GetComponent<GlassBarrierDetector>();
        bottomDetector = bottomDetectorObj.GetComponent<GlassBarrierDetector>();

        topDetector.OnDetect += Disable;
        bottomDetector.OnDetect += DisableInverted;
    }

    // Start is called before the first frame update
    void Start()
    {
        _isVertical = Mathf.Approximately(transform.localEulerAngles.z, 270f);
        topDetector.SetVertical(_isVertical);
        bottomDetector.SetVertical(_isVertical);
    }

    public void Enable()
    {
        SetEnableStatus(true);
    }

    private void _Disable()
    {
        SetEnableStatus(false);
    }

    private void SetEnableStatus(bool isEnable)
    {
        this.isBroken = !isEnable;
        platformCollider.enabled = isEnable;
        topDetectorObj.SetActive(isEnable);
        bottomDetectorObj.SetActive(isEnable);
        animator.SetBool("broken", !isEnable);
    }

    public void Disable()
    {
        _Disable();
        animator.SetBool("inverted", false);
    }

    public void DisableInverted()
    {
        _Disable();
        animator.SetBool("inverted", true);
    }
}
