using System.Collections;
using UnityEngine;

public class SceneLoadingEffect : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private float delay;
    private bool isPlayerEnter = false;
    private GameObject player;
    private SceneLoader sceneLoader;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        sceneLoader = GetComponent<SceneLoader>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerEnter)
            return;
        if (IsEnemiesLeft())
            return;

        if (collision.CompareTag("Player"))
        {
            isPlayerEnter = true;
            player = collision.gameObject;
            player.GetComponent<PositionClamper>().enabled = false;
            player.GetComponent<Entity>().CanBeDamaged = false;
            Camera.main.GetComponent<CameraController>().StopFollow();

            FindObjectOfType<SceneFadeInOut>().FadeIn(delay);

            StartCoroutine("ChangeScene");
        }
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(delay);

        sceneLoader.LoadScene();
    }

    private void Update()
    {
        if (isPlayerEnter)
        {
            player.GetComponent<Movement>().Move(direction * 0.1f);
        }
    }

    private bool IsEnemiesLeft()
    {
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        if (enemies.Length == 0)
            return false;
        return true;
    }
}
