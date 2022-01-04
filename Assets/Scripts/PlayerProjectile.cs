using UnityEngine;

/// <summary>
/// 플레이어 원거리 공격시 발사되는 발사체를 제어하는 클래스
/// </summary>
public class PlayerProjectile : MonoBehaviour
{
    private ProjectileMover projectileMover;
    [SerializeField] private Vector2 direction;
    private float maxDistance;
    private int damage;
    private GameObject instigator;
    private bool isReturn = false;
    private Vector3 startPos;
    [SerializeField] private LayerMask groundMask;

    //플레이어로부터 방향, 최대 거리, 데미지, 공격자 정보 등을 받아옴
    public void Setup(Vector2 direction, float maxDistance, int damage, GameObject instigator)
    {
        projectileMover = GetComponent<ProjectileMover>();
        this.direction = direction;
        this.maxDistance = maxDistance;
        this.damage = damage;
        this.instigator = instigator;
        startPos = instigator.transform.position;
    }

    private void Update()
    {
        //공격자가 죽을 경우 발사체 삭제
        if (instigator != null)
        {
            projectileMover.MoveTo(direction);

            if (!isReturn)
            {
                //최대 거리에 도달하거나 벽에 부딪히면 bool 변수를 true로 설정하고 데미지 처리를 무시하기 위해 콜라이더를 비활성화 함
                float distance = Mathf.Abs(Vector2.SqrMagnitude(startPos - transform.position));
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.1f, groundMask);
                if (distance >= maxDistance || hit)
                {
                    isReturn = true;
                    GetComponent<CircleCollider2D>().enabled = false;
                }
            }
            else
            {
                //최대 거리 도달 시 플레이어한테 돌아오기 위해 플레이어 쪽으로 방향 설정
                direction = (instigator.transform.position - transform.position).normalized;

                float distance = Mathf.Abs(Vector2.SqrMagnitude(instigator.transform.position - transform.position));
                if (distance <= 0.5f)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(instigator.tag)) return;

        ILivingEntity entity = collision.GetComponent<ILivingEntity>();
        if (entity != null)
        {
            entity.TakeDamage(damage, instigator);
        }
    }
}
