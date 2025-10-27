using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bush : MonoBehaviour
{
    Material mat;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AttackHitBox"))
        {
            mat.SetColor("_BaseColor", Color.red);
            Destroy(gameObject, 2f);
        }
    }
}
