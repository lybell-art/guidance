using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovableAttachable
{
    bool isAttached{get;}
    void Attach(IMovablePlatform platform);
    void Detach(IMovablePlatform platform);
}

public class MovingAttachment : MonoBehaviour, IMovableAttachable
{
    private Rigidbody2D rigid;
    private IMovablePlatform platform;
    private Vector2 offset;
    public bool isAttached{get; private set;}
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Attach(IMovablePlatform platform)
    {
        if(this.platform == null || this.platform.Equals(null))
        {
            this.platform = platform;
        }
    }
    public void Detach(IMovablePlatform platform)
    {
        if(this.platform == platform) this.platform = null;
    }
    void FixedUpdate()
    {
        if(PauseManager.paused) return;
        
        if(platform == null) return;
        if(platform.Equals(null))
        {
            platform = null;
            return;
        }
        if(!platform.isMoving)
        {
        	offset = platform.position - rigid.position;
        }
        if(rigid.velocity == Vector2.zero)
        {
            rigid.MovePosition(platform.position - offset);
        }
    }
}