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

    private void Update()
    {
        LoadScene();
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

    private void LoadScene()
    {
        if (Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.H))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SceneManager.LoadScene("Stage1-1");
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                SceneManager.LoadScene("Stage2-1");
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                SceneManager.LoadScene("Stage3-1");
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                SceneManager.LoadScene("Stage4-2");
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                SceneManager.LoadScene("Stage5-1");
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                SceneManager.LoadScene("Stage6-2");
            else if (Input.GetKeyDown(KeyCode.Alpha7))
                SceneManager.LoadScene("Ending3");
        }
    }
}
