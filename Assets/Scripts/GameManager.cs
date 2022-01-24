using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject canvas;
    [SerializeField] private string restartScene;
    [HideInInspector] public bool IsStop = false;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Setup();
    }

    private void Setup()
    {
        Vector3 startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        Instantiate(player, startPoint, Quaternion.identity);
        Instantiate(canvas);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(restartScene);
    }

    public void Stop(bool value)
    {
        IsStop = value;
        if (IsStop)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }
}
