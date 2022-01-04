using UnityEngine;

public class MapData : MonoBehaviour
{
    [Tooltip("¿ÞÂÊ ÇÏ´Ü ÁÂÇ¥")]
    [SerializeField] private Vector2 position;
    [Tooltip("¿À¸¥ÂÊ »ó´Ü ÁÂÇ¥")]
    [SerializeField] private Vector2 size;

    public Vector2 GetPosition()
    {
        return position;
    }

    public Vector2 GetSize()
    {
        return size;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector3(position.x, position.y, 0), new Vector3(size.x, position.y, 0));
        Gizmos.DrawLine(new Vector3(size.x, position.y, 0), new Vector3(size.x, size.y, 0));
        Gizmos.DrawLine(new Vector3(size.x, size.y, 0), new Vector3(position.x, size.y, 0));
        Gizmos.DrawLine(new Vector3(position.x, size.y, 0), new Vector3(position.x, position.y, 0));
    }
}
