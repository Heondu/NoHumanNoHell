using UnityEngine;

/// <summary>
/// �߻�ü�� �������� �����ϴ� Ŭ����
/// </summary>
public class ProjectileMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    public void MoveTo(Vector3 direction)
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
