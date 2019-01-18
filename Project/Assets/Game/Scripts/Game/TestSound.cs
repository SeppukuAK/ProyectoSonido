using UnityEngine;

public class TestSound : MonoBehaviour
{
    public Sound3D MySound;
    AudioSource AudioSource;

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
    }
}
