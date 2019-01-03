using UnityEngine;

public class TestSound : MonoBehaviour {

    public SoundGroup3D SoundGroup3D;
    public Sound3D MySound;

	void Update () {

        if (Input.GetKeyDown(KeyCode.Q))
            MySound.Play();
        else if (Input.GetKeyDown(KeyCode.W))
            MySound.Pause();
        else if (Input.GetKeyDown(KeyCode.E))
            MySound.Resume();
        else if (Input.GetKeyDown(KeyCode.R))
            MySound.Stop();

        else if (Input.GetKeyDown(KeyCode.A))
            SoundGroup3D.Resume();

        else if (Input.GetKeyDown(KeyCode.S))
            SoundGroup3D.Pause();


    }
}
