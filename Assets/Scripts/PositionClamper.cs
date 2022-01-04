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

        xMin = mapData.GetPosition().x + width;
        xMax = mapData.GetSize().x - width;
        yMin = mapData.GetPosition().y + height;
        yMax = mapData.GetSize().y - height;
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
