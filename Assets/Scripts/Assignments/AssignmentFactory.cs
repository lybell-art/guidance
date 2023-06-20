using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentFactory : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    public Assignment CreateAssignment(Vector3 position, AssignmentType type, params object[] additionalArgs)
    {
        if((int)type >= prefabs.Length) return null;
        GameObject prefab = prefabs[(int)type];
        if(prefab == null) return null;
        
        GameObject instance = Instantiate(prefab, position, Quaternion.identity);
        Assignment assignment = instance.GetComponent<Assignment>();
        assignment.Initialize(additionalArgs);
        return assignment;
    }
}