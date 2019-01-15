using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Game attributes")]
    public int ModuleSize;
    [SerializeField] private int numObjectives;
    [SerializeField] private int numTraps;
    [SerializeField] private Transform maze;

    [Header ("References")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private Text progressText;
    [SerializeField] private Objective objectivePrefab;
    [SerializeField] private Trap trapPrefab;

    private int mazeRows, mazeCols;

    /// <summary>
    /// Número de progreso del jugador
    /// Actualiza el texto
    /// </summary>
    public int Progress
    {
        get { return progress; }
        set
        {
            progress = value;
            if (progress == numObjectives)
                SceneManager.LoadScene("WinScene");

            progressText.text = "Progress: " + progress + "/" + numObjectives;
        }
    }
    private int progress;

    /// <summary>
    /// Singleton
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Pone los objetivos y trampas aleatorias
    /// </summary>
    private void Start()
    {
        Progress = 0;

        mazeRows = maze.childCount;
        mazeCols = maze.GetChild(0).childCount;

        for(int i = 0; i < numObjectives; i++)
        {
            int row = Random.Range(0, mazeRows);
            int col = Random.Range(0, mazeCols);

            Instantiate(objectivePrefab, new Vector3(col* ModuleSize, 1f , -row * ModuleSize),Quaternion.identity);
        }

        for (int i = 0; i < numTraps; i++)
        {
            int row = Random.Range(0, mazeRows);
            int col = Random.Range(0, mazeCols);

            Instantiate(trapPrefab, new Vector3(col * ModuleSize, 0.01f, -row * ModuleSize), trapPrefab.transform.rotation).Init(enemy);
        }
    }
}
