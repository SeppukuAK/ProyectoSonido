using UnityEngine;

public class Trap : MonoBehaviour {

    private Enemy _enemy;

    public void Init(Enemy enemy)
    {
        _enemy = enemy;
    }

    /// <summary>
    /// Aumenta la velocidad del enemigo
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
            _enemy.PlayerOnTrap();
    }
}
