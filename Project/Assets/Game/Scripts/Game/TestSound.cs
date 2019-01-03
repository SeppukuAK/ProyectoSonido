using UnityEngine;

public class TestSound : MonoBehaviour {
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
    }
}
