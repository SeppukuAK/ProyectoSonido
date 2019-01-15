using UnityEngine;

public class Objective : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            LevelManager.Instance.Progress++;
            Destroy(gameObject);
        }
    }
}
