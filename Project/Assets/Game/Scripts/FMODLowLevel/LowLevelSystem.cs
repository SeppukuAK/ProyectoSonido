using UnityEngine;

/// <summary>
/// Motor de audio que permite el posicionamiento 3D, geometría y reverbs
/// Inicializa la librería. Se encarga de cargar y reproducir sonidos.
/// </summary>
public class LowLevelSystem : MonoBehaviour
{
    public static LowLevelSystem Instance;

    /// <summary>
    /// Instancia de LowLevelSystem
    /// </summary>
    private FMOD.System system;
    private FMOD.ChannelGroup masterChannel;

    /// <summary>
    /// Obtiene la instancia del LowLevelSystem
    /// </summary>
    private void Awake()
    {
        Instance = this;

        //LowLevel
        system = FMODUnity.RuntimeManager.LowlevelSystem;
        uint version;
        ERRCHECK(system.getVersion(out version));
        Debug.Log("LowLevelSystem version: " + version);
    }

    void Update()
    {
        //TODO: REVISAR SI HACE FALTA
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
        //TODO: COMPROBAR
        ERRCHECK(system.createSound("Assets/Game/Audio" + name + ".wav", FMOD.MODE._3D | FMOD.MODE.LOOP_NORMAL, out sound));
        return sound;
    }

    #region Channel

    /// <summary>
    /// Crea un canal asociado al sonido
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

    #endregion Channel

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
