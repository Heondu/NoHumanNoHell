using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private MapData mapData;
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    private Coroutine shakeCoroutine;
    private Vector3 shakeOffset;

    private void Start()
    {
        ComponentSetup();
        ClampSetup();
    }

    private void ComponentSetup()
    {
        target = FindObjectOfType<PlayerController>().transform;
        mapData = FindObjectOfType<MapData>();
    }

    private void ClampSetup()
    {
        float height = Camera.main.orthographicSize;
        float width = height * Screen.width / Screen.height;

        xMin = mapData.GetPosition().x + width;
        xMax = mapData.GetSize().x - width;
        yMin = mapData.GetPosition().y + height;
        yMax = mapData.GetSize().y - height;
    }

    private void LateUpdate()
    {
        Follow();
        Clamp();
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

    public void Shake(float amount, float duration)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(ShakeCo(amount, duration));
    }

    private IEnumerator ShakeCo(float amount, float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            shakeOffset = (Vector3)Random.insideUnitCircle * amount;
            yield return null;
        }
        shakeOffset = Vector3.zero;
    }
}
