using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Ending
{
    public class MonochromeChanger : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            StudentFaller student = other.GetComponent<StudentFaller>();
            if(student == null) return;
            student.TransMonochrome();
        }
    }
}