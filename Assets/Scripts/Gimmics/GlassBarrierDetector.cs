using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBarrierDetector : MonoBehaviour
{
    private bool isVertical;
    [SerializeField] private bool isInverted;
    public System.Action OnDetect;

    public void SetVertical(bool isVertical)
    {
        this.isVertical = isVertical;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Student>() == null) return;
        Rigidbody2D rigid = other.GetComponent<Rigidbody2D>();
        float axisVelocity = isVertical ? rigid.velocity.x : rigid.velocity.y;
        if(OverThreshold(axisVelocity)) OnDetect?.Invoke();
    }
    private bool OverThreshold(float axisVelocity)
    {
        if(isInverted) return axisVelocity > Constants.barrierThreshold;
        return axisVelocity < -Constants.barrierThreshold;
    }
}
