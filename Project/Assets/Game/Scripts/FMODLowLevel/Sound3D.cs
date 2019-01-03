using UnityEngine;

/// <summary>
/// Controla parámetros del canal de reproducción
/// Tiene una posición en el espacio
/// </summary>
public class Sound3D : MonoBehaviour
{
    private enum SoundState { READY, PLAYING, PAUSED }

    /// <summary>
    /// Pista de audio que reproduce
    /// </summary>
    public AudioClip Clip;
    public bool PlayOnAwake;
    public bool Loop;

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
    /// Carga el sonido, crea el canal, inicializa su estado
    /// </summary>
    private void Start()
    {
        sound = LowLevelSystem.Instance.Create3DSound(Clip.name);
        channel = LowLevelSystem.Instance.CreateChannel(sound);       
        currentState = SoundState.READY;

        if (PlayOnAwake)
            Play();

        SetLoop(Loop);
    }

    #region Flow
    /// <summary>
    /// Reproduce el sonido
    /// </summary>
    public void Play()
    {
        if (currentState == SoundState.PLAYING)
            SetMSPosition(0);

        else if (currentState == SoundState.PAUSED || currentState == SoundState.READY)
            LowLevelSystem.ERRCHECK(channel.setPaused(false));
    }

    /// <summary>
    /// Para el sonido si está reproduciendose o pausado
    /// Se libera el canal 
    /// </summary>
    public void Stop()
    {
        if (currentState == SoundState.PLAYING || currentState == SoundState.PAUSED)
            LowLevelSystem.ERRCHECK(channel.stop());
    }

    /// <summary>
    /// Pausa el sonido si está reproduciendose
    /// </summary>
    public void Pause()
    {
        if (currentState == SoundState.PLAYING)
            LowLevelSystem.ERRCHECK(channel.setPaused(true));
    }

    /// <summary>
    /// Despausa el sonido si está pausado
    /// </summary>
    public void Resume()
    {
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

    /// <summary>
    /// Devuelve el volumen del sonido.
    /// Un valor entre 0.0f y 1.0f
    /// </summary>
    /// <returns></returns>
    public float GetVolume()
    {
        float volume;
        LowLevelSystem.ERRCHECK(channel.getVolume(out volume));
        return volume;
    }

    #endregion Getters

    #region Setters

    /// <summary>
    /// Establece el volumen del sonido
    /// Valor entre 0.0 y 1.0
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolume(float volume)
    {
        LowLevelSystem.ERRCHECK(channel.setVolume(volume));
    }

    /// <summary>
    /// Establece la posición de reproducción de la pista en milisegundos
    /// </summary>
    /// <param name="position"></param>
    public void SetMSPosition(uint position)
    {
        LowLevelSystem.ERRCHECK(channel.setPosition(position, FMOD.TIMEUNIT.MS));
    }

    /// <summary>
    /// Establece el número de veces que tiene que reproducirse en loop
    /// -1 para infinito
    /// </summary>
    /// <param name="loop"></param>
    public void SetLoop(bool loop)
    {
        Loop = loop;

        if (loop)
            LowLevelSystem.ERRCHECK(channel.setLoopCount(-1));
        else
            LowLevelSystem.ERRCHECK(channel.setLoopCount(0));
    }

    /// <summary>
    /// Silencia el sonido o le pone volumen previo a ser muteado
    /// </summary>
    /// <param name="muted"></param>
    public void SetMuted(bool muted)
    {
        LowLevelSystem.ERRCHECK(channel.setMute(muted));
    }

    /// <summary>
    /// Establece el pitch del sonido
    /// </summary>
    /// <param name="pitch"></param>
    public void SetPitch(float pitch)
    {
        LowLevelSystem.ERRCHECK(channel.setPitch(pitch));
    }

    /// <summary>
    /// Establece la frecuencia del sonido
    /// </summary>
    /// <param name="frequency"></param>
    public void SetFrequency(float frequency)
    {
        LowLevelSystem.ERRCHECK(channel.setFrequency(frequency));
    }

    /// <summary>
    /// TODO: Revisar
    /// Asocia el canal a un grupo
    /// </summary>
    /// <param name="channelGroup"></param>
    public void SetChannelGroup(FMOD.ChannelGroup channelGroup)
    {
        LowLevelSystem.ERRCHECK(channel.setChannelGroup(channelGroup));
    }

    #endregion setters

    /// <summary>
    /// Controla el flujo de estados
    /// Además, si ha acabado un sonido, lo carga de nuevo
    /// </summary>
    private void Update()
    {
        switch (currentState)
        {
            case SoundState.READY:
                if (!IsPaused())
                    currentState = SoundState.PLAYING;

                break;

            case SoundState.PLAYING:
                if (HasEnded())
                {
                    channel = LowLevelSystem.Instance.CreateChannel(sound);
                    SetLoop(Loop);
                    currentState = SoundState.READY;
                }
                else if (IsPaused())
                    currentState = SoundState.PAUSED;

                break;
            case SoundState.PAUSED:
                if (HasEnded())
                {
                    channel = LowLevelSystem.Instance.CreateChannel(sound);
                    SetLoop(Loop);
                    currentState = SoundState.READY;
                }
                else if (!IsPaused())
                    currentState = SoundState.PLAYING;

                break;

        }

        UpdatePosition();
    }

    /// <summary>
    /// Actualiza la posición del sonido a la del objeto
    /// </summary>
    private void UpdatePosition()
    {
        FMOD.VECTOR pos = new FMOD.VECTOR(); // posicion del listener
        pos.x = transform.position.x;
        pos.y = transform.position.y;
        pos.z = transform.position.z;

        FMOD.VECTOR vel = new FMOD.VECTOR(); // velocidad del listener
        vel.x = 0;
        vel.y = 0;
        vel.z = 0;

        FMOD.VECTOR alt_pan_pos = new FMOD.VECTOR(); // vector up: hacia la ``coronilla''
        alt_pan_pos.x = transform.forward.x;
        alt_pan_pos.y = transform.forward.y;
        alt_pan_pos.z = transform.forward.z;

        LowLevelSystem.ERRCHECK(channel.set3DAttributes(ref pos, ref vel, ref alt_pan_pos));
    }
}
