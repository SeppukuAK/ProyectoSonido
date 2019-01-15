using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float initialMoveRate;
    [SerializeField] private float trapIncrease;
    [SerializeField] private float maxMoveRate;

    /// <summary>
    /// Velocidad a la que se mueve el enemigo
    /// </summary>
    public float MoveRate { get; set; }

    private void Start()
    {
        MoveRate = initialMoveRate;
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        // Bit shift the index of the layer (10) to get a bit mask
        int layerMask = 1 << 10;

        while (true)
        {
            transform.Rotate(new Vector3(0f, -90f, 0f));

            RaycastHit hit;

            while (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2.0f, layerMask))
                transform.Rotate(new Vector3(0f, 90f, 0f));

            transform.position += transform.forward * LevelManager.Instance.ModuleSize;
            yield return new WaitForSeconds(MoveRate);
        }
    }

    public void PlayerOnTrap()
    {
        if (MoveRate - trapIncrease > maxMoveRate)
            MoveRate -= trapIncrease;
    }

    private void OnCollisionEnter(Collision collision)
    {
        SceneManager.LoadScene("DeathScene");
    }
}
