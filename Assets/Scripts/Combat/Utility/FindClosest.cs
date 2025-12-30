using UnityEngine;

public static class FindClosest
{
    public static GameObject FindClosestGameObject(Vector3 position, float radius, LayerMask layerMask)
    {
        Collider[] hits = Physics.OverlapSphere(position, radius, layerMask);
        
        GameObject closest = null;
        float minDistance = float.MaxValue;

        foreach (Collider hit in hits )
        {
            float distSq = (hit.transform.position - position).sqrMagnitude;

            if (distSq < minDistance)
            {
                minDistance = distSq;
                closest = hit.gameObject;
            }
        }

        return closest;
    }
}
