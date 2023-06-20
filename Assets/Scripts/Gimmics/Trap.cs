using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        IAssignmentRunner student = other.GetComponent<IAssignmentRunner>();
        if(student == null) return;
        student.HitObstacle();
    }
}
