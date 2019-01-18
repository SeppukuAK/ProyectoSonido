using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Componente encargado de controlar el comportamiento del enemigo
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] private float initialMoveRate;
    [SerializeField] private float maxMoveRate;
    [SerializeField] private float trapVelIncrease;
    [SerializeField] private float trapPitchIncrease;

    [SerializeField] private Sound3D enemySound;

    private Transform _playerTransform;
    /// <summary>
    /// Velocidad a la que se mueve el enemigo
    /// </summary>
    public float MoveRate { get; set; }

    public void Init(Transform playerTransform)
    {
        _playerTransform = playerTransform;
        MoveRate = initialMoveRate;
        StartCoroutine(MoveRoutine());

        StartCoroutine(AddEfect());
    }

    /// <summary>
    /// Añade un efecto de sonido de radio: Distorsión + filtro paso alto
    /// </summary>
    private IEnumerator AddEfect()
    {
        yield return new WaitForSeconds(0.1f);
        FMOD.DSP distortion;
        distortion = LowLevelSystem.Instance.CreateDSPByType(FMOD.DSP_TYPE.DISTORTION);
        LowLevelSystem.ERRCHECK(distortion.setParameterFloat((int)FMOD.DSP_DISTORTION.LEVEL, 0.85f));         // parametros del efecto

        FMOD.DSP highpass;
        highpass = LowLevelSystem.Instance.CreateDSPByType(FMOD.DSP_TYPE.HIGHPASS);
        LowLevelSystem.ERRCHECK(highpass.setParameterFloat((int)FMOD.DSP_HIGHPASS.CUTOFF, 2000f));         // parametros del efecto

        // apliacion a un canal (puede aplicarse a un grupo o al sistema)
        enemySound.AddDSP(distortion);
        enemySound.AddDSP(highpass);
    }

    private IEnumerator MoveRoutine()
    {
        // Bit shift the index of the layer (10) to get a bit mask
        int layerMask = 1 << 10;

        while (true)
        {
            transform.Rotate(new Vector3(0f, -90f, 0f));

            RaycastHit hit;

            while (Physics.Raycast(transform.position, transform.forward, out hit, 3.0f, layerMask))
                transform.Rotate(new Vector3(0f, 90f, 0f));

            StartCoroutine(MoveToCoroutine(transform.position + transform.forward * LevelManager.Instance.ModuleSize, MoveRate));

            float totalTicks = MoveRate / Time.fixedDeltaTime;
            for (int i = 0; i < totalTicks; i++)
                yield return new WaitForFixedUpdate();

        }
    }

    /// <summary>
    /// Corrutina que mueve al enemigo hasta un punto en el tiempo establecido
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="vel"></param>
    /// <param name="endMove"></param>
    /// <returns></returns>
    private IEnumerator MoveToCoroutine(Vector3 pos, float time)
    {
        //Calculo de ticks y de direccion
        float totalTicks = time / Time.fixedDeltaTime;
        float distancePerTick = (pos - transform.position).magnitude / totalTicks;
        Vector3 dir = (pos - transform.position).normalized;

        //Movimiento hasta la posición
        for (int i = 0; i < totalTicks; i++)
        {
            transform.position += dir * distancePerTick;
            CheckPlayerCollision();
            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }

    public void PlayerOnTrap()
    {
        if (MoveRate - trapVelIncrease > maxMoveRate)
        {
            MoveRate -= trapVelIncrease;
            if (enemySound.Pitch + trapPitchIncrease < 6)
                enemySound.Pitch += trapPitchIncrease;
        }
    }

    private void CheckPlayerCollision()
    {
        Vector3 playerPos = _playerTransform.position;
        Vector3 enemyPos = transform.position;

        Vector3 aux = playerPos - enemyPos;


        if (Mathf.Abs(aux.x) < LevelManager.Instance.ModuleSize/2 && Mathf.Abs(aux.z) < LevelManager.Instance.ModuleSize/2)
            SceneManager.LoadScene("DeathScene");
    }
}
