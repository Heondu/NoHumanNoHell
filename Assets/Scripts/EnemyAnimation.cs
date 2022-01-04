using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� �ִϸ��̼��� �����ϴ� Ŭ����
/// ������Ʈ���� ���� ���̸� �����ϸ�
/// �ʿ� �� Play �Լ��� ���� ���� �ִϸ��̼��� �����Ѵ�.
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
        //�̵� �ִϸ��̼�
        animator.SetBool("isMove", movement.GetMoveDirection().x != 0 ? true : false);

        //�¿� ���� - �켱 ���� 1. ���� ���� ��, 2. �̵� ���� ��
        if (enemyAttack.GetAttackDirection().x != 0) spriteRenderer.flipX = enemyAttack.GetAttackDirection().x < 0 ? true : false;
        else if (movement.GetMoveDirection().x != 0) spriteRenderer.flipX = movement.GetMoveDirection().x < 0 ? true : false;

        //���� �ִϸ��̼�
        animator.SetBool("isInAir", !movement.IsGrounded());
    }

    public void Play(string name)
    {
        animator.Rebind();
        animator.Play(name);
    }
}
