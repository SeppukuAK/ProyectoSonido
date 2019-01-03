using UnityEngine;

/// <summary>
/// Motor de audio que permite el posicionamiento 3D, geometría y reverbs
/// Inicializa la librería. Se encarga de cargar y reproducir sonidos.
/// </summary>
public class LowLevelSystem : MonoBehaviour
{
    #region Singleton

    public static LowLevelSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion Singleton

    /// <summary>
    /// Ruta donde se encuentran los archivos de audio de la escena
    /// </summary>
    [SerializeField] private string audioPath;

    /// <summary>
    /// Instancia de LowLevelSystem
    /// </summary>
    private FMOD.System system;

    /// <summary>
    /// Canal padre de todos
    /// </summary>
    private FMOD.ChannelGroup masterChannel;

    /// <summary>
    /// Obtiene la instancia del LowLevelSystem
    /// </summary>
    private void Start()
    {
        //LowLevel
        system = FMODUnity.RuntimeManager.LowlevelSystem;
        uint version;
        ERRCHECK(system.getVersion(out version));
        Debug.Log("LowLevelSystem version: " + version);
    }

    /// <summary>
    /// TODO: Update del system. Revisar si hace falta
    /// </summary>
    private void Update()
    {
        ERRCHECK(system.update());
    }

    /// <summary>
    /// Crea un sonido 3D
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public FMOD.Sound Create3DSound(string name)
    {
        FMOD.Sound sound;
        //TODO: Solo va con .wav
        ERRCHECK(system.createSound(audioPath + name + ".wav", FMOD.MODE._3D | FMOD.MODE.LOOP_NORMAL, out sound));
        return sound;
    }

    /// <summary>
    /// Crea un canal asociado al sonido
    /// Arranca en pause para dejarlo disponible en memoria
    /// </summary>
    /// <param name="sound"></param>
    /// <returns></returns>
    public FMOD.Channel CreateChannel(FMOD.Sound sound)
    {
        FMOD.Channel channel;
        //Se crea el canal
        ERRCHECK(system.playSound(
             sound,
             masterChannel, // grupo de canales, 0 sin agrupar (agrupado en el master)
             true, // arranca con "pause" 
             out channel));

        ERRCHECK(channel.setLoopCount(0));
        return channel;
    }

    /// <summary>
    /// Crea un grupo de canales con un nombre
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public FMOD.ChannelGroup CreateChannelGroup(string name)
    {
        FMOD.ChannelGroup channelGroup;
        ERRCHECK(system.createChannelGroup(name,out channelGroup));
        return channelGroup;
    }

    /// <summary>
    /// TODO: Revisar
    /// Crea geometria
    /// </summary>
    /// <param name="maxPoligons"></param>
    /// <param name="maxVertex"></param>
    /// <returns></returns>
    public FMOD.Geometry CreateGeometry(int maxPoligons, int maxVertex)
    {
        FMOD.Geometry geometry;
        ERRCHECK(system.createGeometry(maxPoligons, maxVertex, out geometry));
        return geometry;
    }

    /// <summary>
    /// Facilita la gestión de errores
    /// </summary>
    /// <param name="result"></param>
    public static void ERRCHECK(FMOD.RESULT result)
    {
        if (result != FMOD.RESULT.OK)
            Debug.LogError(result);
    }
}
