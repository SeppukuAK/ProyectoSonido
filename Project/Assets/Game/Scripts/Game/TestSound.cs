using UnityEngine;

public class TestSound : MonoBehaviour
{
    public Sound3D MySound;
    AudioSource AudioSource;


    private void AddEfect()
    {
        FMOD.DSP dsp = LowLevelSystem.Instance.CreateDSP(FMOD.DSP_TYPE.ECHO);

        // apliacion a un canal (puede aplicarse a un grupo o al sistema)
        MySound.AddDSP(dsp);

        // parametros del efecto
        //  dsp.setParameterFloat((int)FMOD.DSP_ECHO.DELAY, 0.02f);
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
            MySound.Play();
        else if (Input.GetKeyDown(KeyCode.W))
            MySound.Pause();
        else if (Input.GetKeyDown(KeyCode.E))
            MySound.Resume();
        else if (Input.GetKeyDown(KeyCode.R))
            MySound.Stop();
        else if (Input.GetKeyDown(KeyCode.D))
            AddEfect();

    }
}
