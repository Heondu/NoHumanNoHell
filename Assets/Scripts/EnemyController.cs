using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적의 행동을 전체적으로 관리하는 클래스
/// </summary>
public class EnemyController : MonoBehaviour, ILivingEntity
{
    private SpriteRenderer spriteRenderer;
    private Movement movement;
    private EnemyAttack enemyAttack;
    private Status status;
    private Transform target;

    [SerializeField] private Vector2 detectSize;
    [SerializeField] private LayerMask detectLayer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<Movement>();
        enemyAttack = GetComponent<EnemyAttack>();
        status = GetComponent<Status>();
    }

    private void Update()
    {
        FSM();
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        StopCoroutine("Flash");
        StartCoroutine("Flash");

        status.TakeDamage(damage);
        Vector3 direction = (transform.position - instigator.transform.position).normalized;
        movement.Knockback(direction);

        if (status.GetCurrentHP() == 0)
        {
            Destroy(gameObject);
        }
    }

    //플레이어와 공유할 수 있는 클래스로 기능 분리 필요
    private IEnumerator Flash()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.black;
    }

    //임시 함수, 행동 별로 별도의 클래스 제작 필요
    private void FSM()
    {
        Detect();
        if (target == null) return;

        float distance = Vector3.SqrMagnitude(target.position - transform.position);

        if (distance <= enemyAttack.GetAttackDistance())
        {
            movement.MoveTo(Vector3.zero);
            Attack();
        }
        else if (distance <= detectSize.x)
        {
            Move();
        }
        else
        {
            movement.MoveTo(Vector3.zero);
        }
    }

    /// <summary>
    /// 플레이어를 감지하는 함수
    /// </summary>
    private void Detect()
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position, detectSize, 0, detectLayer);
        if (collider != null) target = collider.transform;
        else target = null;
    }

    private void Move()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        movement.MoveTo(direction);
    }

    private void Attack()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        enemyAttack.Attack(direction);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, detectSize);
    }
}
