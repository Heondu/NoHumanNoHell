using System.Collections;
using UnityEngine;

/// <summary>
/// ī�޶��� �������� �����ϰ� ī�޶� ����Ʈ�� �����ϴ� Ŭ����
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

        //ī�޶� �̵� ���� ���� ����
        xMin = mapData.GetPosition().x + width;
        xMax = mapData.GetSize().x - width;
        yMin = mapData.GetPosition().y + height;
        yMax = mapData.GetSize().y - height;
    }

    private void LateUpdate()
    {
        Follow();
        Clamp();
        //Clamp�� �����ϰ� ���� ���� ���� ��鸲 ���� ���� ��
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
    /// ī�޶� ����
    /// </summary>
    /// <param name="amount">����</param>
    /// <param name="duration">���� �ð�</param>
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

    // Ŭ������ ���� �ʴ� �Լ�. �Լ� �̵� �ʿ�
    /// <summary>
    /// ���ο� ���
    /// </summary>
    /// <param name="timeScale"></param>
    /// <param name="duration">���� �ð�</param>
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
