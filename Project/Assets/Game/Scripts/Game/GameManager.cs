using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    /// <summary>
    /// Persistent Singleton
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Sale de la aplicación si le das a escape
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ExitApplication();
    }

    /// <summary>
    /// Resetea el juego
    /// </summary>
    public void ResetGame()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Sale de la aplicación
    /// </summary>
    public void ExitApplication()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }
}
