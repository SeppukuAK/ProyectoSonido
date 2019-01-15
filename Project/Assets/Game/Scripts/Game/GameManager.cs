using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int totalObjectives;

    [SerializeField] private Text progressText;

    public int Progress
    {
        get { return progress; }
        set
        {
            progress = value;
            if (progress == totalObjectives)
                SceneManager.LoadScene("WinScene");

            progressText.text = "Progress: " + progress + "/" + totalObjectives;
        }
    }
    private int progress;

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

    private void Start()
    {
        Progress = 0;
    }

    /// <summary>
    /// Sale de la aplicación
    /// </summary>
    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
