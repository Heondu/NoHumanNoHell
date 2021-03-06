using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

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
    private PixelPerfectCamera pixelPerfectCamera;

    private bool follow = true;

    private void Start()
    {
        ComponentSetup();
        ClampSetup();
    }

    private void ComponentSetup()
    {
        target = FindObjectOfType<PlayerController>().transform;
        mapData = FindObjectOfType<MapData>();
        pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
    }

    private void ClampSetup()
    {
        float height = (float)pixelPerfectCamera.refResolutionY / pixelPerfectCamera.assetsPPU / 2;
        float width = (float)pixelPerfectCamera.refResolutionX / pixelPerfectCamera.assetsPPU / 2;

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
        if (!follow) return;

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

    public void StopFollow()
    {
        follow = false;
    }

    public void StartFollow()
    {
        follow = true;
    }
}
