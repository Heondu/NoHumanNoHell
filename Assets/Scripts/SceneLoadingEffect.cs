using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingEffect : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private float delay;
    private bool isPlayerEnter = false;
    private GameObject player;
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GetComponent<SceneLoader>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerEnter) return;

        if (collision.CompareTag("Player"))
        {
            isPlayerEnter = true;
            player = collision.gameObject;
            player.GetComponent<PositionClamper>().enabled = false;
            player.GetComponent<Rigidbody2D>().isKinematic = true;
            player.GetComponent<CapsuleCollider2D>().enabled = false;

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
            player.GetComponent<Movement>().MoveTo(direction * 0.1f);
        }
    }
}
