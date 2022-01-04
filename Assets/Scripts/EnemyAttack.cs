using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 공격을 제어하는 클래스
/// </summary>
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float distance;
    [SerializeField] private float cooldown;
    [SerializeField] private float delay;

    [SerializeField] private LayerMask groundLayer;

    private Movement movement;
    private EnemyAnimation enemyAnimation;

    private bool isAttacking = false;
    private Vector3 attackDirection;

    private void Start()
    {
        movement = GetComponent<Movement>();
        enemyAnimation = GetComponent<EnemyAnimation>();
    }

    public void Attack(Vector3 direction)
    {
        if (isAttacking) return;

        isAttacking = true;
        attackDirection = direction;

        //코루틴을 외부에서 쉽게 호출할 수 있도록 껍데기 함수와 메인 함수로 나누었다.
        StartCoroutine("AttackCo", direction);
    }

    private IEnumerator AttackCo(Vector3 direction)
    {
        yield return new WaitForSeconds(delay);

        //movement 클래스에 설정한 값에 따라 공격 시 대시
        movement.Dash(direction);

        enemyAnimation.Play("Attack");

        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, Vector2.one, 0, direction, distance);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag(gameObject.tag)) continue;

            ILivingEntity entity = hit.collider.GetComponent<ILivingEntity>();
            if (entity != null)
            {
                //타겟 사이 벽 체크
                RaycastHit2D hitWallCheck = Physics2D.Raycast(transform.position, hit.point - (Vector2)transform.position, distance, groundLayer);
                if (hitWallCheck) continue;

                entity.TakeDamage(damage, gameObject);
            }
        }

        attackDirection = Vector3.zero;

        StartCoroutine("Cooldown", cooldown);
    }

    private IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);

        isAttacking = false;
        enemyAnimation.Play("Idle");
    }

    public Vector3 GetAttackDirection()
    {
        return attackDirection;
    }

    public float GetAttackDistance()
    {
        return distance;
    }
}
