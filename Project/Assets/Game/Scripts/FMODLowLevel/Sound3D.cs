using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Controla parámetros del canal de reproducción
/// Tiene una posición en el espacio
/// </summary>
public class Sound3D : MonoBehaviour
{
    private enum SoundState { READY, PLAYING, PAUSED }

    public AudioClip Clip; // Pista de audio que reproduce

    //Atributos del canal
    public bool Mute = false;
    public bool PlayOnAwake = true;
    public bool Loop = false;

    [Range(0f, 1f)] public float Volume = 1f;
    [Range(0f, 6f)] public float Pitch = 1f;

    [Header("3D Sound Settings")]
    public float MinDistance = 1f;       //Distancia a partir de la cual el sonido empieza a atenuarse
    public float MaxDistance = 10000f;        //Distancia a partir de la cual el sonido no se atenúa más

    [Header("Sound Direction")]
    [Range(0f, 360f)] public float InsideConeAngle = 360f;
    [Range(0f, 360f)] public float OutsideConeAngle = 360f;
    [Range(0f, 1f)] public float OutsideVolume = 1f;

    [Header("Reverb Properties")]
    public float ReverbWet;

    /// <summary>
    /// Unifica el tratamiento de los diferentes formatos de sonido
    /// </summary>
    private FMOD.Sound sound;

    /// <summary>
    /// Controla parámetros del canal
    /// </summary>
    private FMOD.Channel channel;

    /// <summary>
    /// Estado actual de la reproducción
    /// </summary>
    private SoundState currentState;

    /// <summary>
    /// Lista con todos los DSPs aplicados al canal
    /// </summary>
    private List<FMOD.DSP> DSPList;

    /// <summary>
    /// Carga el sonido, crea el canal, inicializa su estado
    /// </summary>
    private void Start()
    {
        sound = LowLevelSystem.Instance.Create3DSound(Clip.name);
        channel = LowLevelSystem.Instance.CreateChannel(sound);
        currentState = SoundState.READY;
        DSPList = new List<FMOD.DSP>();

        if (PlayOnAwake)
            StartCoroutine(PlayOnAwakeDelay());
    }

