using System.Collections;
using UnityEngine;

/// <summary>
/// 카메라의 움직임을 제어하고 카메라 이펙트를 관리하는 클래스
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private MapData mapData;
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;
    private Vector3 shakeOffset;
    private Coroutine shakeCoroutine;
    private Coroutine slowCoroutine;

    private void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
        mapData = FindObjectOfType<MapData>();

        float height = Camera.main.orthographicSize;
        float width = height * Screen.width / Screen.height;

        //카메라 이동 제한 범위 설정
        xMin = mapData.GetPosition().x + width;
        xMax = mapData.GetSize().x - width;
        yMin = mapData.GetPosition().y + height;
        yMax = mapData.GetSize().y - height;
    }

    private void LateUpdate()
    {
        Follow();
        Clamp();
        //Clamp와 무관하게 흔들기 위해 따로 흔들림 값을 더해 줌
        transform.position += shakeOffset;
    }

    private void Follow()
    {
        if (target == null) return;

        Vector3 targetPos = target.position;
        targetPos.z = transform.position.z;
        transform.position = targetPos + offset;
    }

    private void Clamp()
    {
        float x = Mathf.Clamp(transform.position.x, xMin, xMax);
        float y = Mathf.Clamp(transform.position.y, yMin, yMax);
        transform.position = new Vector3(x, y, transform.position.z);
    }

    /// <summary>
    /// 카메라 흔들기
    /// </summary>
    /// <param name="amount">세기</param>
    /// <param name="duration">지속 시간</param>
    public void Shake(float amount, float duration)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeCo(amount, duration));
    }

    private IEnumerator ShakeCo(float amount, float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            shakeOffset = Random.insideUnitCircle * amount;

            timer += Time.deltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;
    }

    // 클래스에 맞지 않는 함수. 함수 이동 필요
    /// <summary>
    /// 슬로우 모션
    /// </summary>
    /// <param name="timeScale"></param>
    /// <param name="duration">지속 시간</param>
    public void Slow(float timeScale, float duration)
    {
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }
        slowCoroutine = StartCoroutine(SlowCo(timeScale, duration));
    }

    private IEnumerator SlowCo(float timeScale, float duration)
    {
        Time.timeScale = timeScale;

        float current = 0;
        float percent = 0;
        while (current < duration)
        {
            current += Time.deltaTime;
            percent = current / duration;
            Time.timeScale = Mathf.Lerp(timeScale, 1, percent);
            yield return null;
        }

        Time.timeScale = 1;
    }
}
