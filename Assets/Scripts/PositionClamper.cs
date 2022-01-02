using UnityEngine;

public class PositionClamper : MonoBehaviour
{
    private MapData mapData;
    private Bounds bounds;
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    private void Start()
    {
        mapData = FindObjectOfType<MapData>();
        bounds = GetComponent<CapsuleCollider2D>().bounds;
        float width = bounds.max.x - bounds.min.x;
        float height = bounds.max.y - bounds.min.y;
        width += (mapData.GetPosition().x + mapData.GetSize().x) / 2;
        height += (mapData.GetPosition().y + mapData.GetSize().y) / 2;

        xMin = width - mapData.GetSize().x / 2;
        xMax = mapData.GetSize().x / 2 - width;
        yMin = height - mapData.GetSize().y / 2;
        yMax = mapData.GetSize().y / 2 - height;
    }

    private void Update()
    {
        Clamp();
    }

    private void Clamp()
    {
        float x = Mathf.Clamp(transform.position.x, xMin, xMax);
        float y = Mathf.Clamp(transform.position.y, yMin, yMax);
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
