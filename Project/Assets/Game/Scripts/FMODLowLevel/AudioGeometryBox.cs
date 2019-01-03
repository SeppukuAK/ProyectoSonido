using UnityEngine;

/// <summary>
/// Simulación de recintos acústicos con obstrucción, oclusión, etc.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class AudioGeometryBox : MonoBehaviour
{
    //Oclusión: emisor y oyente el distinto recinto
    //todas las ondas están atenuadas y filtradas

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

    private void Update()
    {
        UpdateGeometryTransform();
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

        FMOD.VECTOR scale = new FMOD.VECTOR();
        scale.x = transform.lossyScale.x;
        scale.y = transform.lossyScale.y;
        scale.z = transform.lossyScale.z;

        LowLevelSystem.ERRCHECK(geometry.setScale(ref scale));
    }
    /// <summary>
    /// Se crea la geometría del cubo
    /// </summary>
    private void Start()
    {
        maxVertices = 4; // numero de vertices  
        maxPoligons = 6; // numero de poligonos

        //Obtenemos los vertices
        FMOD.VECTOR[] vertexCollider = GetColliderVertexPositions();

        //Se crean las caras del cubo
        FMOD.VECTOR[][] Faces = new FMOD.VECTOR[maxPoligons][];
        Faces[0] = new FMOD.VECTOR[] { vertexCollider[0], vertexCollider[1], vertexCollider[2], vertexCollider[3] };
        Faces[1] = new FMOD.VECTOR[] { vertexCollider[4], vertexCollider[0], vertexCollider[3], vertexCollider[7] };
        Faces[2] = new FMOD.VECTOR[] { vertexCollider[5], vertexCollider[4], vertexCollider[7], vertexCollider[6] };
        Faces[3] = new FMOD.VECTOR[] { vertexCollider[1], vertexCollider[5], vertexCollider[6], vertexCollider[2] };
        Faces[4] = new FMOD.VECTOR[] { vertexCollider[3], vertexCollider[2], vertexCollider[6], vertexCollider[7] };
        Faces[5] = new FMOD.VECTOR[] { vertexCollider[1], vertexCollider[0], vertexCollider[4], vertexCollider[5] };


        geometry = LowLevelSystem.Instance.CreateGeometry(maxPoligons, maxVertices* maxPoligons);

        int polygonIndex; // Indice al poligono generado para referenciarlo despues
        for (int i = 0; i < maxPoligons; i++)       
            LowLevelSystem.ERRCHECK(geometry.addPolygon(DirectOcclusion, ReverbOcclusion, DoubleSided, maxVertices, Faces[i], out polygonIndex));
        
    }

    /// <summary>
    /// Realiza la conversión de Vector3 a FMOD.VECTOR
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    private FMOD.VECTOR Vector3ToFMODVector(Vector3 v)
    {
        FMOD.VECTOR fmodVector = new FMOD.VECTOR();
        fmodVector.x = v.x;
        fmodVector.x = v.y;
        fmodVector.x = v.z;

        return fmodVector;
    }

    /// <summary>
    /// Devuelve los vértices del cubo
    /// </summary>
    /// <returns></returns>
    private FMOD.VECTOR[] GetColliderVertexPositions()
    {
        FMOD.VECTOR[] vertices = new FMOD.VECTOR[8];

        //vertices del box collider
        vertices[0] = Vector3ToFMODVector(new Vector3(boxCollider.center.x + boxCollider.size.x / 2, boxCollider.center.y - boxCollider.size.y / 2, boxCollider.center.z + boxCollider.size.z / 2));//(0)
        vertices[1] = Vector3ToFMODVector(new Vector3(boxCollider.center.x - boxCollider.size.x / 2, boxCollider.center.y - boxCollider.size.y / 2, boxCollider.center.z + boxCollider.size.z / 2));//(1)
        vertices[2] = Vector3ToFMODVector(new Vector3(boxCollider.center.x - boxCollider.size.x / 2, boxCollider.center.y + boxCollider.size.y / 2, boxCollider.center.z + boxCollider.size.z / 2));//(2)
        vertices[3] = Vector3ToFMODVector(new Vector3(boxCollider.center.x + boxCollider.size.x / 2, boxCollider.center.y + boxCollider.size.y / 2, boxCollider.center.z + boxCollider.size.z / 2)); //(3)

        vertices[4] = Vector3ToFMODVector(new Vector3(boxCollider.center.x + boxCollider.size.x / 2, boxCollider.center.y - boxCollider.size.y / 2, boxCollider.center.z - boxCollider.size.z / 2));///(4)
        vertices[5] = Vector3ToFMODVector(new Vector3(boxCollider.center.x - boxCollider.size.x / 2, boxCollider.center.y - boxCollider.size.y / 2, boxCollider.center.z - boxCollider.size.z / 2));//(5)
        vertices[6] = Vector3ToFMODVector(new Vector3(boxCollider.center.x - boxCollider.size.x / 2, boxCollider.center.y + boxCollider.size.y / 2, boxCollider.center.z - boxCollider.size.z / 2));//(6)
        vertices[7] = Vector3ToFMODVector(new Vector3(boxCollider.center.x + boxCollider.size.x / 2, boxCollider.center.y + boxCollider.size.y / 2, boxCollider.center.z - boxCollider.size.z / 2));//(7)

        return vertices;
    }
}
