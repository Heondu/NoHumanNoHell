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

    private void Start()
    {
        mapData = FindObjectOfType<MapData>();

        float height = Camera.main.orthographicSize;
        float width = height * Screen.width / Screen.height;
        width += (mapData.GetPosition().x + mapData.GetSize().x) / 2;
        height += (mapData.GetPosition().y + mapData.GetSize().y) / 2;

        xMin = width - mapData.GetSize().x / 2;
        xMax = mapData.GetSize().x / 2 - width;
        yMin = height - mapData.GetSize().y / 2;
        yMax = mapData.GetSize().y / 2 - height;
    }

    private void LateUpdate()
    {
        Follow();
        Clamp();
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
}
