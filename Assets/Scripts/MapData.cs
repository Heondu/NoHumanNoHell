using UnityEngine;

/// <summary>
/// 맵의 이동 제한 범위를 설정하는 클래스
/// </summary>
public class MapData : MonoBehaviour
{
    [Tooltip("왼쪽 하단 좌표")]
    [SerializeField] private Vector2 position;
    [Tooltip("오른쪽 상단 좌표")]
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
