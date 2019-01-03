using UnityEngine;

/// <summary>
/// Simulación de recintos acústicos con obstrucción, oclusión, etc.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class AudioGeometryBox : MonoBehaviour
{
    public float DirectOcclusion;// 0.0 no atenua, 1.0 atenua totalmente
    public float ReverbOcclusion;// atenuacion de la reverberacion
    public bool DoubleSided; // atenua por ambos lados o no 

    private FMOD.Geometry geometry;
    private int maxPoligons, maxVertices;

    private BoxCollider boxCollider;

    /// <summary>
    /// Crea la geometria
    /// </summary>
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();    
    }

    private void Start()
    {
        maxVertices = 8; // numero de vertices (>=3)

        FMOD.VECTOR[] vertices = new FMOD.VECTOR[maxVertices];

        //TODO: CREAR VERTICES
        //TODOS LOS VERTICES EN EL MISMO PLANO
        //POLIGONOS CONVEXOS
        //POLIGONOS CON AREA POSITIVA

        Vector3[] vertexCollider = GetColliderVertexPositions();

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new FMOD.VECTOR();
            vertices[i].x = vertexCollider[i].x;
            vertices[i].y = vertexCollider[i].y;
            vertices[i].z = vertexCollider[i].z;

            Debug.Log("Vertice " + i + " : x: " + vertices[i].x + " y: " + vertices[i].y + " z: " + vertices[i].z);
        }

        maxPoligons = 12;
        geometry = LowLevelSystem.Instance.CreateGeometry(maxPoligons, maxVertices);

        //TODO: PONER POSICIONES
        int polygonIndex; // Indice al poligono generado para referenciarlo despues
        LowLevelSystem.ERRCHECK(geometry.addPolygon(DirectOcclusion, ReverbOcclusion, DoubleSided, maxVertices, vertices, out polygonIndex));
    }

    //TODO: SENTIDO ANTIHORARIO Y SUPERFICIES CONVEXAS
    private Vector3[] GetColliderVertexPositions()
    {
        Vector3[] vertices = new Vector3[8];

        vertices[0] = transform.TransformPoint(boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f);
        vertices[1] = transform.TransformPoint(boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f);
        vertices[2] = transform.TransformPoint(boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f);
        vertices[3] = transform.TransformPoint(boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f);
        vertices[4] = transform.TransformPoint(boxCollider.center + new Vector3(-boxCollider.size.x, boxCollider.size.y, -boxCollider.size.z) * 0.5f);
        vertices[5] = transform.TransformPoint(boxCollider.center + new Vector3(boxCollider.size.x, boxCollider.size.y, -boxCollider.size.z) * 0.5f);
        vertices[6] = transform.TransformPoint(boxCollider.center + new Vector3(boxCollider.size.x, boxCollider.size.y, boxCollider.size.z) * 0.5f);
        vertices[7] = transform.TransformPoint(boxCollider.center + new Vector3(-boxCollider.size.x, boxCollider.size.y, boxCollider.size.z) * 0.5f);

        return vertices;
    }
}
