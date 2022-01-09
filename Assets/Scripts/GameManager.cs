using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject canvas;

    private void Init()
    {
        Vector3 startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        Instantiate(player, startPoint, Quaternion.identity);
        Instantiate(canvas);
    }

    private void Awake()
    {
        Init();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
