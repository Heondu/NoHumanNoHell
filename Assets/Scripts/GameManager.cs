using UnityEngine;

/// <summary>
/// 게임 시작 시 필요한 오브젝트를 생성하는 클래스
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject canvas;

    private void Init()
    {
        //시작 위치를 찾아 플레이어 생성
        Vector3 startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        Instantiate(player, startPoint, Quaternion.identity);
        Instantiate(canvas);
    }

    private void Awake()
    {
        Init();
    }
}
