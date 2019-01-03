﻿using UnityEngine;

/// <summary>
/// Motor de audio que permite el posicionamiento 3D, geometría y reverbs
/// Inicializa la librería. Se encarga de cargar y reproducir sonidos.
/// </summary>
public class LowLevelSystem : MonoBehaviour
{
    public static LowLevelSystem Instance;

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
    private void Awake()
    {
        Instance = this;

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
        ERRCHECK(system.createChannelGroup(name, out channelGroup));
        return channelGroup;
    }

    /// <summary>
    /// Crea una reverb
    /// </summary>
    /// <returns></returns>
    public FMOD.Reverb3D CreateReverb()
    {
        FMOD.Reverb3D reverb;
        ERRCHECK(system.createReverb3D(out reverb));
        return reverb;
    }

    /// <summary>
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
    /// Crea un DSP
    /// </summary>
    /// <param name="DSPType"></param>
    /// <returns></returns>
    public FMOD.DSP CreateDSP(FMOD.DSP_TYPE DSPType)
    {
        FMOD.DSP dsp;
        ERRCHECK(system.createDSPByType(DSPType, out dsp));
        return dsp;
    }

    #region System parameters

    /// <summary>
    /// Devuelve la variación de frecuencia por la velocidad
    /// 1.0 = valor natural
    /// 0.0 = Lo anula
    /// >1.0 => Exagera el fenómeno 
    /// </summary>
    /// <returns></returns>
    public float GetDopplerScale()
    {
        float dopplerScale, distanceFactor, rollOffScale;
        ERRCHECK(system.get3DSettings(out dopplerScale, out distanceFactor, out rollOffScale));
        return dopplerScale;
    }

    /// <summary>
    /// Devuelve las dimensiones del escenario de cara al motor de sonido
    /// 1.0 = valor natural
    /// 0.0 = Lo anula
    /// >1.0 => Exagera el fenómeno 
    /// </summary>
    /// <returns></returns>
    public float GetDistanceFactor()
    {
        float dopplerScale, distanceFactor, rollOffScale;
        ERRCHECK(system.get3DSettings(out dopplerScale, out distanceFactor, out rollOffScale));
        return distanceFactor;
    }

    /// <summary>
    /// Devuelve la atenuación con la distancia
    /// 1.0 = valor natural
    /// 0.0 = Lo anula
    /// >1.0 => Exagera el fenómeno 
    /// </summary>
    /// <returns></returns>
    public float GetRollOffScale()
    {
        float dopplerScale, distanceFactor, rollOffScale;
        ERRCHECK(system.get3DSettings(out dopplerScale, out distanceFactor, out rollOffScale));
        return rollOffScale;
    }

    /// <summary>
    /// Establece la variación de frecuencia por la velocidad
    /// </summary>
    /// <param name="dopplerScale">   
    /// 1.0 = valor natural
    /// 0.0 = Lo anula
    /// >1.0 => Exagera el fenómeno 
    /// </param>
    public void SetDopplerScale(float dopplerScale)
    {
        ERRCHECK(system.set3DSettings(dopplerScale, GetDistanceFactor(), GetRollOffScale()));
    }

    /// <summary>
    /// Establece las dimensiones del escenario de cara al motor de sonido
    /// </summary>
    /// <param name="rollOffScale">   
    /// 1.0 = valor natural
    /// 0.0 = Lo anula
    /// >1.0 => Exagera el fenómeno 
    /// </param>
    public void SetDistanceFactor(float distanceFactor)
    {
        ERRCHECK(system.set3DSettings(GetDopplerScale(), distanceFactor, GetRollOffScale()));
    }

    /// <summary>
    /// Establece la atenuación con la distancia
    /// </summary>
    /// <param name="rollOffScale">   
    /// 1.0 = valor natural
    /// 0.0 = Lo anula
    /// >1.0 => Exagera el fenómeno 
    /// </param>
    public void SetRollOffScale(float rollOffScale)
    {
        ERRCHECK(system.set3DSettings(GetDopplerScale(), GetDistanceFactor(), rollOffScale));
    }

    #endregion System parameters

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
