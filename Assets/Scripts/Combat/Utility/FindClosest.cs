using UnityEngine;

public static class FindClosest
{
    /*public static GameObject FindClosestGameObject(Vector3 position, float radius, LayerMask layerMask)
    {
        if (layerMask == 0)
        {
            return null;
        }
        Collider[] hits = Physics.OverlapSphere(position, radius, layerMask);
        
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

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
    }*/
    
    private static readonly Collider[] hits = new Collider[32];
    public static GameObject FindClosestGameObject(Vector3 position, float radius, LayerMask layerMask)
    {
        if (layerMask == 0)
        {
            return null;
        }
        int count = Physics.OverlapSphereNonAlloc(position, radius, hits, layerMask);
        
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < count; i++)
        {
            var collider = hits[i];
            if (!collider) continue;

            float distSq = (collider.transform.position - position).sqrMagnitude;
            if (distSq < minDistance)
            {
                minDistance = distSq;
                closest = collider.gameObject;
            }
        }
        return closest;
    }
}
