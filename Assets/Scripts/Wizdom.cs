using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizdom : MonoBehaviour
{
    public string Tooltip="Test Tooltip";
    void OnTriggerEnter2D(Collider2D other)
    {
        Deliberator student = other.GetComponent<Deliberator>();
        if(student == null) return;
        student.AddWizdom();
        StudentSoundManager soundPlayer = other.GetComponent<StudentSoundManager>();
        if(soundPlayer != null) soundPlayer.PlayGetItem();
        Destroy(gameObject);
    }
}
