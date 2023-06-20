using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectMover : MonoBehaviour, IStudentBrain
{
    private IStudent student;

    public void SetTarget(IStudent student)
    {
        this.student = student;
    }

    void Awake()
    {
        student = GetComponent<IStudent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PauseManager.paused) return;
        student.SetHorizontal(Input.GetAxisRaw("Horizontal"));
        if(Input.GetButtonDown("Jump")) student.Jump();
    }
}
