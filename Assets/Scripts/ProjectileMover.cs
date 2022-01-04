using UnityEngine;

/// <summary>
/// 발사체의 움직임을 제어하는 클래스
/// </summary>
public class ProjectileMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    public void MoveTo(Vector3 direction)
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
