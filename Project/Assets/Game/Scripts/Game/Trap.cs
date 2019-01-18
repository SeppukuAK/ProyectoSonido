using UnityEngine;

/// <summary>
/// Componente asociado a una trampa
/// </summary>
public class Trap : MonoBehaviour {

    private Enemy _enemy;

    /// <summary>
    /// Le pasa la referencia al enemigo
    /// </summary>
    /// <param name="enemy"></param>
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
