using UnityEngine;

public class InstantiateonVertexColor : MonoBehaviour
{
    public GameObject prefabtoSpawn;
    public Color targetColor = Color.black;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter != null )
        {
            return;
        }

        Mesh mesh = meshFilter.mesh;
        Vector3[] verti = mesh.vertices;
        Color[] colors = mesh.colors;

        for (int i = 0; i < verti.Length; i++)
        {
            if (Vector4.Distance(colors[i], targetColor) < 0)
            {
                Vector3 spawnPos = transform.TransformPoint(verti[i]);
                //spawnPos = spawnPos.normalized;

                Instantiate(prefabtoSpawn, spawnPos, Quaternion.identity, transform);
            }
        }
    }

    // Update is called once per frame
   // void Update()
   // {
   //     
   // }
}
