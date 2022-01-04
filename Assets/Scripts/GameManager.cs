using UnityEngine;

/// <summary>
/// ���� ���� �� �ʿ��� ������Ʈ�� �����ϴ� Ŭ����
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject canvas;

    private void Init()
    {
        //���� ��ġ�� ã�� �÷��̾� ����
        Vector3 startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        Instantiate(player, startPoint, Quaternion.identity);
        Instantiate(canvas);
    }

    private void Awake()
    {
        Init();
    }
}
