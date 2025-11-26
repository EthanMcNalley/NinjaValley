using UnityEngine;
using UnityEngine.Rendering;

public class ProceduralGenerator : MonoBehaviour
{
    public ComputeShader comShader;

    private Mesh terrainMesh;

    public Mesh meshToPlace;
    public Material mat;

    public ShadowCastingMode castShadows = ShadowCastingMode.On;
    public bool receiveShadows = true;

    public float 
        scale = 1.0f,
        minHight = .5f,
        maxHight = 1.5f;

    public float 
        minOffset = -.1f,
        maxOffset = -.1f;

    private GraphicsBuffer 
        terrainTriBuffer,
        terrainVertBuffer,
        transformMatrixBuffer,

        objTriBuffer,
        objVertBuffer,
        objUVBuffer;

    private Bounds boundBox;

    private MaterialPropertyBlock properties;

    private int kernel;
    private uint threadGroupSize;
    private int terrainTriCount = 0;

}
