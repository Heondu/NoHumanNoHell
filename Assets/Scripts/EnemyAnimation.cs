using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 애니메이션을 제어하는 클래스
/// 업데이트에서 상태 변이를 제어하며
/// 필요 시 Play 함수를 통해 직접 애니메이션을 제어한다.
/// </summary>
public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Movement movement;
    private EnemyAttack enemyAttack;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<Movement>();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    private void Update()
    {
        //이동 애니메이션
        animator.SetBool("isMove", movement.GetMoveDirection().x != 0 ? true : false);

        //좌우 반전 - 우선 순위 1. 공격 중일 때, 2. 이동 중일 때
        if (enemyAttack.GetAttackDirection().x != 0) spriteRenderer.flipX = enemyAttack.GetAttackDirection().x < 0 ? true : false;
        else if (movement.GetMoveDirection().x != 0) spriteRenderer.flipX = movement.GetMoveDirection().x < 0 ? true : false;

        //점프 애니메이션
        animator.SetBool("isInAir", !movement.IsGrounded());
    }

    public void Play(string name)
    {
        animator.Rebind();
        animator.Play(name);
    }
}
