using System;
using UnityEngine;

public class ColliderGizmo : MonoBehaviour
{
    public Color color;
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(GetComponent<Collider>().bounds.center, GetComponent<Collider>().bounds.size);
    }
}
