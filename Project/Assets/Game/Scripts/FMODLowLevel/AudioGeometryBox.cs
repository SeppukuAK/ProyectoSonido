using UnityEngine;

/// <summary>
/// Simulación de recintos acústicos con obstrucción, oclusión, etc.
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class AudioGeometryBox : MonoBehaviour
{
    //Oclusión: emisor y oyente el distinto recinto
    //todas las ondas están atenuadas y filtradas

    public float DirectOcclusion;// 0.0 no atenua, 1.0 atenua totalmente
    public float ReverbOcclusion;// atenuacion de la reverberacion
    public bool DoubleSided; // atenua por ambos lados o no 

    private FMOD.Geometry geometry;
    private int maxPoligons, maxVertices;
    private MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    /// <summary>
    /// Se crea la geometría del cubo
    /// </summary>
    private void Start()
    {
        maxVertices = 3; // numero de vertices  
        maxPoligons = 2; // numero de poligonos

        //Obtenemos los vertices
        FMOD.VECTOR[] vertexCollider = GetColliderVertexPositions();

        //Se crean las caras del cubo
        FMOD.VECTOR[][] Faces = new FMOD.VECTOR[maxPoligons][];
        Faces[0] = new FMOD.VECTOR[] { vertexCollider[0], vertexCollider[2], vertexCollider[1] };
        Faces[1] = new FMOD.VECTOR[] { vertexCollider[1], vertexCollider[3], vertexCollider[0] };


        geometry = LowLevelSystem.Instance.CreateGeometry(maxPoligons, maxVertices * maxPoligons);

        int polygonIndex; // Indice al poligono generado para referenciarlo despues
        for (int i = 0; i < maxPoligons; i++)
            LowLevelSystem.ERRCHECK(geometry.addPolygon(DirectOcclusion, ReverbOcclusion, DoubleSided, maxVertices, Faces[i], out polygonIndex));

    }

    private void Update()
    {
        UpdateGeometryTransform();
    }

    private void OnDestroy()
    {
        geometry.setActive(false);
    }

    /// <summary>
    /// Modifica la posicion, rotación y escala al objeto
    /// </summary>
    private void UpdateGeometryTransform()
    {
        FMOD.VECTOR pos = new FMOD.VECTOR();
        pos.x = transform.position.x;
        pos.y = transform.position.y;
        pos.z = transform.position.z;

        LowLevelSystem.ERRCHECK(geometry.setPosition(ref pos));

        FMOD.VECTOR forward = new FMOD.VECTOR();
        forward.x = transform.forward.x;
        forward.y = transform.forward.y;
        forward.z = transform.forward.z;

        FMOD.VECTOR up = new FMOD.VECTOR();
        up.x = transform.up.x;
        up.y = transform.up.y;
        up.z = transform.up.z;

        LowLevelSystem.ERRCHECK(geometry.setRotation(ref forward, ref up));

    }

    /// <summary>
    /// Realiza la conversión de Vector3 a FMOD.VECTOR
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    private FMOD.VECTOR Vector3ToFMODVector(Vector3 v)
    {
        FMOD.VECTOR fmodVector = new FMOD.VECTOR();
        fmodVector.x = v.x *transform.lossyScale.x;
        fmodVector.y = v.y * transform.lossyScale.y;
        fmodVector.z = v.z * transform.lossyScale.z;
            
        return fmodVector;
    }

    /// <summary>
    /// Devuelve los vértices del cubo
    /// </summary>
    /// <returns></returns>
    private FMOD.VECTOR[] GetColliderVertexPositions()
    {
        FMOD.VECTOR[] vertices = new FMOD.VECTOR[4];

        vertices[0] = Vector3ToFMODVector(meshFilter.mesh.vertices[0]);//(0) -0.5,-0.5
        vertices[1] = Vector3ToFMODVector(meshFilter.mesh.vertices[1]);//(1) 0.5,0.5,0.0
        vertices[2] = Vector3ToFMODVector(meshFilter.mesh.vertices[2]);//(2) 0.5,-0.5
        vertices[3] = Vector3ToFMODVector(meshFilter.mesh.vertices[3]);//(3)-0.5,0.5

        return vertices;
    }
}
