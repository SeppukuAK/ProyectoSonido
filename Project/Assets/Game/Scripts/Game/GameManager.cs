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
        Instance = this;
    }

    private void Start()
    {
        Progress = 0;
    }
}
