using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 씬 전환 시 플레이어 이동과 화면 효과를 제어하는 클래스
/// </summary>
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
            //플레이어가 이동 제한 범위 밖으로 나갈 수 있고 중력과 적의 영향을 받지 않도록 컴포넌트 비활성화
            player.GetComponent<PositionClamper>().enabled = false;
            player.GetComponent<Rigidbody2D>().isKinematic = true;
            player.GetComponent<CapsuleCollider2D>().enabled = false;

            //씬 전환 시 페이드 인
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
        //플레이어가 설정해둔 영역 안으로 들어오면 미리 지정해둔 방향으로 이동시킴
        if (isPlayerEnter)
        {
            player.GetComponent<Movement>().MoveTo(direction * 0.1f);
        }
    }
}
