using UnityEngine;

public class TestSound : MonoBehaviour
{
    public Sound3D MySound;
    AudioSource AudioSource;

    /// <summary>
    /// Añade un efecto de sonido de radio: Distorsión + filtro paso alto
    /// </summary>
    private void AddEfect()
    {
        FMOD.DSP distortion;
        distortion = LowLevelSystem.Instance.CreateDSPByType(FMOD.DSP_TYPE.DISTORTION);
        LowLevelSystem.ERRCHECK(distortion.setParameterFloat((int)FMOD.DSP_DISTORTION.LEVEL, 0.85f));         // parametros del efecto

        FMOD.DSP highpass;
        highpass = LowLevelSystem.Instance.CreateDSPByType(FMOD.DSP_TYPE.HIGHPASS);
        LowLevelSystem.ERRCHECK(highpass.setParameterFloat((int)FMOD.DSP_HIGHPASS.CUTOFF,2000f));         // parametros del efecto

        // apliacion a un canal (puede aplicarse a un grupo o al sistema)
        MySound.AddDSP(distortion);
        MySound.AddDSP(highpass);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            MySound.Play();
        else if (Input.GetKeyDown(KeyCode.O))
            MySound.Pause();
        else if (Input.GetKeyDown(KeyCode.P))
            MySound.Resume();
        else if (Input.GetKeyDown(KeyCode.K))
            MySound.Stop();

        else if (Input.GetKeyDown(KeyCode.U))
            AddEfect();
    }
}
