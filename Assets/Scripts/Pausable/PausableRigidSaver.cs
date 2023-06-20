using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausableRigidSaver
{
    private bool isPreviousPaused = false;
    private bool isKinematic;
    private Vector2 position;
    private Vector2 velocity;
    private Rigidbody2D rigid;
    public PausableRigidSaver(Rigidbody2D rigid)
    {
        this.rigid = rigid;
    }
    public void Run()
    {
        if(isPreviousPaused == false && PauseManager.paused) Pause();
        else if(isPreviousPaused && PauseManager.paused == false) LoadRigid();
        isPreviousPaused = PauseManager.paused;
    }
    public void Pause()
    {
        SaveRigid();
        rigid.isKinematic = true;
        rigid.velocity = Vector2.zero;
    }
    public void SaveRigid()
    {
        isKinematic = rigid.isKinematic;
        velocity = rigid.velocity;
    }
    public void LoadRigid()
    {
        rigid.isKinematic = isKinematic;
        rigid.velocity = velocity;
    }
}