using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityItem : MonoBehaviour
{
    private AssignmentGenerator generator;
    [SerializeField] private GameObject generatorObj;
    void Awake()
    {
        generator = generatorObj.GetComponent<AssignmentGenerator>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        IStudent student = other.GetComponent<IStudent>();
        if(student == null) return;
        generator.EarnPriorityAbility();
        Destroy(gameObject);
    }
}