    /// <summary>
    /// Delay para que le de tiempo a que le afecte el escenario
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayOnAwakeDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Play();
    }

    /// <summary>
    /// Para el canal asociado y libera el handle
    /// </summary>
    private void OnDestroy()
    {
        channel.stop();
        channel.clearHandle();
        sound.clearHandle();
    }

    #region Flow

    /// <summary>
    /// Reproduce el sonido
    /// </summary>
    public void Play()
    {
        CheckState();

        if (currentState == SoundState.PLAYING)
            SetMSPosition(0);

        else 
            LowLevelSystem.ERRCHECK(channel.setPaused(false));
    }

    /// <summary>
    /// Para el sonido si está reproduciendose o pausado
    /// Se libera el canal 
    /// </summary>
    public void Stop()
    {
        CheckState();

        if (currentState != SoundState.READY)
            LowLevelSystem.ERRCHECK(channel.stop());
    }

    /// <summary>
    /// Pausa el sonido si está reproduciendose
    /// </summary>
    public void Pause()
    {
        CheckState();

        if (currentState == SoundState.PLAYING)
            LowLevelSystem.ERRCHECK(channel.setPaused(true));
    }

    /// <summary>
    /// Despausa el sonido si está pausado
    /// </summary>
    public void Resume()
    {
        CheckState();

        if (currentState == SoundState.PAUSED)
            LowLevelSystem.ERRCHECK(channel.setPaused(false));
    }

    #endregion Flow

    #region Getters

    /// <summary>
    /// Devuelve si el sonido está pausado
    /// </summary>
    /// <returns></returns>
    public bool IsPaused()
    {
        bool isPaused;
        LowLevelSystem.ERRCHECK(channel.getPaused(out isPaused));
        return isPaused;
    }

    /// <summary>
    /// Devuelve si el sonido está reproduciendose
    /// </summary>
    /// <returns></returns>
    public bool IsPlaying()
    {
        bool isPlaying;
        LowLevelSystem.ERRCHECK(channel.isPlaying(out isPlaying));
        return isPlaying;
    }

    /// <summary>
    /// Devuelve si el sonido ha acabado
    /// </summary>
    /// <returns></returns>
    public bool HasEnded()
    {
        bool playing;
        return channel.isPlaying(out playing) == FMOD.RESULT.ERR_INVALID_HANDLE;
    }

    #endregion Getters

    #region UpdateSetters

    /// <summary>
    /// Silencia el sonido o le pone volumen previo a ser muteado
    /// </summary>
    /// <param name="muted"></param>
    private void UpdateMute(bool muted)
    {
        Mute = muted;
        LowLevelSystem.ERRCHECK(channel.setMute(Mute));
    }

    /// <summary>
    /// Establece el número de veces que tiene que reproducirse en loop
    /// -1 para infinito
    /// </summary>
    /// <param name="loop"></param>
    private void UpdateLoop(bool loop)
    {
        Loop = loop;

        if (loop)
            LowLevelSystem.ERRCHECK(channel.setLoopCount(-1));
        else
            LowLevelSystem.ERRCHECK(channel.setLoopCount(0));
    }

    /// <summary>
    /// Establece el volumen del sonido
    /// Valor entre 0.0 y 1.0
    /// </summary>
    /// <param name="volume"></param>
    private void UpdateVolume(float volume)
    {
        Volume = volume;
        LowLevelSystem.ERRCHECK(channel.setVolume(Volume));
    }

    /// <summary>
    /// Establece el pitch del sonido
    /// </summary>
    /// <param name="pitch"></param>
    private void UpdatePitch(float pitch)
    {
        Pitch = pitch;
        LowLevelSystem.ERRCHECK(channel.setPitch(Pitch));
    }

    /// <summary>
    /// Establece el angulo del cono interior: donde no hay atenuación por dirección
    /// Establece el ángulo del cono exterior: donde el sonido se atenúa
    /// Establece el volumen fuera del cono exterior
    /// </summary>
    /// <param name="insideConeAngle"></param>
    private void UpdateConeSettings(float insideConeAngle, float outsideConeAngle, float outsideVolume)
    {
        InsideConeAngle = insideConeAngle;
        OutsideConeAngle = outsideConeAngle;
        OutsideVolume = outsideVolume;

        LowLevelSystem.ERRCHECK(channel.set3DConeSettings(InsideConeAngle, OutsideConeAngle, OutsideVolume));
    }

    /// <summary>
    /// Establece la Distancia a partir de la cual el sonido empieza a atenuarse
    /// Establece la Distancia a partir de la cual el sonido no se atenúa más
    /// </summary>
    /// <param name="minDistance"></param>
    private void UpdateMinMaxDistance(float minDistance, float maxDistance)
    {
        MinDistance = minDistance;
        MaxDistance = maxDistance;
        LowLevelSystem.ERRCHECK(channel.set3DMinMaxDistance(MinDistance, MaxDistance));
    }

    /// <summary>
    /// Establece la Distancia a partir de la cual el sonido no se atenúa más
    /// Asumo que hay que pasar 0
    /// </summary>
    /// <param name="maxDistance"></param>
    private void UpdateReverbWet(float reverbWet)
    {
        ReverbWet = reverbWet;
        LowLevelSystem.ERRCHECK(channel.setReverbProperties(0, ReverbWet));
    }

    #endregion UpdateSetters

    #region Setters

    /// <summary>
    /// Añade un DSP al canal
    /// </summary>
    /// <param name="DSP"></param>
    public void AddDSP(FMOD.DSP DSP)
    {
        CheckState();
        DSPList.Add(DSP);
        LowLevelSystem.ERRCHECK(channel.addDSP(DSPList.IndexOf(DSP), DSP));
    }

    /// <summary>
    /// Establece la posición de reproducción de la pista en milisegundos
    /// </summary>
    /// <param name="position"></param>
    public void SetMSPosition(uint position)
    {
        CheckState();
        LowLevelSystem.ERRCHECK(channel.setPosition(position, FMOD.TIMEUNIT.MS));
    }

    ///// <summary>
    ///// TODO: Revisar
    ///// Asocia el canal a un grupo
    ///// </summary>
    ///// <param name="channelGroup"></param>
    //public void SetChannelGroup(FMOD.ChannelGroup channelGroup)
    //{
    //    LowLevelSystem.ERRCHECK(channel.setChannelGroup(channelGroup));
    //}

    #endregion Setters

    /// <summary>
    /// Controla el flujo de estados
    /// Además, si ha acabado un sonido, lo carga de nuevo
    /// Updatea los atributos
    /// </summary>
    private void FixedUpdate()
    {
        CheckState();

        //Update de los atributos
        UpdateMute(Mute);
        UpdateLoop(Loop);
        UpdateVolume(Volume);
        UpdatePitch(Pitch);
        UpdateConeSettings(InsideConeAngle,OutsideConeAngle,OutsideVolume);
        UpdateMinMaxDistance(MinDistance,MaxDistance);
        UpdateReverbWet(ReverbWet);

        UpdatePosition();
    }

    /// <summary>
    /// Actualiza el estado actual del sonido
    /// </summary>
    private void CheckState()
    {
        switch (currentState)
        {
            case SoundState.READY:
                if (!IsPaused())
                    currentState = SoundState.PLAYING;

                break;

            case SoundState.PLAYING:
                if (HasEnded())
                    ResetChannel();
                else if (IsPaused())
                    currentState = SoundState.PAUSED;

                break;
            case SoundState.PAUSED:
                if (HasEnded())
                    ResetChannel();
                else if (!IsPaused())
                    currentState = SoundState.PLAYING;

                break;
        }
    }

    /// <summary>
    /// Crea un nuevo canal con los parámetros del anterior
    /// </summary>
    private void ResetChannel()
    {
        channel.clearHandle();
        channel = LowLevelSystem.Instance.CreateChannel(sound);
        currentState = SoundState.READY;

        //Se vuelven a añadir todos los efectos al canal
        foreach (FMOD.DSP dsp in DSPList)
            LowLevelSystem.ERRCHECK(channel.addDSP(DSPList.IndexOf(dsp), dsp));

    }

    /// <summary>
    /// Actualiza la posición y la velocidad del sonido a la del objeto
    /// Actualiza la orientación del sonido
    /// </summary>
    private void UpdatePosition()
    {
        FMOD.VECTOR lastPos, lastVel, alt_pan_pos;
        LowLevelSystem.ERRCHECK(channel.get3DAttributes(out lastPos,out lastVel, out alt_pan_pos));

        FMOD.VECTOR pos = new FMOD.VECTOR();
        pos.x = transform.position.x;
        pos.y = transform.position.y;
        pos.z = transform.position.z;

        FMOD.VECTOR vel = new FMOD.VECTOR();
        vel.x = (pos.x - lastPos.x) * Time.deltaTime;
        vel.y = (pos.y - lastPos.y) * Time.deltaTime;
        vel.z = (pos.z - lastPos.z) * Time.deltaTime;

        LowLevelSystem.ERRCHECK(channel.set3DAttributes(ref pos, ref vel, ref alt_pan_pos));

        FMOD.VECTOR forward = new FMOD.VECTOR();
        forward.x = transform.forward.x;
        forward.y = transform.forward.y;
        forward.z = transform.forward.z;

        LowLevelSystem.ERRCHECK(channel.set3DConeOrientation(ref forward));
    }

}
