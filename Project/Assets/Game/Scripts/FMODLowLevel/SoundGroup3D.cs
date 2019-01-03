using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGroup3D : MonoBehaviour
{
    public string GroupName;
    public Sound3D[] Sounds;

    private FMOD.ChannelGroup channelGroup;

    /// <summary>
    /// Carga el sonido
    /// </summary>
    private void Start()
    {
        channelGroup = LowLevelSystem.Instance.CreateChannelGroup(GroupName);

        foreach (Sound3D sound in Sounds)
            sound.SetChannelGroup(channelGroup);
    }

    /// <summary>
    /// Para el sonido si está reproduciendose o pausado
    /// </summary>
    public void Stop()
    {
        LowLevelSystem.ERRCHECK(channelGroup.stop());
    }

    /// <summary>
    /// Pausa el sonido si está reproduciendose
    /// </summary>
    public void Pause()
    {
        LowLevelSystem.ERRCHECK(channelGroup.setPaused(true));
    }

    /// <summary>
    /// Despausa el sonido si está pausado
    /// </summary>
    public void Resume()
    {
        LowLevelSystem.ERRCHECK(channelGroup.setPaused(false));
    }

    #region Getters

    /// <summary>
    /// Devuelve si el sonido está pausado
    /// </summary>
    /// <returns></returns>
    public bool IsPaused()
    {
        bool isPaused;
        LowLevelSystem.ERRCHECK(channelGroup.getPaused(out isPaused));
        return isPaused;
    }

    /// <summary>
    /// Devuelve si el sonido está reproduciendose
    /// </summary>
    /// <returns></returns>
    public bool IsPlaying()
    {
        bool isPlaying;
        LowLevelSystem.ERRCHECK(channelGroup.isPlaying(out isPlaying));
        return isPlaying;
    }

    /// <summary>
    /// Devuelve si el sonido ha acabado
    /// </summary>
    /// <returns></returns>
    public bool HasEnded()
    {
        bool playing;
        return channelGroup.isPlaying(out playing) == FMOD.RESULT.ERR_INVALID_HANDLE;
    }

    /// <summary>
    /// Devuelve el volumen del sonido.
    /// Un valor entre 0.0f y 1.0f
    /// </summary>
    /// <returns></returns>
    public float GetVolume()
    {
        float volume;
        LowLevelSystem.ERRCHECK(channelGroup.getVolume(out volume));
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
        LowLevelSystem.ERRCHECK(channelGroup.setVolume(volume));
    }

    /// <summary>
    /// Silencia el sonido o le pone volumen previo a ser muteado
    /// </summary>
    /// <param name="muted"></param>
    public void SetMuted(bool muted)
    {
        LowLevelSystem.ERRCHECK(channelGroup.setMute(muted));
    }

    /// <summary>
    /// Establece el pitch del sonido
    /// </summary>
    /// <param name="pitch"></param>
    public void SetPitch(float pitch)
    {
        LowLevelSystem.ERRCHECK(channelGroup.setPitch(pitch));
    }

    #endregion setters

}
